using System.Text;

namespace OOP_RPG
{
    /// <summary>
    /// トレースログ<br/>
    /// - 画面へのログ出力<br/>
    /// - ログバッファ管理<br/>
    /// </summary>
    internal class TraceLog
    {
        //  追加
        private static FrmBattleField? _battleField;

        //ログバッファ
        private static readonly StringBuilder _logBuffer = new();

        //ログバッファに書き込むかどうか
        private static bool _isLogBuffering = false;

        /// <summary>
        /// ゲーム画面インスタンスの参照をセット
        /// </summary>
        /// <param name="frmBattleField">ゲーム画面インスタンス</param>
        public static void SetBattleField(FrmBattleField frmBattleField)
        {
            _battleField = frmBattleField; 
        }

        ///<summary>
        ///ログバッファをクリアして、ロギングを開始する
        ///</summary>
        public static void StartLogBuffering()
        {
            _logBuffer.Clear();
            _isLogBuffering = true;
        }

        ///<summary>
        ///ロギングを停止する
        ///</summary>
        public static void StopLogBuffering()
        {
            _isLogBuffering = false;
        }

        ///<summary>
        ///ログバッファの内容を全て返して、ログバッファをクリアする
        ///</summary>
        ///<returns>ログバッファの内容</returns>
        public static string GetLog()
        {
            var tmp = _logBuffer.ToString();
            _logBuffer.Clear();
            return tmp;
        }

        ///<summary>
        ///トレースログ出力<br/>
        ///とりあえずはDebugトレース出力するだけだが、
        ///画面を作成した後は画面にもログを表示するように変更する
        ///</summary>
        ///<param name="message">ログメッセージ</param>
        public static void Write(string message)
        {
            //ログバッファに保存
            if (_isLogBuffering)
            {
                _logBuffer.Append(message);
                _logBuffer.Append("\r\n");
            }

            //xUnit でのテスト出力確認用
            Console.WriteLine(message);

            //@@画面にログ表示@@
            _battleField?.MsgLog(message);
        }
    }
}