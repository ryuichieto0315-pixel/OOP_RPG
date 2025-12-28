namespace OOP_RPG
{
    /// <summary>
    /// Callbackデリゲート<br/>
    /// デリゲート(delegate)=メソッドをデータ型とするもの<br/>
    /// ここではCallbackという型(2つの引数を取り、戻り値ないのメソッド)を宣言する<br/>
    /// </summary>
    /// <param name="action"></param>
    /// <param name="target"></param>
    delegate void Callback(string action, string target);

    /// <summary>
    /// コールバックを受けるインターフェイス
    /// </summary>
    internal interface ICallback
    {
        /// <summary>
        /// プレイヤーが選択したアクションとターゲットを受け取って実施
        /// </summary>
        /// <param name="action">選ばれたアクション</param>
        /// <param name="target">選ばれたターゲット</param>
        public void Callback(string action, string target);
    }
}