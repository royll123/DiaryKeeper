using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiaryKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Diary diary;

        private int autosave_interval;

        private string original_text;

        public MainWindow()
        {
            InitializeComponent();

            diary = new Diary();
            blackout_range();

            // set Date to today
            datePicker.SelectedDate = DateTime.Today;

            autosave_interval = 5*60;
            diaryBox.Document.LineHeight = 1;
            original_text = "";

            // add shortcuts
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, saveText_shortcut));
            this.InputBindings.Add(new KeyBinding(ApplicationCommands.Save, new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S")));
        }
        
        /**
         * カレンダーに表示しない日付を指定する
         */
        private void blackout_range()
        {
            DateTime oldest = diary.findOldestMonth();
            datePicker.DisplayDateStart = oldest;
            datePicker.DisplayDateEnd = DateTime.Today;
        }

        /**
         * 日付が変更された時の処理・表示するテキストを変更する
         */
        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? nday = datePicker.SelectedDate;
            if (nday != null)
            {
                DateTime day = nday.Value;
                FlowDocument document = diaryBox.Document;
                TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
                range.Text = diary.loadDiaryFile(day);
                original_text = range.Text;
            }
        }

        /**
         * テキストボックス上のテキストを取得する
         */
        private string getDiaryBoxText()
        {
            FlowDocument document = diaryBox.Document;
            TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
            return range.Text;
        }

        /**
         * テキストを日記として保存する
         * @param text 保存するテキスト
         * @param day 日記の日付
         */
        private void saveText(string text, DateTime day)
        {
            diary.saveDiary(text, day);
        }

        /**
         * Ctrl+S keyが押された時に日記を保存する
         */
        public void saveText_shortcut(object target, ExecutedRoutedEventArgs e)
        {
            if (datePicker.SelectedDate != null)
            {
                saveText(getDiaryBoxText(), (DateTime)datePicker.SelectedDate);
            }
        }

        /**
         * 日記が編集されたら自動保存する
         */
        private void diaryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime now = DateTime.Now;

          
            if(datePicker.SelectedDate != null){
                DateTime lastEdited = diary.getLastEditedTime((DateTime)datePicker.SelectedDate.Value);
                lastEdited.AddSeconds(autosave_interval);
                if (now.CompareTo(lastEdited) >= 0)
                {
                    string text = getDiaryBoxText();
                    if (!text.Equals(original_text))
                    {
                        saveText(text, (DateTime)datePicker.SelectedDate);
                    }
                }
            }
        }
    }
}
