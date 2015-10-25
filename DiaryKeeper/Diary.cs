using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaryKeeper
{
    /**
     * 日記管理クラス
     */
    class Diary
    {
        /**
         * 特定の階層から最も古いディレクトリを選択する
         * @param folders 選択肢となるフォルダ群
         * @detail 数字のみで構成されたリストから、最小の数字の要素を選ぶ
         */
        private int getMinimumDirectory(List<String> folders)
        {
            int min = int.MaxValue;
            foreach(String i in folders){
                int num = Int32.Parse(i);
                if(num < min) min = num;
            }

            return min;
        }

        /**
         * 日記が書かれている最も古い月を取得する
         */
        public DateTime findOldestMonth()
        {
            string currentDir = System.IO.Directory.GetCurrentDirectory();

            int year = getMinimumDirectory(getDirectories(currentDir));
            int month = getMinimumDirectory(getDirectories(currentDir + "\\" + year));

            try
            {
                DateTime oldest = new DateTime(year, month, 1);

                return oldest;
            }
            catch(Exception e)
            {
                return DateTime.Today;
            }
        }

        /**
         * ディレクトリ一覧を取得する
         * @param path 取得する場所のパス
         */
        private List<String> getDirectories(String path)
        {
            List<String> list = new List<String>();
            try
            {
                String[] dirs = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.TopDirectoryOnly);
                foreach (String dir in dirs)
                {
                    list.Add(System.IO.Path.GetFileName(dir));
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }

        /**
         * 日記のファイル名を取得する
         * @param day 日記の日付
         */
        private static String getFormatedFilename(DateTime day)
        {
            return String.Format(@"{0:0000}\{1:00}\{2:00}.txt", day.Year, day.Month, day.Day);
        }

        /**
         * 日記が書かれているか調べる
         * @param day 日記の日付
         */
        public static bool checkExistFile(DateTime day)
        {
            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\" + getFormatedFilename(day)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * 日記を保存する
         * @param text 保存するテキスト
         * @param day 保存する日付
         */
        public void saveDiary(string text, DateTime day)
        {
            string currentDir = System.IO.Directory.GetCurrentDirectory();

            if (prepareDirectory(day) == false)
            {
                return;
            }

            Cipher cp = new Cipher();
            string encript = cp.Encrypt(text);
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(currentDir + "\\" + getFormatedFilename(day), false,
    System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(encript);
                    sw.Close();
                }
            }
            catch (Exception e)
            {

            }
        }

        /**
         * 日記が最後に編集された時間を取得する
         * @param written 取得する日記の日付
         */
        public DateTime getLastEditedTime(DateTime written)
        {
            string currentDir = System.IO.Directory.GetCurrentDirectory();

            DateTime edited;
            try
            {
                edited = System.IO.File.GetLastWriteTime(currentDir + "\\" + getFormatedFilename(written));
            }
            catch
            {
                edited = DateTime.MinValue;
            }

            return edited;
        }

        /**
         * ディレクトリを用意する
         */
        private bool prepareDirectory(DateTime date)
        {
            string currentDir = System.IO.Directory.GetCurrentDirectory();

            try
            {
                System.IO.Directory.CreateDirectory(currentDir + "\\" + date.Year + "\\" + date.Month);
            }
            catch (UnauthorizedAccessException e)
            {
                System.Windows.MessageBox.Show("Unable to create directory at " + currentDir + "\\" + date.Year + "\\" + date.Month);
                return false;
            }
            catch (Exception e)
            {
                
            }

            return true;
        }

        /**
         * 日記の内容を取得する
         * @param date 取得する日記の日付
         */
        public String loadDiaryFile(DateTime date)
        {
            String file = "";
            string currentDir = System.IO.Directory.GetCurrentDirectory();

            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(currentDir + "\\" + getFormatedFilename(date), Encoding.GetEncoding("shift-jis")))
                {
                    file = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {

            }

            Cipher cp = new Cipher();

            return cp.Decrypt(file);
        }
    }
}
