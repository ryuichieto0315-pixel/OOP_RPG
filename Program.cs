using System.Windows.Forms;

namespace OOP_RPG
{
    internal static class Program
    {
        public static FrmBattleField? bf;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ゲーム画面の生成
            bf = new FrmBattleField();
            foreach (var it in CharacterList.Heroes)
            {
                bf.HeroLabel(it.Value.Name, it.Value.Hp, it.Value.Hp, it.Value.Mp, it.Value.Mp);
            }

            // GameMasterにゲーム画面の参照をセット
            GameMaster.SetBattleField(bf);
            //EnemyMotion enemy = new EnemyMotion();
            // TraceLogにゲーム画面の参照をセット
            TraceLog.SetBattleField(bf);
            TraceLog.Write("バトル開始！");
            Application.Run(bf);
        }
    }
}