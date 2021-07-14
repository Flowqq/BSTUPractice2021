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

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = new TextBox
            {
                //Text = "Collection 1",
                FontSize = 17,
                Height = 32,
                Width = 242,
                Padding = new Thickness(5, 0, 0, 0)
            };
            tb.LostFocus += new RoutedEventHandler(TextBoxClosing);

            CollectionsList.Items.Add(tb);
        }

        

        private void TextBoxClosing(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int index = CollectionsList.Items.IndexOf(tb);
            string collectionName = tb.Text;
            CollectionsList.Items.Remove(tb);

            TextBlock textBlock = new TextBlock
            {
                Text = collectionName,
                FontSize = 17,
                Height = 32,
                Width = 560,
                Padding = new Thickness(10, 0, 0, 0)
            };
            textBlock.MouseLeftButtonDown += new MouseButtonEventHandler(OpenCollection);



            CollectionsList.Items.Insert(index, textBlock);
        }

        private void OpenCollection(object sender, RoutedEventArgs e)
        {
            if (CollectionsList.SelectedItem == sender)
            {
                CollectionWindow collectionWindow = new CollectionWindow();
                collectionWindow.Show();
            }
            
        }


        private void DeleteCollection(object sender, RoutedEventArgs e)
        {
            CollectionsList.Items.Remove(CollectionsList.SelectedItem);
        }

        private void RenameCollection(object sender, RoutedEventArgs e)
        {
            if (CollectionsList.SelectedItem.GetType().Name == "TextBlock") //скорее всего можно сделать умнее
            {
                int index = CollectionsList.Items.IndexOf(CollectionsList.SelectedItem);
                string collectionName = (CollectionsList.SelectedItem as TextBlock).Text;
                CollectionsList.Items.Remove(CollectionsList.SelectedItem);
                TextBox tb = new TextBox
                {
                    Text = collectionName,
                    FontSize = 17,
                    Height = 32,
                    Width = 242,
                    Padding = new Thickness(5, 0, 0, 0)
                };
                tb.LostFocus += new RoutedEventHandler(TextBoxClosing);

                CollectionsList.Items.Insert(index, tb);
            }


        }
    }
}