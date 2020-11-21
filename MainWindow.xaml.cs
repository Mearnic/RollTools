using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RollTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DBManager dBManager = null;
        public MainWindow()
        {
            InitializeComponent();
            string directory = Environment.CurrentDirectory + "/data";
            string dbPath = directory + "/SqliteModel.db";
            dBManager = new DBManager(dbPath);
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(dbPath))
            {
                dBManager.NewDbFile();
                initTable();
            }
            loadData();
        }

        void initTable()
        {
            string pollTableSql = "create table poll (id Number,name varchar)";
            string tagTableSql = "create table tag (id Number,tag_id Number,name varchar)";
            dBManager.Open();
            dBManager.Execute(pollTableSql);
            dBManager.Execute(tagTableSql);
            dBManager.Close();
        }

        void loadData() { 

        }

        private void ToSettings(object sender, RoutedEventArgs e)
        {
            TagSetting tagSetting = new TagSetting();
            tagSetting.Show();
        }
    }
}
