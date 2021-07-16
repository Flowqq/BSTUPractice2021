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
using Program.userInterface;

namespace Program
{
    /// <summary>
    /// Логика взаимодействия для CollectionWindow.xaml
    /// </summary>
    /// 
    public partial class CollectionWindow : Window
    {
        MainRepo main;
        UserInterface userInterface;

        //string collectionID;
        internal static string collectionID { get; set; }

        List<DataUnit> dataUnits;

        internal static DataUnit selectedRecord { get; set; }
        internal static string fieldName { get; set; }
        internal static string fieldValue { get; set; }

        public CollectionWindow()
        {
            InitializeComponent();

            main = new MainRepo();
            userInterface = new UserInterface(main);

            dataUnits = new List<DataUnit>();

            collectionID = MainWindow.selectedCollectionId;

            //RefreshDataUnits();
            RefreshCollectionInterface();
        }


        private void CreateRecord(object sender, RoutedEventArgs e)
        {
            dataUnits.Add(userInterface.AddDataUnit(collectionID));
            RefreshCollectionInterface();
        }

        private void DeleteRecord(object sender, RoutedEventArgs e)
        {
            if (fieldsList.SelectedItem.GetType().Name == "TextBlock")
            {
                userInterface.DeleteDataUnit(collectionID, FindRecordInList().Id);
                //dataUnits.RemoveAt(fieldsList.SelectedIndex);
                RefreshCollectionInterface();
            }
            
        }

        private DataUnit FindRecordInList(int minus = 0)
        {
            int index = 0;
            foreach(var c in fieldsList.Items)
            {
                if (c == fieldsList.SelectedItem)
                {
                    break;
                }
                if (c.GetType().Name == "TextBlock")
                {
                    index++;
                }
            }
            return dataUnits[index - minus];
        }

        private void AddField(object sender, RoutedEventArgs e)
        {
            selectedRecord = FindRecordInList();

            FieldCreationWindow fieldCreationWindow = new FieldCreationWindow();
            fieldCreationWindow.Show();
        }

        private void RefreshCollectionInterface()
        {
            fieldsList.Items.Clear();
            dataUnits.Clear();

            var collectionData = userInterface.GetCollectionData(collectionID);

            foreach (var dataUnitPage in collectionData.DataPages)
            {
                foreach (var record in dataUnitPage.PageData)
                {
                    dataUnits.Add(record);
                    fieldsList.Items.Add(new TextBlock { Text = "Record", Margin = new Thickness(40, 0, 0, 0) });
                    foreach (var field in record.Props)
                    {
                        StackPanel panel = new StackPanel { Height = 35, Width = 540, Orientation = Orientation.Horizontal };
                        panel.Children.Add(new TextBlock { Text = field.Name, Width = 163 });
                        panel.Children.Add(new TextBlock { Text = field.Value.ToString(), Width = 163 });
                        fieldsList.Items.Add(panel);
                    }
                }
            }
        }


        private void WindowActivated(object sender, EventArgs e)
        {
            RefreshCollectionInterface();
        }

        private void DeleteField(object sender, RoutedEventArgs e)
        {
            var record = FindRecordInList(1);
            var sp = fieldsList.SelectedItem as StackPanel;
            var tb = sp.Children[0] as TextBlock;

            record.RemoveProperty(tb.Text);
            userInterface.UpdateDataUnit(collectionID, record);
            RefreshCollectionInterface();
        }

        private void EditField(object sender, RoutedEventArgs e)
        {
            var sp = fieldsList.SelectedItem as StackPanel;
            var tbName = sp.Children[0] as TextBlock;
            var tbValue = sp.Children[1] as TextBlock;

            selectedRecord = FindRecordInList(1);
            fieldName = tbName.Text;
            fieldValue = tbValue.Text;


            FieldUpdateWindow fieldUpdateWindow = new FieldUpdateWindow();
            fieldUpdateWindow.Show();



        }
    }
}
    