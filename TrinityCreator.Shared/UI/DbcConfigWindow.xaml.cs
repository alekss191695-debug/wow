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
using System.Windows.Shapes;
using System.IO;
using TrinityCreator.Shared.DBC;

namespace TrinityCreator.Shared.UI
{
    /// <summary>
    /// Interaction logic for DbcConfigWindow.xaml
    /// </summary>
    public partial class DbcConfigWindow : Window
    {
        public DbcConfigWindow()
        {
            InitializeComponent();
            dbcDirTxt.Text = Properties.Settings.Default.DbcDir;
            ShowInfo();
        }

        private void ShowInfo()
        {
            infoTxt.Text = string.Format("Эта настройка необязательна и используется только инструментом поиска в некоторых случаях.{0}" +
                "Если у вас уже есть папка DBC, укажите её здесь.{0}" +
                "Также можно указать папку WoW для автоматического извлечения файлов DBC.{0}{0}" +
                "Для корректной работы должны быть файлы DBC версии 3.3.5a. Оставить пустым тоже можно."
                , Environment.NewLine);
        }

        private void selectBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = new System.Windows.Forms.FolderBrowserDialog();
            var r = d.ShowDialog();
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                dbcDirTxt.Text = d.SelectedPath;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            string dir = dbcDirTxt.Text;
            if (File.Exists(dir + @"\Wow.exe")) // Is wow dir, add dbc to path
                dir += @"\dbc";

            // Save path for verification
            string oldDbcPath = Properties.Settings.Default.DbcDir;
            Properties.Settings.Default.DbcDir = dir;
            Properties.Settings.Default.Save();

            // Verify & notify or undo
            if (DbcHandler.VerifyDbcDir(openConfigWindowOnError: false))
            {
                MessageBox.Show("Путь к DBC настроен корректно.", "Настройки сохранены", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                Properties.Settings.Default.DbcDir = oldDbcPath;
                Properties.Settings.Default.Save();
            }
        }
    }
}
