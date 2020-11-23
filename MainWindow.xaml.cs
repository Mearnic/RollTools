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
using System.Threading;
using System.Windows.Threading;

namespace RollTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DBManager dBManager = null;
        PollService pollService = null;
        TagService tagService;
        TemplateService templateService = null;
        Dictionary<long,int> indexs;
        public MainWindow()
        {
            InitializeComponent();
            indexs = new Dictionary<long, int>();
            string directory = Environment.CurrentDirectory + "/data";
            string dbPath = directory + "/SqliteModel.db";
            bool exists = File.Exists(dbPath);
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            dBManager = new DBManager(dbPath);
            if (!exists)
            {
                initTable();
            }
            templateService = new TemplateService();
            tagService = new TagService();
            loadWindow();
        }

        void initTable()
        {
            string templateTableSql = "create table template (id Number, name varchar, is_used varchar )";
            string pollTableSql = "create table poll (id Number, template_id Number, name varchar, is_visibility varchar)";
            string tagTableSql = "create table tag (id Number, poll_id Number, name varchar, is_use varchar)";
            dBManager.Open();
            dBManager.Execute(templateTableSql);
            dBManager.Execute(pollTableSql);
            dBManager.Execute(tagTableSql);
            dBManager.Commit();
            dBManager.Close();
        }

        private void ToSettings(object sender, RoutedEventArgs e)
        {
            Settings setting = new Settings();
            setting.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            setting.Owner = this;
            setting.ChangeTextEvent += new ChangeTextHandler(FuncSettingClosed);
            setting.ShowDialog();
        }

        private void RollTag(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Name.Replace("rollName", ""));
            TextBox textBox = FindName("tagName" + (index)) as TextBox;
            long poll_id = Convert.ToInt64(button.Tag);
            List<Tag> tagList = tagService.queryList(poll_id);

            textBox.Tag = poll_id;
            if (!indexs.ContainsKey(poll_id) && tagList.Count > 0)
            {
                indexs.Add(poll_id, 0);
                DispatcherTimer _timer = new DispatcherTimer();
                TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, 30);
                _timer.Tick += new EventHandler(updateRoll);
                _timer.Interval = timeSpan;
                _timer.Tag = textBox;
                _timer.Start();
            }
            else {
                if (tagList.Count <=0 )
                {
                    MessageBox.Show(Application.Current.MainWindow, "池中没有标签", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                MessageBox.Show(Application.Current.MainWindow, "Roll滚动中...", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void updateRoll(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            TextBox box = timer.Tag as TextBox;
            long poll_id = Convert.ToInt64(box.Tag);
            List<Tag> tagList = tagService.queryList(poll_id);
            if (tagList.Count > 0)
            {
                Tag tag = tagList.OrderBy(t => Guid.NewGuid()).First();
                box.Text = tag.Name;
                box.Foreground = new SolidColorBrush(Colors.DarkRed);
                if (indexs[poll_id]++ > 50)
                {
                    timer.Stop();
                    box.Foreground = new SolidColorBrush(Colors.Green);
                    indexs.Remove(poll_id);
                }
            }
        }

        private void SettingTag(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            TagSetting tagSetting = new TagSetting(Convert.ToInt64(button.Tag));
            tagSetting.Tag = button.Tag;
            tagSetting.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            tagSetting.Owner = this;
            tagSetting.ChangeTextEvent += new ChangeTextHandler2(FuncSettingClosed);
            tagSetting.ShowDialog(); 

        }

        void FuncSettingClosed(string text) {
            loadWindow();
        }

        private void newTemplate(object sender, RoutedEventArgs e)
        {
            Template template = new Template();
            template.Id = templateService.GenerateId();
            template.Name = "new";
            template.Is_used = "1";
            templateService.insert(template);
        }


        private void loadWindow() {

            Template template = templateService.getDefaultTemplate();
            if (template == null)
            {
                template = new Template();
                template.Id = templateService.GenerateId();
                template.Name = "default";
                template.Is_used = "1";
                templateService.insert(template);
            }

            pollService = new PollService();
            List<Poll> polls = pollService.queryList(template.Id);
            for (int i = 0; i < polls.Count; i++)
            {
                Label label = FindName("pollName" + (i + 1)) as Label;
                label.Content = polls[i].Name;
                Button roll_button = FindName("rollName" + (i + 1)) as Button;
                Button setting_button = FindName("settingName" + (i + 1)) as Button;
                TextBox textBox = FindName("tagName" + (i + 1)) as TextBox;
                textBox.FontSize = 20;
                textBox.Foreground = new SolidColorBrush(Colors.DarkRed);
                roll_button.Tag = polls[i].Id;
                setting_button.Tag = polls[i].Id;
                textBox.Text = "待Roll";
                if (!polls[i].IsVisibility)
                {
                    roll_button.IsEnabled = false;
                    roll_button.Content = "停止使用";
                }
                else {
                    roll_button.IsEnabled = true;
                    roll_button.Content = "待Roll";
                }
            }
        }

    }
}
