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
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for AddPackingItemWindow.xaml
    /// </summary>
    public partial class AddPackingItemWindow : Window
    {
        private Travel travel;
        
        public AddPackingItemWindow(Travel travel)
        {
            InitializeComponent();

            this.travel = travel;

            tbxItemName.Focus();

            ResetUI();

            UpdatePackingItemsListView();
        }

        //hide and disable the fields for "required" until user checks the document-box
        private void HideRequiredFields()
        {
            lblRequired.Visibility = Visibility.Hidden;
            cbxRequired.Visibility = Visibility.Hidden;
            cbxRequired.IsEnabled = false;
            lblQuantity.Visibility = Visibility.Visible;
            tbxQuantity.Visibility = Visibility.Visible;
            tbxQuantity.IsEnabled = true;
        }

        private void ResetUI()
        {
            HideRequiredFields();

            tbxItemName.Clear();
            cbxDocument.IsChecked = false;
            cbxRequired.IsChecked = false;
            tbxQuantity.Clear();
        }

        private void UpdatePackingItemsListView()
        {
            lvPackingList.Items.Clear();

            foreach (IPackingListItem packingListItem in travel.PackingList)
            {
                ListViewItem lvItem = new();

                lvItem.Content = packingListItem.GetInfo();
                lvItem.Tag = packingListItem;

                lvPackingList.Items.Add(lvItem);
            }
        }

        private void cbxDocument_Checked(object sender, RoutedEventArgs e)
        {
            lblQuantity.Visibility = Visibility.Hidden;
            tbxQuantity.Visibility = Visibility.Hidden;
            tbxQuantity.IsEnabled = false;

            lblRequired.Visibility = Visibility.Visible;
            cbxRequired.Visibility = Visibility.Visible;
            cbxRequired.IsEnabled = true;
        }

        private void cbxDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            HideRequiredFields();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string itemName = tbxItemName.Text;
                if (string.IsNullOrEmpty(itemName))
                {
                    throw new ArgumentException("You need to input an item name!");
                }

                if ((bool)cbxDocument.IsChecked)
                {
                    //the item is a TravelDocument
                    bool isRequired = (bool)cbxRequired.IsChecked;
                    travel.PackingList.Add(new TravelDocument(itemName, isRequired));
                }
                else if (!(bool)cbxDocument.IsChecked)
                {
                    //the item is an OtherItem
                    int quantity = int.Parse(tbxQuantity.Text);
                    travel.PackingList.Add(new OtherItem(itemName, quantity));
                }

                ResetUI();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("You need to input a whole number in the 'Quantity' field");
            }

            UpdatePackingItemsListView();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
