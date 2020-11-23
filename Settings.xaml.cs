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
    public delegate void ChangeTextHandler(string text);
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Window
    {


        public event ChangeTextHandler ChangeTextEvent;
        TemplateService templateService;
        PollService pollService;
        BindingList<Template> bindingList;
        BindingList<Poll> settingListViewBindingList;
        public Settings()
        {
            InitializeComponent();
            templateService = new TemplateService();
            pollService = new PollService();
            List<Template> templateList = templateService.queryList();
            bindingList = new BindingList<Template>(templateList);
            this.cmbTemplates.ItemsSource = bindingList;
            foreach (Template item in bindingList)
            {
                if (item.IsSelected)
                {
                    this.cmbTemplates.SelectedItem = item;
                    item.IsLasted = true;
                }
            }
        }

        private void useTemplate(object sender, RoutedEventArgs e)
        {
            Template template = (Template)this.cmbTemplates.SelectedItem;
            if (template == null)
            {
                MessageBox.Show(Application.Current.MainWindow, "请先保存修改", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (MessageBox.Show("确定启用选择的池模板？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {

                foreach (Template item in bindingList)
                {
                    item.Is_used = "0";
                }
                templateService.updateUsed(template);
                template.Is_used = "1";
                ChangeTextEvent("");
            }
            else
            {
                return;
            }

        }

        private void updateTemplate(object sender, RoutedEventArgs e)
        {
            Template template = (Template)this.cmbTemplates.SelectedItem;
            if (template == null)
            {
                foreach (Template item in bindingList)
                {
                    if (item.IsLasted)
                    {
                        item.Name = this.cmbTemplates.Text;
                        templateService.update(item);
                        Refresh();
                    }

                }
                MessageBox.Show(Application.Current.MainWindow, "池模板名称修改完成", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                foreach (Poll item in settingListViewBindingList)
                {
                    pollService.update(item);
                    if (template.Is_used == "1")
                    {
                        ChangeTextEvent("");
                    }
                }
                MessageBox.Show(Application.Current.MainWindow, "池信息修改完成", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void deleteTemplate(object sender, RoutedEventArgs e)
        {


            Template template = (Template)this.cmbTemplates.SelectedItem;

            if (template == null)
            {
                MessageBox.Show(Application.Current.MainWindow, "请先保存修改", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("确定删除选择的池模板？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                if (bindingList.Count > 1 && template.Is_used != "1")
                {
                    for (int i = 0; i < bindingList.Count; i++)
                    {
                        if (bindingList[i].Id == template.Id)
                        {
                            templateService.delete(template.Id);
                            bindingList.Remove(bindingList[i]);
                            Refresh();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "不能删除使用中的池模板", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                return;
            }
        }

        private void addTemplate(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("确定添加新的池模板？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Template template = new Template();
                template.Id = templateService.GenerateId();
                template.Name = "new";
                template.Is_used = "1";
                foreach (Template item in bindingList)
                {
                    item.Is_used = "0";
                }
                templateService.insert(template);
                bindingList.Insert(0, template);
                Refresh();
                ChangeTextEvent("");
            }

        }

        private void Refresh()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(bindingList);
            foreach (Template item in bindingList)
            {
                if (item.IsSelected)
                {
                    this.cmbTemplates.SelectedItem = item;
                }
            }
            view.Refresh();
        }

        private void changeSelected(object sender, SelectionChangedEventArgs e)
        {
            Template template = (Template)this.cmbTemplates.SelectedItem;
            if (template != null)
            {

                List<Poll> pollList = pollService.queryList(template.Id);
                settingListViewBindingList = new BindingList<Poll>(pollList);
                this.settingListView.ItemsSource = settingListViewBindingList;
                foreach (Template item in bindingList)
                {
                    item.IsLasted = false;
                }
                template.IsLasted = true;
            }
        }
    }
}
