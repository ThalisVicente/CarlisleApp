using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Service.Enum;
using Service.Library.Classes;
using Service.Library;
using CarlisleApp.Pages;

namespace CarlisleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBaseService DataBaseService { get; set; }
        private ItemModel Item { get; set; }
        private List<string> SelectedAttributeTitles { get; set; }
        private List<string> AllAttributeTitles { get; set; }

        public MainWindow()
        {
            InitializeProperties();
            InitializeComponent();
        }

        private void InitializeProperties()
        {
            DataBaseService = new DataBaseService();
            Item = new ItemModel();
            SelectedAttributeTitles = new List<string>();
            AllAttributeTitles = new List<string>();
        }

        private void OnClickCreateItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInputItem())
            {
                LoadItemModel();
                var dbRequestResult = DataBaseService.CreateItem(Item);

                if (dbRequestResult.Success)
                {
                    ShowSuccessMessageBox(dbRequestResult.Message);
                    ClearFields();
                }
            }
            else
                ShowErrorMessageBox("Please, enter a valid 4 digit Item Id");
        }

        private void ClearFields()
        {
            Item = new ItemModel();
            tbInputItem.Text = string.Empty;
            SelectedAttributeTitles.Clear();
            if (AllAttributeTitles.Any())
                LoadAllAttributeButtons();
        }

        private bool IsValidInputItem()
        {
            return int.TryParse(tbInputItem.Text, out var item)
                && tbInputItem.Text.Length == (short)MaxDigitsInputItem.MaxDigitsInputItem;
        }

        private void LoadItemModel()
        {
            Item.Id = int.Parse(tbInputItem.Text);

            foreach (var title in SelectedAttributeTitles)
            {
                Item.Attributes.Add(new AttributeModel
                {
                    Title = title,
                    ItemId = Item.Id
                });
            }
        }

        private void OnClickImportCsv_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|XML files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                AllAttributeTitles.Clear();

                using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
                {
                    var lines = File.ReadLines(openFileDialog.FileName).Take((short)MaxAttributes.MaxAttributes).ToList();
                    AllAttributeTitles = lines.Select(a => a.Split(',').First()).ToList();
                }

                ClearAllAttributeButtons();
                LoadAllAttributeButtons();
            }
        }

        private void LoadAllAttributeButtons()
        {
            var index = 1;

            foreach (var attribute in AllAttributeTitles)
            {
                Button button = (Button)this.FindName($"btAttribute{index}");
                button.Content = attribute.ToUpper();
                button.Background = Brushes.Gray;
                button.IsEnabled = true;
                index++;
            }
        }

        private void ClearAllAttributeButtons()
        {
            for (var i = 1; i <= (short)MaxAttributes.MaxAttributes; i++)
            {
                Button button = (Button)this.FindName($"btAttribute{i}");
                button.Content = "-";
                button.Background = Brushes.Gray;
                button.IsEnabled = false;
            }
        }

        private void OnClickAttributeButton_Click(object sender, RoutedEventArgs e)
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
                button.Background = Brushes.Gray;
            }
        }

        private void ViewAllItems_Click(object sender, RoutedEventArgs e)
        {
            frameViewAllItems.Content = new ViewItems(this);
            frameViewAllItems.Visibility = Visibility.Visible;
        }

        private void CloseApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Carlisle", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowSuccessMessageBox(string message)
        {
            MessageBox.Show(message, "Carlisle", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
