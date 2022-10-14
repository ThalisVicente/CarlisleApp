using Service.Library;
using Service.Library.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CarlisleApp.Pages
{
    /// <summary>
    /// Interaction logic for ViewItems.xaml
    /// </summary>
    public partial class ViewItems : Page
    {
        private DataBaseService Service { get; set; }
        private MainWindow MainWindow { get; set; }
        private List<ItemModel> Items { get; set; }
        public ViewItems(MainWindow mainWindow)
        {
            Service = new DataBaseService();
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
