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
    /// Логика взаимодействия для FieldCreationWindow.xaml
    /// </summary>
    public partial class FieldCreationWindow : Window
    {
        MainRepo main;
        UserInterface userInterface;

        DataUnit record;

        public FieldCreationWindow()
        {
            InitializeComponent();

            main = new MainRepo();
            userInterface = new UserInterface(main);

            record = CollectionWindow.selectedRecord;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            record.AddProperty(new StringDataUnitProp(NameTextBox.Text, ValueTextBox.Text));

            userInterface.UpdateDataUnit(CollectionWindow.collectionID, record);

            this.Close();
        }
    }
}
