using System.Xml.Linq;
using System.Threading;

namespace OOP_RPG
{
    //静的クラス：インスタンスを生成せずに(newせずに)使用する⇒そもそもnewできない
    //静的フィールド変数、静的メソッドのみが定義されている
    //シングルトンパターンに似ているが、これを継承することはできない点が異なる。
    internal static class GameMaster
    {
        // @@画面を作成した後コメントを外す
        public static FrmBattleField? BattleField { get; private set; }

        // 静的コンストラクター：このクラス初回使用時に1回だけ実行
        static GameMaster()
        {
            // キューに全キャラクターを挿入
            EnqAllChars();
        }

        // @@画面を作成した後コメントを外す
        public static void SetBattleField(FrmBattleField battleField)
        {
            BattleField = battleField;
        }

        public static void EnqAllChars()
        {
            //@@画面を作成した後コメントを外す@@
            foreach (var it in CharacterList.Heroes)
            {
                BattleQueue.Enq(it.Value, it.Value.Speed);
            }

            foreach (var it in CharacterList.Enemies)
            {
                BattleQueue.Enq(it.Value, it.Value.Speed);
            }
        }

        /// <summary>
        /// 次のキャラクターにYourTurnを送る
        /// </summary>
        public static async void Run()
        {
            var turn = BattleQueue.Deq();
            if (turn == null) return;
            //全滅チェック
            if (CharacterList.AllChars.Any(x => x.Value.Annihilation)) await Task.Delay(3000);

            //TraceLog.Write($"GameMaster: {((Character)turn).Name}にターンを渡す");
            turn.YourTurn();
        }

        /// <summary>
        ///  コンストラクターを動かすためのダミーメソッド
        ///  ユニットテスト用
        /// </summary>
        public static void Dummy()
        {
            //何もしない
        }

        //戦闘不能処理
        public static void Buster(Character buster)
        {
            bool f = false;
            if (buster is Enemy)
            {
                TraceLog.Write($"\r\n{buster.Name}を倒した！");
                foreach (var it in CharacterList.Enemies)
                {
                    if (it.Value.Hp != 0)   //HPが0じゃないキャラがいたらフラグをfalseにして処理終了
                    {
                        f = false;
                        break;
                    }
                    f = true;
                }
                buster.JudgeWinner(buster, f);  //全滅チェックの結果を渡す、ここでtrueの時だけ呼ぶようにすればよかった
            }
            else
            {
                TraceLog.Write($"\r\n{buster.Name}は死んでしまった");
                foreach (var it in CharacterList.Heroes)
                {
                    if (it.Value.Hp != 0)   //HPが0じゃないキャラがいたらフラグをfalseにして処理終了
                    {
                        f = false;
                        break;
                    }
                    f = true;
                }
                buster.JudgeWinner(buster, f);  //全滅チェックの結果を渡す、ここでtrueの時だけ呼ぶようにすればよかった
            }
        }


        //戦闘終了処理
        public static async void Annihilation(Character looser)
        {
            if (looser is Enemy)
            {
                TraceLog.Write($"\r\n{looser.Name}達を倒した！！");
            }
            else
            {
                TraceLog.Write($"\r\n{looser.Name}達は全滅した...");
            }
            //Annihilation2 = true;
            if (BattleField != null && !BattleField.IsDisposed)
            {
                //3秒停止後フォーム終了(暫定処理)
                //Thread.Sleep(3000);
                await Task.Delay(3000);
                BattleField.Close();
                BattleField = null;
            }
        }
    }
}