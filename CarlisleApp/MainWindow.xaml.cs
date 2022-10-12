using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Service.Enum;
using Service.Classes;
using Service;
using CarlisleApp.Pages;

namespace CarlisleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Service.Service Service { get; set; }
        private Item Item { get; set; }
        private List<string> SelectedAttributeTitles { get; set; }
        private List<string> AllAttributeTitles { get; set; }

        public MainWindow()
        {
            Service = new Service.Service();
            Item = new Item();
            SelectedAttributeTitles = new List<string>();
            AllAttributeTitles = new List<string>();
            InitializeComponent();
        }

        private void btCreateItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInputItem())
            {
                LoadItem();
                var dbRequestResult = Service.CreateItem(Item);
                MessageBox.Show(dbRequestResult.Message);
                if (dbRequestResult.Success)
                    ClearFields();
            }
            else
                MessageBox.Show("Please, Upload the attributes and enter a valid 4 digit Item Id");
        }

        private void ClearFields()
        {
            tbInputItem.Text = string.Empty;
            SelectedAttributeTitles.Clear();
            if (AllAttributeTitles.Any())
                LoadAllAttributeButtons();
        }

        private bool IsValidInputItem()
        {
            return int.TryParse(tbInputItem.Text, out var item) 
                && tbInputItem.Text.Length == (short)MaxDigitsInputItem.MaxDigitsInputItem
                && AllAttributeTitles.Any();
        }

        private void LoadItem()
        {
            Item.Id = int.Parse(tbInputItem.Text);

            foreach (var title in SelectedAttributeTitles)
            {
                Item.Attributes.Add(new Service.Classes.Attribute
                {
                    Title = title,
                    ItemId = Item.Id
                });
            }
        }

        private void ImportCsv_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            openFileDialog.Filter = "CSV Files|*.csv";
            openFileDialog.Multiselect = false;

            using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
            {
                AllAttributeTitles = File.ReadLines(openFileDialog.FileName).Take((short)MaxAttributes.MaxAttributes).ToList();
            }

            LoadAllAttributeButtons();
        }

        private void LoadAllAttributeButtons()
        {
            var index = 1;

            foreach (var attribute in AllAttributeTitles)
            {
                Button button = (Button)this.FindName($"btAttribute{index}");
                button.Content = attribute;
                button.Background = Brushes.LightGray;
                button.IsEnabled = true;
                index++;
            }
        }

        private void btAttribute_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (!SelectedAttributeTitles.Any(t => t == button.Content.ToString()))
            {
                SelectedAttributeTitles.Add(button.Content.ToString());
                button.Background = Brushes.Green;
            }
            else
            {
                SelectedAttributeTitles.Remove(button.Content.ToString());
                button.Background = Brushes.LightGray;
            }
        }

        private void ViewAllItems_Click(object sender, RoutedEventArgs e)
        {
            frameViewAllItems.Content = new ViewItems(this);
        }
    }
}
