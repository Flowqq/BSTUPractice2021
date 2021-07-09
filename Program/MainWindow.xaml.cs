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
            CollectionsList.Items.Add(new TextBlock { Text = "Collection" });
        }

        private void CollectionsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        //пока не работает почему-то
        private void CreateCollection_Click(object sender, MouseButtonEventArgs e)
        {
            CollectionsList.Items.Add(new TextBlock { Text = "Collection", FontSize=17 });
        }
    }
}