using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace OOP_RPG
{
	/// <summary>
	/// Heroクラス: プレイヤーが操作するキャラクター
	/// </summary>
	internal class Hero : Character, ITurn, ICallback
	{
		public char Gender { get; private set; }
		public string Profession { get; private set; }
		private static IRand _rand = new Rand();

		/// <summary>
		/// コンストラクター
		/// </summary>
		/// <param name="name">名前</param>
		/// <param name="hp">Hit Point</param>
		/// <param name="mp">Magic Point</param>
		/// <param name="speed">速さ</param>
		/// <param name="gender">性別</param>
		/// <param name="profession">職業</param>
		public Hero(string name, uint hp, uint mp, uint speed, uint str,
			char gender, string profession)
			: base(name, hp, mp, speed, str)
		{
			Gender = gender;
			Profession = profession;
		}

		/// <summary>
		/// 自分のターン
		/// </summary>
		public void YourTurn()
		{
			if (this.NotAlive) return;
			TraceLog.Write($"\r\n{Name}のターン！");
			//TraceLog.Write($"{Name}: ターンが移った");
			StartTurn();    //ターン開始処理
			if (this.NotAlive) return;  //毒で戦闘不能になった場合のため

			if (this.Sleeping)	//睡眠状態の場合はターンスキップ
			{
				BattleQueue.Enq(this, this.Speed);
				return;
			}

			if (this.SpellCasting)
			{
				var tar = CharacterList.AllChars[this.delayedTarget];

				//魔法詠唱中
				TraceLog.Write($"{Name}: {this.delayedAction}を詠唱中である");
				string[] targets = [];
				//ターゲットが生存しているか確認
				if (tar.NotAlive && tar is Enemy)
				{
					targets = CharacterList.Enemies.Where(x => x.Value.Hp > 0).Select(x => x.Key).ToArray();
					this.delayedTarget = targets[_rand.Next(targets.Length)];
				}
				else if (tar.NotAlive && tar is Hero)
				{
					targets = CharacterList.Heroes.Where(x => x.Value.Hp > 0).Select(x => x.Key).ToArray();
					this.delayedTarget = targets[_rand.Next(targets.Length)];
				}

				//ターゲットがEnemyの時攻撃モーション再生
				if (tar is Enemy) GameMaster.BattleField?.AttackMotion(CharacterList.Enemies[this.delayedTarget],
																	   CharacterList.CreateAttackGif(this.delayedAction));

				tar = CharacterList.AllChars[this.delayedTarget];

				//魔法を発動する
				TraceLog.Write($"{Name}: {this.delayedTarget}への{this.delayedAction}が発動");
				tar.CastSpellOf(this.delayedAction, this);

				//次ターンで遅延実行はしないので状態クリア
				this.delayedAction = "";
				this.delayedTarget = "";

				//@@バトルキューに挿入@@
				BattleQueue.Enq(this, this.Speed);
			}
			else
			{
				//@@画面入力@@
				GameMaster.BattleField?.ActivateCommand(this);
			}
		}

		/// <summary>
		/// プレイヤーが選択したアクションとターゲットを受け取って実施
		/// </summary>
		/// <param name="action">選ばれたアクション</param>
		/// <param name="target">選ばれたターゲット</param>
		public void Callback(string action, string target)
		{
			//ダメージ軽減倍率解除
			//this.DefendDamage = 100;
			//庇う庇われる状態解除
			//Uncover();
			string act = "";
			if (CommandList.Spells.ContainsKey(action)) act = "呪文";  //呪文選択時は呪文で返す
			else act = action;

				switch (act)
				{
					//case "攻撃魔法":
					case "呪文":
						//魔法詠唱中に遷移
						TraceLog.Write($"{Name}: {target}に{action}を唱えた");
						this.delayedAction = action;
						this.delayedTarget = target;
						break;

					case "攻撃":
						//ターゲットに対して攻撃gif再生
						GameMaster.BattleField?.AttackMotion(CharacterList.Enemies[target],
															 CharacterList.CreateAttackGif(action));

						TraceLog.Write($"{Name}: {target}を{action}した");
						CharacterList.AllChars[target].AttackedBy(this);
						break;

					case "防御":
						TraceLog.Write($"{Name}: {action}している{target}");
						this.DefendDamage = 20;	//80%軽減
						break;

					case "庇う":
						TraceLog.Write($"{Name}: {target}を{action}態勢に入った");
						this.DefendDamage = 50;	//50%軽減
						this.Covered = true;
						CharacterList.Heroes[target].CoveredBy = this;
						break;
				}

			//@@バトルキューに挿入@@
			BattleQueue.Enq(this, this.Speed);
		}
		//庇う状態解除
		public override void Uncover()
		{
			if (!this.Covered)   //庇う状態でないなら処理終了
				return;

			this.Covered = false;
			RemoveCoverFromHero();
		}

		//庇う対象キャラの庇われる状態解除
		private void RemoveCoverFromHero()
		{
			//このHeroが庇っているHeroを見つけて解除する
			foreach (var hero in CharacterList.Heroes.Values)
			{
				if (hero.CoveredBy == this)
				{
					hero.CoveredBy = null;
					break;
				}
			}
		}

	}
}
