using Service.Classes;
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

namespace CarlisleApp.Pages
{
    /// <summary>
    /// Interaction logic for ViewItems.xaml
    /// </summary>
    public partial class ViewItems : Page
    {
        private Service.Service Service { get; set; }
        private MainWindow MainWindow { get; set; }
        private List<Item> Items { get; set; }
        public ViewItems(MainWindow mainWindow)
        {
            Service = new Service.Service();
            InitializeComponent();
            MainWindow = mainWindow;
            LoadItems();
        }

        private void LoadItems()
        {
            Items = Service.GetAllItems();
            dgItems.ItemsSource = Items;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.frameViewAllItems.Visibility = Visibility.Collapsed;
        }
    }
}
