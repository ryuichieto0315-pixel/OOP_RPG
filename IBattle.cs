namespace OOP_RPG
{
    /// <summary>
    /// 相手からの攻撃を受けるインターフェイス
    /// </summary>
    internal interface IBattle
    {
        ///<summary>
        ///相手からの攻撃を受ける
        /// </summary>
        /// <param name="byChar">攻撃したキャラクター</param>
        public void AttackedBy(Character byChar);

        ///<summary>
        ///相手からの攻撃を受ける
        /// </summary>
        /// <param name="spell">魔法の名前</param>
        /// <param name="byChar">魔法をかけたキャラクター</param>
        public void CastSpellOf(string spell, Character byChar);
    }
}