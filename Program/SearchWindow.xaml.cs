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
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        MainRepo main;
        UserInterface userInterface;

        public SearchWindow()
        {
            InitializeComponent();

            main = new MainRepo();
            userInterface = new UserInterface(main);
        }


        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            List<DataUnitProp> dataUnitProps = new List<DataUnitProp>();
            dataUnitProps.Add(new StringDataUnitProp(NameTextBox.Text, ValueTextBox.Text));
            var data = userInterface.SearchDataUnitsAllCollections(dataUnitProps);

            foreach (var dataUnitPage in data.DataPages)
            {
                foreach (var record in dataUnitPage.PageData)
                {
                    foreach (var field in record.Props)
                    {
                        StackPanel panel = new StackPanel { Height = 35, Width = 540, Orientation = Orientation.Horizontal };
                        panel.Children.Add(new TextBlock { Text = field.Name, Width = 163 });
                        panel.Children.Add(new TextBlock { Text = field.Value.ToString(), Width = 163 });
                        ResultListBox.Items.Add(panel);
                    }
                }
            }
        }
    }
}
