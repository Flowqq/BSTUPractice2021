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

namespace Program
{
    /// <summary>
    /// Логика взаимодействия для CollectionWindow.xaml
    /// </summary>
    public partial class CollectionWindow : Window
    {
        public CollectionWindow()
        {
            InitializeComponent();
        }


        private void CreateRecordClick(object sender, RoutedEventArgs e)
        {
            ListBox fieldsList = new ListBox();

            TabItem tabItem = new TabItem{ Content = fieldsList, Header = "Record 3" };
            records.Items.Add(tabItem);
        }

        private void DeleteRecordClick(object sender, RoutedEventArgs e)
        {
            records.Items.Remove(records.SelectedItem);
        }

        private void AddFieldClick(object sender, RoutedEventArgs e)
        {
            TabItem selectedTabItem = records.SelectedItem as TabItem;
            ListBox listBox = selectedTabItem.Content as ListBox;
            StackPanel stackPanel = new StackPanel { Height = 70, Width = 463, Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new TextBlock { Text = "color", Width = 163 });
            stackPanel.Children.Add(new TextBlock { Text = "blue", Width = 163 });

            listBox.Items.Add(stackPanel);

        }
    }
}
    