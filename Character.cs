using System;

namespace OOP_RPG
{
    /// <summary>
    /// Character抽象クラス
    /// </summary>
    internal abstract class Character : IBattle
    {
        public string Name { get; }
        public uint Hp { get; private set; }
        public uint Mp { get; private set; }
        public uint HpMax { get; private set; }
        public uint MpMax { get; private set; }
        public uint Speed { get; private set; }
        public uint Str { get; private set; }
        public bool Pinch { get; private set; } = false;
        public uint DefendDamage { get; set; } = 100;
        public Character? CoveredBy { get; set; }
        public bool Covered { get; set; } = false;


        public bool Normal
        {
            get
            {
                return Hp > 1 && !Sleeping;
            }
        }
        public bool NotAlive { get { return Hp == 0; } }
        public bool SpellCasting
        {
            get
            {
                //遅延実行が魔法の時true
                return (delayedAction != ""
                    && CommandList.Spells[delayedAction].Kind == "Magic");
            }
        }
        public bool Sleeping { get; protected set; } = false;
        public uint SleepCount { get; protected set; } = 0;
        public bool Poisoned { get; protected set; } = false;


        //魔法のように次ターンで実行するアクションとターゲットを保存
        //delayedAction == ""なら次ターンは通常
        protected string delayedAction = "";
        protected string delayedTarget = "";

        public bool Annihilation { get; private set; } = false;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="hp">Hit Point</param>
        /// <param name="mp">Magic Point</param>
        /// <param name="speed">速さ</param>
        public Character(string name, uint hp, uint mp, uint speed, uint str)
        {
            Name = name;
            Hp = hp;
            Mp = mp;
            HpMax = hp;
            MpMax = mp;
            Speed = speed;
            Str = str;
        }

        /// <summary>
        /// 相手からの攻撃を受ける
        /// </summary>
        /// <param name="byChar">攻撃したキャラクター</param>

        public async void AttackedBy(Character byChar)
        {
            TraceLog.Write($"{Name}: {byChar.Name}から攻撃を受けた");

            uint damage = (byChar.Str * DefendDamage / 100);    //防御時ダメージ軽減
            if (damage == 0) damage = 1;

            // ダメージ計算
            if (Hp > damage)
            {
                Hp -= damage;
                Pinch = (100 * Hp / HpMax < 31);
            }
            else
            {
                Hp = 0;
                Pinch = false;
            }

            // UI更新
            if (GameMaster.BattleField != null)
            {
                UpdateCharacterUI(this);     // 攻撃対象のUI更新
                UpdateCharacterUI(byChar);   // 攻撃者のUI更新（MPなど）
            }

            //FrmBattleField.panel1.Refresh();
            await Task.Delay(200);
            TraceLog.Write($"{damage}のダメージ！");

            // 倒された場合の処理
            if (Hp == 0)
            {
                GameMaster.Buster(this);
                Uncover();
                this.Poisoned = false;
                this.Sleeping = false;
                this.SleepCount = 0;
                this.DefendDamage = 100;
                this.Pinch = false;

                if (Annihilation)
                {
                    GameMaster.Annihilation(this);
                }
            }
        }

        // 共通のUI更新メソッド
        private void UpdateCharacterUI(Character character)
        {
            if (GameMaster.BattleField != null)
            {
                if (character is Hero hero)
                {
                    GameMaster.BattleField.UpdateHeroStatus(
                        hero.Name,
                        hero.Hp,
                        hero.HpMax,
                        hero.Mp,
                        hero.MpMax
                    );
                }
                else if (character is Enemy enemy)
                {
                    GameMaster.BattleField.UpdateEnemyStatus(enemy);
                }
            }
        }

        /// <summary>
        /// 相手からの魔法を受ける
        /// </summary>
        /// <param name="spell">魔法の名前</param>
        /// <param name="byChar">魔法をかけたキャラクター</param>
        public async void CastSpellOf(string spell, Character byChar)
        {
            TraceLog.Write($"{Name}: {byChar.Name}の魔法「{spell}」を受けた");

            if (!CommandList.Spells.TryGetValue(byChar.delayedAction, out var command))
                return; // コマンドが存在しなければ中断

            //MPが足りない場合
            if (byChar.Mp < command.UseMp)
            {
                TraceLog.Write($"{byChar.Name}は「{spell}」を発動しようとしたが、MPが足りなかった");
                return;
            }

            uint amount = command.TakeHp; // 魔法効果量
            byChar.Mp -= command.UseMp;    // 攻撃者のMP消費

            switch (spell)
            {
                case "メラ":
                    {
                        // HPが残る場合
                        if (Hp > (amount * DefendDamage / 100))
                        {
                            Hp -= (amount * DefendDamage / 100);
                            Pinch = (100 * Hp / HpMax < 31);
                        }
                        else
                        {
                            Hp = 0;
                            Pinch = false;
                        }

                        TraceLog.Write($"{(amount * DefendDamage / 100)}のダメージ！");
                        break;

                    }
                case "ホイミ":

                    // 回復量が最大HPを超えないように調整
                    uint healAmount = Math.Min(amount, HpMax - Hp);
                    Hp += healAmount;
                    Pinch = (100 * Hp / HpMax < 31);

                    TraceLog.Write($"{healAmount}回復した！");
                    break;

                case "ヴェレ":
                    if (!Poisoned)
                    {
                        TraceLog.Write($"{Name}を毒状態にした！");
                        Poisoned = true;
                    }
                    else TraceLog.Write($"{Name}はすでに毒状態だ！");
                    break;

                case "ラリホー":

                    if (!Sleeping)
                    {
                        TraceLog.Write($"{Name}を眠らせた！");
                        Sleeping = true;
                        SleepCount = 3;
                    }
                    else TraceLog.Write($"{Name}はすでに眠っている！");
                    break;


            }

            // 攻撃者と対象のUIを更新
            if (GameMaster.BattleField != null)
            {
                UpdateCharacterUI(this);     // 対象
                UpdateCharacterUI(byChar);   // 攻撃者
            }

            await Task.Delay(200);

            if (Hp == 0)
            {
                GameMaster.Buster(this);
                Uncover();
                this.Poisoned = false;
                this.Sleeping = false;
                this.SleepCount = 0;
                this.DefendDamage = 100;
                this.Pinch = false;


                if (Annihilation)
                    GameMaster.Annihilation(this);
            }
        }


        //全滅判定
        public void JudgeWinner(Character it, bool judge)
        {
            it.Annihilation = judge;
            //MessageBox.Show($"Annihilation:{Annihilation}");
        }
        public virtual void Uncover() { }

        //毒ダメージ判定
        private uint PoisonDamage = 5;

        private void DamagedByPoison()
        {
            if (!this.Poisoned) return;     //毒じゃないなら処理終了
            TraceLog.Write($"{this.Name}は毒のダメージを受けた\r\n" +
                           $"{Math.Min(this.Hp, PoisonDamage)}のダメージ！");
            this.Hp = (uint)Math.Max((int)0, (int)this.Hp - (int)PoisonDamage);  //Math.Maxは2つの引数の値が大きい方を返すメソッド

            if (GameMaster.BattleField != null)
            {
                UpdateCharacterUI(this);     //UI更新
            }


            if (this.Hp != 0) return;   //毒ダメージで戦闘不能にならなかった場合処理終了
            GameMaster.Buster(this);

            if (Annihilation) GameMaster.Annihilation(this);    //全滅判定
        }

        //眠り状態判定
        private void SkipTurnToSleep()
        {
            if (!this.Sleeping) return;     //眠りじゃないなら処理終了
            this.SleepCount--;
            if (this.SleepCount > 0) TraceLog.Write($"{this.Name}は眠っている");
            else
            {
                TraceLog.Write($"{this.Name}は目を覚ました！");
                this.Sleeping = false;
            }
        }

        //ターン開始時処理
        public void StartTurn()
        {
            Uncover();  //庇う解除
            DamagedByPoison();  //毒判定
            SkipTurnToSleep();  //眠り判定
            this.DefendDamage = 100;    //ダメージ軽減処理解除
        }

    }
}

