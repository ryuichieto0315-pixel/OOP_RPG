namespace OOP_RPG
{
    using Elm = Tuple<ulong, ITurn>;

    /// <summary>
    /// 戦闘キュー
    /// </summary>
    internal static class BattleQueue
    {
        private static ulong _count = 0;

        private static readonly PriorityQueue<Elm, ulong> _queue = new();

        /// <summary>
        /// 静的コンストラクター
        /// </summary>
        static BattleQueue()
        {
            _count = 0;
            _queue.Clear();
        }

        /// <summary>
        /// キューにエントリー
        /// </summary>
        /// <param name="turn">キャラクターのインスタンス</param>
        /// <param name="wait">待ち時間</param>
        public static void Enq(ITurn turn, uint wait)
        {
            _count++;
            ulong cnt = _count + wait;
            _queue.Enqueue(new Elm(cnt, turn), cnt);
        }

        /// <summary>
        /// 強制的に先頭に挿入する
        /// </summary>
        /// <param name="turn">キャラクターのインスタンス</param>
        public static void EnqHead(ITurn turn)
        {
            _count++;
            _queue.Enqueue(new Elm(0, turn), 0);
        }

        /// <summary>
        /// キューが空かどうか
        /// </summary>
        /// <returns>空ならtrue</returns>
        public static bool IsEmpty()
        {
            return _queue.Count == 0;
        }

        /// <summary>
        /// キューのエントリーをすべてクリアする
        /// </summary>
        public static void Clear()
        {
            _queue.Clear();
        }

        /// <summary>
        /// キューから先頭のエントリーを抽出
        /// </summary>
        /// <returns>
        /// countが_countより小さい時ITurnのインスタンスを返す</br>
        /// そうでなければnullを返す</br>
        /// </returns>
        public static ITurn? Deq()
        {
            _count++;

            if (IsEmpty()) return null;

            var elm = _queue.Dequeue();
            if (elm == null) return null;

            if (elm.Item1 > _count)
            {
                _queue.Enqueue(elm, elm.Item1);
                return null;
            }

            return elm.Item2;
        }
    }
}
