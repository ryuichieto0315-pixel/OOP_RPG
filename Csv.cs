using System.Reflection;

namespace OOP_RPG
{
    /// <summary>
    /// CSVファイルアクセス
    /// </summary>
    internal static class Csv
    {
        /// <summary>
        /// CSVファイルのデータをロード<br/>
        /// CSVファイルからList<T>へ読み込み<br/>
        /// CSVファイルの項目名とクラスTのプロパティ名が一致<br/>
        /// </summary>
        /// <typeparam name="T">出力するクラス</typeparam>
        /// <param name="filePath"></param>
        /// <returns>List<T>で出力</returns>
        public static List<T> Load<T>(string filePath) where T : new()
        {
            var list = new List<T>();

            try
            {
                using StreamReader sr = new(filePath);

                var headerLine = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(headerLine)) return list;
                var header = headerLine.Split(',').Select(x => x.Trim()).ToArray();

                while (true)
                {
                    var line = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) break;

                    var data = line.Split(",").Select(x => x.Trim()).ToArray();

                    var rec = new T();

                    for (var i = 0; i < header.Length; i++)
                    {
                        if (typeof(T).GetProperty(header[i]) is not PropertyInfo prop) continue;

                        prop.SetValue(rec, Convert.ChangeType(data[i], prop.PropertyType));
                    }

                    list.Add(rec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(1);
            }

            return list;
        }
    }
}
