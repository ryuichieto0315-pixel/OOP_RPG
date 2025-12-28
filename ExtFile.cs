namespace OOP_RPG
{
    public static class ExtFile
    {
        //public static readonly string Path =
        //    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
        //        @"\source\repos\OOP_RPG\data"; 
        //public static readonly string Path = "Data\\";
        public static string DataDir => Path.Combine(AppContext.BaseDirectory, "Data");

        public static string SpellCsv => Path.Combine(DataDir, "Spells.csv");

    }
}