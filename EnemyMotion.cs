//using System;
//using System.IO;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

//namespace OOP_RPG
//{
//    internal class EnemyMotion
//    {
//        private readonly string folderPath;
//        private readonly string[] files;

//        public EnemyMotion()
//        {
//            //フォルダパス取得
//            folderPath = $@"{ExtFile.Path}\EnemyImage";
//            //画像のパス取得
//            files = Directory.GetFiles(folderPath);
//            //CreatePBにすべての画像パスを渡す
//            if (Program.bf != null)
//            {
//                Program.bf.CreatePB(files);
//                //MessageBox.Show($"{file}");
//            }
//        }
//        public void HeroAttackMotion()
//        {

//        }
//    }
//}
