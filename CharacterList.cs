using System.Xml.Linq;

namespace OOP_RPG
{
    /// <summary>
    /// CSV で受け渡しするデータの定義（味方側）
    /// </summary>
    internal class HeroEntity
    {
        public string Name { get; set; } = "";
        public uint Hp {  get; set; }
        public uint Mp { get; set; }
        public uint Speed { get; set; }
        public uint Str { get; set; }
        public char Gender { get; set; }
        public string Profession { get; set; } = "";
    }

    /// <summary>
    /// CSV で受け渡しするデータの定義（敵側）
    /// </summary>
    internal class EnemyEntity
    {
        public string Name { get; set; } = "";
        public uint Hp { get; set; }
        public uint Mp { get; set; }
        public uint Speed { get; set; }
        public uint Str { get; set; }
        public string Kind { get; set; } = "";
    }

    internal class EnemyImageEntity
    {
        public string Kind { get; set; } = "";
        public string imagePath { get; set; } = "";
    }

    internal class AttackGifEntity
    {
        public string Name { get; set; } = "";
        public string gifPath { get; set; } = "";
    }



    /// <summary>
    /// CharacterList 静的クラス
    /// </summary>
    internal static class CharacterList
    {
        public static readonly Dictionary<string, Hero> Heroes = [];
        public static readonly Dictionary<string, Enemy> Enemies = [];
        public static readonly Dictionary<string, Character> AllChars = [];
        public static readonly Dictionary<string, string> files = [];
        private static List<string>? _enemyImages;



        /// <summary>
        /// 静的コンストラクター
        /// </summary>
        static CharacterList()
        {
            CreateHero();
            CreateEnemy();
            //var enemyFiles = CreateEnemyImage();
            //// EnemyMotion のコンストラクタなど
            //Program.bf?.CreatePB(enemyFiles.ToArray());

        }

        /// <summary>
        /// CSVファイルからHeroインスタンスの作成
        /// </summary>
        private static void CreateHero()
        {
            try
            {
                var csv = Csv.Load<HeroEntity>($@"{ExtFile.DataDir}\Hero.csv");
                foreach (var it in csv)
                {
                    var hero = new Hero(it.Name, it.Hp, it.Mp, it.Speed, it.Str,
                                        it.Gender, it.Profession);
                    Heroes.Add(hero.Name, hero);
                    AllChars.Add(hero.Name, hero);
                }
            }
            catch (Exception ex)
            {
                TraceLog.Write($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// CSVファイルからEnemyインスタンスの作成
        /// </summary>
        private static void CreateEnemy()
        {
            try
            {
                var csv = Csv.Load<EnemyEntity>($@"{ExtFile.DataDir}\Enemy.csv");
                foreach (var it in csv)
                {
                    var enemy = new Enemy(it.Name, it.Hp, it.Mp, it.Speed, it.Str, it.Kind);
                    Enemies.Add(enemy.Name, enemy);
                    AllChars.Add(enemy.Name, enemy);
                }
            }
            catch (Exception ex)
            {
                TraceLog.Write($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// CSVファイルからEnemy画像の作成
        /// </summary>
        private static List<string> CreateEnemyImage()
        {
            List<string> files = new List<string>();

            try
            {
                var csv = Csv.Load<EnemyImageEntity>($@"{ExtFile.DataDir}\EnemyImages.csv");

                foreach (var ec in Enemies.Values)
                {
                    // CSV 内で最初にマッチしたものだけを取得
                    var match = csv.FirstOrDefault(it => ec.Kind == it.Kind);
                    if (match != null)
                    {
                        files.Add($@"{ExtFile.DataDir}\EnemyImage\" + match.imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.Write($"Error: {ex.Message}");
                Environment.Exit(1);
            }

            return files;
        }
        public static IReadOnlyList<string> GetEnemyImages()
        {
            if (_enemyImages == null)   //最初の1回だけ作成
            {
                _enemyImages = CreateEnemyImage();
            }
            return _enemyImages;
        }

        /// <summary>
        /// CSVファイルからgif情報の取得
        /// </summary>
        public static string? CreateAttackGif(string name)
        {
            try
            {
                var csv = Csv.Load<AttackGifEntity>(
                    Path.Combine(ExtFile.DataDir, "AttackMotion.csv"));

                var match = csv.FirstOrDefault(it => name == it.Name);

                if (match != null)
                {
                    return Path.Combine(ExtFile.DataDir, "AttackGif", match.gifPath);
                }
            }
            catch (Exception ex)
            {
                TraceLog.Write($"Error: {ex.Message}");
                Environment.Exit(1);
            }

            return null; // 見つからなかった場合
        }
    }
}