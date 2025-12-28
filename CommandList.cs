namespace OOP_RPG
{
    /// <summary>
    /// アクションデータの定義
    /// </summary>
    //internal class Command
    //{
    //    public string Action { get; private set; } = "";
    //    public string Target { get; private set; } = "";
    //    public string Kind { get; private set; } = "";
    //    public uint TakeHp { get; private set; }
    //    public uint UseMp { get; private set; }
    //}

    //internal class Spell
    //{
    //    public string Name { get; private set; } = "";
    //    public string Target { get; private set; } = "";
    //    public string Kind { get; private set; } = "";
    //    public uint TakeHp { get; private set; }
    //    public uint UseMp { get; private set; }
    //}


    /// <summary>
    /// CommandList静的クラス
    /// </summary>
    internal static class CommandList
    {
        public static readonly Dictionary<string, Command> Commands = [];
        public static readonly Dictionary<string, Spell> Spells = [];
        //public static readonly Dictionary<string, BattleAction> Commands = [];

        /// <summary>
        /// 静的コンストラクター
        /// </summary>
        static CommandList()
        {
            LoadCommand();
            LoadSpell();
        }

        /// <summary>
        /// CSVファイルからコマンドデータをロード
        /// </summary>
        private static void LoadCommand()
        {
            try
            {
                var csv = Csv.Load<Command>($@"{ExtFile.DataDir}\Command.csv");
                foreach (var it in csv)
                {
                    Commands.Add(it.Action, it);
                }
            }
            catch(Exception ex)
            {
                TraceLog.Write($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private static void LoadSpell()
        {
            try
            {
                var csv = Csv.Load<Spell>($@"{ExtFile.DataDir}\Spell.csv");
                foreach (var it in csv)
                {
                    Spells.Add(it.Action, it);
                }
            }
            catch (Exception ex)
            {
                TraceLog.Write($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

    }
}