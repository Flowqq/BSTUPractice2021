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
    /// Логика взаимодействия для FieldUpdateWindow.xaml
    /// </summary>
    public partial class FieldUpdateWindow : Window
    {
        MainRepo main;
        UserInterface userInterface;

        DataUnit record;
        string fieldName;
        string fieldValue;

        public FieldUpdateWindow()
        {
            InitializeComponent();

            main = new MainRepo();
            userInterface = new UserInterface(main);

            record = CollectionWindow.selectedRecord;
            fieldName = CollectionWindow.fieldName;
            fieldValue = CollectionWindow.fieldValue;

            NameTextBox.Text = fieldName;
            ValueTextBox.Text = fieldValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            record.SetProperty(fieldName, new StringDataUnitProp(NameTextBox.Text, ValueTextBox.Text));

            userInterface.UpdateDataUnit(CollectionWindow.collectionID, record);

            this.Close();
        }
    }
}
