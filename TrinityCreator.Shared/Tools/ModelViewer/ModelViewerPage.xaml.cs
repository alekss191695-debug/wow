using System;
using System.Windows;
using System.Windows.Controls;
using TrinityCreator.Shared.Database;

namespace TrinityCreator.Shared.Tools.ModelViewer
{
    /// <summary>
    /// Логика взаимодействия для ModelViewerPage.xaml
    /// </summary>
    public partial class ModelViewerPage : Page
    {
        public ModelViewerPage()
        {
            InitializeComponent();
            Global.ModelViewer = this;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadModel();
        }

        public void LoadModel(int displayId = 0, string type = "", string idType = "")
        {
            if (displayId == 0)
            {
                try
                {
                    displayId = int.Parse(displayIdTxt.Text);
                }
                catch
                {
                    MessageBox.Show("Некорректный ID отображения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                displayIdTxt.Text = displayId.ToString();
            }

            // Внутренняя логика и вызывающий код используют прежние английские идентификаторы.
            if (string.IsNullOrEmpty(type))
            {
                type = displayTypeCb.SelectedIndex == 1 ? "Creature" : "Item";
            }
            else
            {
                switch (type)
                {
                    case "Item":
                        displayTypeCb.SelectedIndex = 0;
                        break;
                    case "Creature":
                        displayTypeCb.SelectedIndex = 1;
                        break;
                }
            }

            if (string.IsNullOrEmpty(idType))
            {
                idType = inputIdCb.SelectedIndex == 1 ? "Entry ID" : "Display ID";
            }
            else
            {
                switch (idType)
                {
                    case "Display ID":
                        inputIdCb.SelectedIndex = 0;
                        break;
                    case "Entry ID":
                        inputIdCb.SelectedIndex = 1;
                        break;
                }
            }

            switch (type)
            {
                case "Item":
                    if (idType == "Entry ID")
                        displayId = LookupQuery.GetItemDisplayFromEntry(displayId);
                    mvBrowser.LoadUrl("http://www.wowhead.com/#modelviewer:4:13;" + displayId, blankFirst: true);
                    break;
                case "Creature":
                    if (Connection.IsConfigured())
                    {
                        if (idType == "Display ID")
                            displayId = LookupQuery.GetCreatureIdFromDisplayId(displayId);
                        mvBrowser.LoadUrl("http://www.wowhead.com/npc=" + displayId + "/#modelviewer:10+0");
                    }
                    break;
            }
        }
    }
}
