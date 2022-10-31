using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    /// 
    //See details about a travel (låsta inputs)
    public partial class TravelDetailsWindow : Window
    {
        private Travel selectedTravel; 
        public TravelDetailsWindow(Travel selectedTravel)
        {
            InitializeComponent();

            this.selectedTravel = selectedTravel;

            HideTripTypeAndAllInclusive();

            DisplayInitialInfoInFields();

            MakeFieldsReadOnly();

            UpdatePackingItemsListView();

        }

        //hide the labels and inputs för trip type and all inclusive until we know if travel is Vacation or Trip
        private void HideTripTypeAndAllInclusive()
        {
            //hide
            lblTripType.Visibility = Visibility.Hidden;
            cbTripType.Visibility = Visibility.Hidden;
            cbxAllInclusive.Visibility = Visibility.Hidden;
            lblAllInclusive.Visibility = Visibility.Hidden;
            
        }


        //display travel info in the different input fields
        private void DisplayInitialInfoInFields()
        {
            //Display chosen dates
            lblStartDate.Content = selectedTravel.StartDate.ToString("d");
            lblEndDate.Content = selectedTravel.EndDate.ToString("d");

            tbxDestination.Text = selectedTravel.Destination;

            cbCountry.Text = selectedTravel.Country.ToString();

            cbNoOfTravellers.Text = selectedTravel.Travellers.ToString();

            cbTripOrVacation.Text = selectedTravel.GetType().Name;

            lblTripLength.Content = $"{selectedTravel.TravelDays} days";

            if (selectedTravel is Trip)
            {
                DisplayTripTypeInfo();
            }
            else if (selectedTravel is Vacation)
            {
                DisplayAllInclusiveInfo();
            }
        }


        //if travel is Trip, make trip type fields visible and display the trip type info
        private void DisplayTripTypeInfo()
        {
            Trip trip = selectedTravel as Trip;

            lblTripType.Visibility = Visibility.Visible;
            cbTripType.Visibility = Visibility.Visible;

            cbTripType.Text = trip.TripType.ToString();
        }


        //if travel is Vacation, make all inclusive fields visible and display the checkbox info
        private void DisplayAllInclusiveInfo()
        {
            Vacation vacation = selectedTravel as Vacation;

            cbxAllInclusive.Visibility = Visibility.Visible;
            lblAllInclusive.Visibility = Visibility.Visible;

            if (vacation.IsAllInclusive)
            {
                cbxAllInclusive.IsChecked = true;
            }
            else
            {
                cbxAllInclusive.IsChecked = false;
            }
        }

        //make all the fields "readonly" (prepare for implementation to Edit travel)
        private void MakeFieldsReadOnly()
        {
            cbTripType.IsEditable = true;
            cbTripType.IsReadOnly = true;
            cbTripType.IsHitTestVisible = false;
            cbxAllInclusive.IsHitTestVisible = false;

            tbxDestination.IsReadOnly = true;

            cbCountry.IsEditable = true;
            cbCountry.IsReadOnly = true;
            cbCountry.IsHitTestVisible = false;

            cbNoOfTravellers.IsEditable = true;
            cbNoOfTravellers.IsReadOnly = true;
            cbNoOfTravellers.IsHitTestVisible = false;

            cbTripOrVacation.IsEditable = true;
            cbTripOrVacation.IsReadOnly = true;
            cbTripOrVacation.IsHitTestVisible = false;
        }

        //Update List view with packing list items
        private void UpdatePackingItemsListView()
        {
            lvPackingList.Items.Clear();

            foreach (IPackingListItem packingListItem in selectedTravel.PackingList)
            {
                ListViewItem lvItem = new();

                lvItem.Content = packingListItem.GetInfo();
                lvItem.Tag = packingListItem;

                lvPackingList.Items.Add(lvItem);
            }

        }

        //Click on Add Item button - send user to Add Item Window
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            AddPackingItemWindow addPackingItemWindow = new(selectedTravel);
            addPackingItemWindow.ShowDialog();

            UpdatePackingItemsListView();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
