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

using Program.DataPage;
using Program.userInterface;

namespace Program
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainRepo main;
        UserInterface userInterface;

        List<CollectionDefinition> collectionDefinitions;

        internal static string selectedCollectionId { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            main = new MainRepo();
            userInterface = new UserInterface(main);

            RefreshCollectionList();
        }

        private void RefreshCollectionList()
        {
            collectionDefinitions = userInterface.GetCollectionDefinitions();

            CollectionsList.Items.Clear();


            foreach (var collDef in collectionDefinitions)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = collDef.Name,
                    FontSize = 17,
                    Height = 32,
                    Width = 560,
                    Padding = new Thickness(10, 0, 0, 0)
                };
                textBlock.MouseLeftButtonDown += new MouseButtonEventHandler(OpenCollection);

                CollectionsList.Items.Add(textBlock);
            }
        }


        private void CreateCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CollectionsList.Items.IsEmpty)
            {
                if (CollectionsList.Items[CollectionsList.Items.Count - 1].GetType().Name == "TextBlock")
                {
                    TextBox tb = new TextBox
                    {
                        FontSize = 17,
                        Height = 32,
                        Width = 242,
                        Padding = new Thickness(5, 0, 0, 0)
                    };
                    tb.LostFocus += new RoutedEventHandler(TextBoxClosingCreation);

                    CollectionsList.Items.Add(tb);
                }
            }
            else
            {
                TextBox tb = new TextBox
                {
                    FontSize = 17,
                    Height = 32,
                    Width = 242,
                    Padding = new Thickness(5, 0, 0, 0)
                };
                tb.LostFocus += new RoutedEventHandler(TextBoxClosingCreation);

                CollectionsList.Items.Add(tb);
            }
            
        }

        internal static string GetSelectedCollectionId()
        {
            return selectedCollectionId;
        }

        private void TextBoxClosingCreation(object sender, RoutedEventArgs e)
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


            collectionDefinitions.Add(userInterface.CreateCollection(collectionName));//
            CollectionsList.Items.Insert(index, textBlock);
        }

        private void TextBoxClosingRenaming(object sender, RoutedEventArgs e)
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

            userInterface.RenameCollection(collectionDefinitions[index].Id, collectionName);
            CollectionsList.Items.Insert(index, textBlock);
        }

        private void OpenCollection(object sender, RoutedEventArgs e)
        {
            if (CollectionsList.SelectedItem == sender)
            {
                selectedCollectionId = collectionDefinitions[CollectionsList.Items.IndexOf(CollectionsList.SelectedItem)].Id;

                CollectionWindow collectionWindow = new CollectionWindow();
                collectionWindow.Show();
            }

        }


        private void DeleteCollection(object sender, RoutedEventArgs e)
        {
            userInterface.DeleteCollection(collectionDefinitions[CollectionsList.Items.IndexOf(CollectionsList.SelectedItem)].Id);
            collectionDefinitions.RemoveAt(CollectionsList.SelectedIndex);
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
                tb.LostFocus += new RoutedEventHandler(TextBoxClosingRenaming);


                CollectionsList.Items.Insert(index, tb);
            }

        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.Show();
        }
    }
}