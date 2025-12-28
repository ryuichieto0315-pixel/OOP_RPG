using System;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace OOP_RPG
{
    /// <summary>
    /// Enemyクラス: コンストラクター
    /// </summary>
    internal class Enemy : Character, ITurn
    {
        public string Kind { get; }

        private static IRand _rand = new Rand();

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="hp">Hit Point</param>
        /// <param name="mp">Magic Point</param>
        /// <param name="speed">速さ</param>
        /// <param name="kind">種族</param>
        public Enemy(string name, uint hp, uint mp, uint speed, uint str, string kind)
            : base(name, hp, mp, speed, str)
        {
            Kind = kind;
        }

        /// <summary>
        /// ユニットテスト用のRandモックを注入(inject)
        /// </summary>
        /// <param name="mock">ユニットテスト用IRandモック</param>
        public static void InjectRandMock(IRand mock)
        {
            _rand = mock;
        }

        /// <summary>
        /// 今ターンのアクションとターゲットの決定
        /// </summary>
        /// <returns>(次ターンのアクション、次ターンのターゲット)</returns>
        private (string act, string tar) MakeDecision()
        {
            string[] commands = [.. CommandList.Commands.Keys];
            string[] spells = [.. CommandList.Spells.Keys];
            string act;
            string actTarget;
            //MPが十分にあり、HPが30%以下の場合は回復魔法が選択肢に入る
            //if (this.Mp >= CommandList.Spells["ホイミ"].UseMp && CharacterList.Enemies.Any(x => x.Value.Pinch))
            //{
            //	act = commands[_rand.Next(3)];
            //	//MessageBox.Show($"挙動1{this.Mp}_{CommandList.Commands["回復魔法"].UseMp}");
            //}
            ////MPが十分にある場合、攻撃か攻撃魔法を使用する
            //else if (this.Mp >= CommandList.Spells["メラ"].UseMp)
            //{
            //	//MessageBox.Show($"挙動2{this.Mp}_{CommandList.Commands["攻撃魔法"].UseMp}");
            //	act = commands[_rand.Next(2)];
            //}
            ////なにも条件を満たさなかった場合、攻撃する
            //else
            //{
            //	//ランダムに行動を決定(_rand.Nextは0～引数の値-1をランダムに決定する。この場合はコマンドの数を引数にしている)
            //	//act = commands[_rand.Next(CommandList.Commands.Count)];
            //	act = commands[0];
            //}

            if (this.Mp >= CommandList.Spells.Values.Min(x => x.UseMp)) //呪文を使用するのにMPが足りているかチェック
            {
                var ableSpells = CommandList.Spells.Values.Where(x => x.UseMp <= this.Mp).Select(x => x.Action).ToList();    //使用できる呪文リストを作成
                if (ableSpells.Contains("戻る")) ableSpells.Remove("戻る"); //多分ないが、戻るが混じっていたら排除
                ableSpells.Add("攻撃");   //攻撃も選択肢に追加

                if (ableSpells.Contains("ホイミ") && CharacterList.Enemies.Any(x => x.Value.Pinch)) { }    //ピンチの敵がいなければ、ホイミを選択肢から排除
                else ableSpells.Remove("ホイミ");

                act = ableSpells[_rand.Next(ableSpells.Count)]; //使用できる呪文+攻撃からランダムに決定
                if (act != "攻撃") actTarget = CommandList.Spells[act].Target; //呪文が選択された場合、呪文リストを参照してターゲット決定
                else actTarget = CommandList.Commands[act].Target;
            }
            else
            {
                act = "攻撃";
                actTarget = CommandList.Commands[act].Target;
            }

            string[] targets = [];
            switch (actTarget)
            {
                case "敵方":
                    targets = CharacterList.Heroes.Where(x => x.Value.Hp > 0).Select(x => x.Key).ToArray(); //死亡したキャラはターゲット対象外に
                    //targets = [.. CharacterList.Heroes.Keys];
                    break;

                case "味方":
                    targets = CharacterList.Enemies.Where(x => x.Value.Hp > 0).Select(x => x.Key).ToArray(); //死亡したキャラはターゲット対象外に
                    //targets = [.. CharacterList.Enemies.Keys];
                    break;

                case "自分":
                    targets = [this.Name];
                    break;
            }
            if (targets.Length == 0) return ("", "");
            int t = _rand.Next(targets.Length);
            string tar = targets[t];

            return (act, tar);
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
            if (this.NotAlive) return;//毒で戦闘不能になった場合のため

            if (this.Sleeping)
            {
                BattleQueue.Enq(this, this.Speed);
                return;
            }


            if (this.SpellCasting)
            {
                var tar = CharacterList.AllChars[this.delayedTarget];

                // 魔法詠唱中
                TraceLog.Write($"{Name}: {this.delayedAction}を詠唱中である");
                //ターゲットが生存しているか確認
                string[] targets = [];
                if (tar.NotAlive && tar is Hero)
                {
                    targets = CharacterList.Heroes.Where(x => x.Value.Hp > 0).Select(x => x.Key).ToArray();
                    this.delayedTarget = targets[_rand.Next(targets.Length)];
                }
                else if (tar.NotAlive && tar is Enemy)
                {
                    targets = CharacterList.Enemies.Where(x => x.Value.Hp > 0).Select(x => x.Key).ToArray();
                    this.delayedTarget = targets[_rand.Next(targets.Length)];
                }

                tar = CharacterList.AllChars[this.delayedTarget];

                // 魔法を発動する
                if (this.delayedAction == "ホイミ") GameMaster.BattleField?.AttackMotion(CharacterList.Enemies[this.delayedTarget],
                                                                                         CharacterList.CreateAttackGif(this.delayedAction));
                TraceLog.Write($"{Name}: {this.delayedTarget}への{this.delayedAction}が発動");

                if (tar is Hero && tar.CoveredBy is Hero coverHero)
                {
                    TraceLog.Write($"{coverHero.Name}が{tar.Name}を庇った！");
                    coverHero.CastSpellOf(this.delayedAction, this);
                }
                else
                {
                    tar.CastSpellOf(this.delayedAction, this);
                }

                // 次ターンで遅延実行はしないので状態クリア
                this.delayedAction = "";
                this.delayedTarget = "";
            }
            else
            {
                var (act, tar) = MakeDecision();
                if (act == "")
                {
                    TraceLog.Write($"{Name}:{Name}は怠けている");
                    return;
                }
                //this.DefendDamage = 100;
                switch (act)
                {
                    case "メラ":
                    case "ホイミ":
                    case "ヴェレ":
                    case "ラリホー":
                        // 魔法詠唱中に遷移
                        TraceLog.Write($"{Name}: {tar}に{act}を唱えた");
                        this.delayedAction = act;
                        this.delayedTarget = tar;
                        break;

                    case "攻撃":
                        // ターゲットに対して攻撃
                        TraceLog.Write($"{Name}: {tar}に{act}した");
                        if (!(CharacterList.AllChars[tar].CoveredBy is Hero coverHero))
                        {
                            CharacterList.AllChars[tar].AttackedBy(this);
                        }
                        else
                        {
                            TraceLog.Write($"{coverHero.Name}が{tar}を庇った！");
                            coverHero.AttackedBy(this);
                        }
                        break;

                    case "防御":
                        TraceLog.Write($"{Name}: {tar}を{act}している");
                        this.DefendDamage = 20;
                        break;

                    case "庇う":
                        TraceLog.Write($"{Name}: {tar}を{act}態勢に入った");
                        this.DefendDamage = 50;
                        break;
                }
            }

            // @@バトルキューに挿入@@
            BattleQueue.Enq(this, this.Speed);
        }
    }
}