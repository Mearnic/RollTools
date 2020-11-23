using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RollTools
{
    public delegate void ChangeTextHandler2(string text);
    /// <summary>
    /// TagSetting.xaml 的交互逻辑
    /// </summary>
    public partial class TagSetting : Window
    {
        public event ChangeTextHandler2 ChangeTextEvent;
        TagService tagService;
        BindingList<Tag> tagList;
        PollService pollService;
        long poll_id;


        public TagSetting(long id)
        {
            
            InitializeComponent();
            Label title = FindName("poll_title") as Label;
            title.Content = id;
            poll_id = id;
            tagService = new TagService();
            pollService = new PollService();
            Poll poll = pollService.getPoll(poll_id);
            this.poll_title.Content = poll.Name;
            tagList = new BindingList<Tag>(tagService.queryAllList(id));
            this.tagListView.ItemsSource = tagList;

        }

        private void saveChange(object sender, RoutedEventArgs e)
        {
            tagService.deleteAll(poll_id);
            foreach (var item in tagList)
            {
                tagService.insert(item);
            }
            Refresh();
            MessageBox.Show(Application.Current.MainWindow, "保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            ChangeTextEvent("");
        }

        private void selectAll(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            bool isChecked = (bool)checkBox.IsChecked;
            foreach (var item in tagList)
            {
                item.IsChecked = isChecked;
            }
            Refresh();
        }

        private void Refresh()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(tagList);
            view.Refresh();
        }

        private void addTag(object sender, RoutedEventArgs e)
        {
            Tag tag = new Tag();
            tag.Id = tagService.GenerateId();
            tag.Name = "";
            tag.Poll_id = poll_id;
            tag.Is_use = "1";
            tagList.Insert(0, tag);
            Refresh();
        }

        private void openSelected(object sender, RoutedEventArgs e)
        {
            foreach (Tag item in tagList)
            {
                if (item.IsChecked)
                {
                    item.Is_use = "1";
                }
            }
            Refresh();

        }

        private void closeSelected(object sender, RoutedEventArgs e)
        {
            foreach (Tag item in tagList)
            {
                if (item.IsChecked)
                    item.Is_use = "0";
            }
            Refresh();
        }

        private void deleteSelected(object sender, RoutedEventArgs e)
        {

            for (int i = tagList.Count; i > 0; i--)
            {
                if (tagList[i - 1].IsChecked)
                {
                    tagList.Remove(tagList[i - 1]);
                }
            }
            Refresh();
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
