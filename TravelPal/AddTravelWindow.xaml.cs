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
using TravelPal.Enums;
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for AddTravelWindow.xaml
    /// </summary>
    /// 
    //
    public partial class AddTravelWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        private Travel addedTravel;
        public AddTravelWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();

            this.userManager = userManager;
            this.travelManager = travelManager;

            ResetUI();

            LoadComboboxesWithData();

            //disable the possibility to choose a date before todays date
            dpStartDate.DisplayDateStart = DateTime.Now;
            dpEndDate.DisplayDateStart = DateTime.Now;
            
        }

        //hides and disables the trip type and all inclusive alternatives until we know what kind of travel the user chooses
        private void ResetUI()
        {
            lblTripType.Visibility = Visibility.Hidden;
            cbTripType.Visibility = Visibility.Hidden;
            cbTripType.IsEnabled = false;
            cbxAllInclusive.Visibility = Visibility.Hidden;
            cbxAllInclusive.IsEnabled = false;
            lblAllInclusive.Visibility = Visibility.Hidden;
            btnAddItemToPackingList.IsEnabled = false;
        }

        private void LoadComboboxesWithData()
        {
            //load combobox with countries
            string[] countries = Enum.GetNames(typeof(Countries));
            cbCountry.ItemsSource = countries;

            //load combobox for no of travellers with 1-10
            for (int i = 1; i <= 10; i++)
            {
                cbNoOfTravellers.Items.Add(i.ToString());
            }

            //load combobox with alternatives Trip or Vacation
            cbTripOrVacation.Items.Add("Trip");
            cbTripOrVacation.Items.Add("Vacation");

            //load combobox with trip types
            string[] tripTypes = Enum.GetNames(typeof(TripTypes));
            cbTripType.ItemsSource = tripTypes;
        }

        //when user has selected trip or vacation - show and enable trip type or all inclusive depending on the selected item
        private void cbTripOrVacation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetUI();

            //if user has chosen trip in combobox
            if (cbTripOrVacation.SelectedItem.ToString() == "Trip")
            {
                //show label for trip type
                lblTripType.Visibility = Visibility.Visible;

                //show and enable combobox for choosing trip type
                cbTripType.Visibility = Visibility.Visible;
                cbTripType.IsEnabled = true;

            }
            else if (cbTripOrVacation.SelectedItem.ToString() == "Vacation")
            {
                //show and enable checkbox for all inclusive
                cbxAllInclusive.Visibility = Visibility.Visible;
                cbxAllInclusive.IsEnabled = true;

                //show label for all inclusive
                lblAllInclusive.Visibility = Visibility.Visible;
            }
            
        }

        //when user has chosen start date, set selected date in datepicker end date to the startdate
        private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpEndDate.SelectedDate = dpStartDate.SelectedDate;
        }

        //when user presses the add button create a Trip or Vacation object and add to list of travels (in travelmanager and usermanager)
        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //load data from date pickers
                DateTime startDate = (DateTime)dpStartDate.SelectedDate;
                DateTime endDate = (DateTime)dpEndDate.SelectedDate;

                //load data from textboxes and comboboxes
                string destination = tbxDestination.Text;

                string country = cbCountry.SelectedItem as string;
                Countries countryEnum = (Countries)Enum.Parse(typeof(Countries), country);

                string numberOfTravellers = cbNoOfTravellers.SelectedItem as string;
                int numberOfTravellersInt = int.Parse(numberOfTravellers);

                string tripOrVacation = cbTripOrVacation.SelectedItem as string;

                if (tripOrVacation == "Trip")
                {
                    string tripType = cbTripType.SelectedItem as string;
                    TripTypes tripTypeEnum = (TripTypes)Enum.Parse(typeof(TripTypes), tripType);

                    //create a Trip object
                    Trip newTrip = new(destination, countryEnum, numberOfTravellersInt, startDate, endDate, tripTypeEnum);

                    travelManager.AddTravel(newTrip);
                    userManager.AddTravelToUser(newTrip);

                    //set field to the newly created trip
                    addedTravel = newTrip;

                }
                else if (tripOrVacation == "Vacation")
                {
                    bool isAllInclusive = false;

                    if ((bool)cbxAllInclusive.IsChecked)
                    {
                        isAllInclusive = true;
                    }

                    //create a Vacation object
                    Vacation newVacation = new(destination, countryEnum, numberOfTravellersInt, startDate, endDate, isAllInclusive);

                    travelManager.AddTravel(newVacation);
                    userManager.AddTravelToUser(newVacation);

                    //set field to the newly created vacation
                    addedTravel = newVacation;
                }

                //enable user to add item to packing list
                btnAddItemToPackingList.IsEnabled = true;

                //show packinglist
                UpdatePackingItemsListView();

                //disable textinputs and comboboxes
                DisableTextInputsAndComboboxes();

                txbTravelAdded.Visibility = Visibility.Visible;
                txbTravelAdded.Text = "New Travel Added!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please fill in all required fields!", "Warning");
            }

        }

        //disable text inputs and comboboxes so that user can't add more travels (so that user can focus on packing list)
        private void DisableTextInputsAndComboboxes()
        {
            dpStartDate.IsEnabled = false;
            dpEndDate.IsEnabled = false;
            tbxDestination.IsEnabled = false;
            cbCountry.IsEnabled = false;
            cbNoOfTravellers.IsEnabled = false;
            cbTripOrVacation.IsEnabled = false;
            btnAddTravel.IsEnabled = false;
        }

        //update listview with packing list items
        private void UpdatePackingItemsListView()
        {
            lvPackingList.Items.Clear();

            foreach (IPackingListItem packingListItem in addedTravel.PackingList)
            {
                ListViewItem lvItem = new();

                lvItem.Content = packingListItem.GetInfo();
                lvItem.Tag = packingListItem;

                lvPackingList.Items.Add(lvItem);
            }

        }

        //when user clicks on Add Item - send user to add packing item window
        private void btnAddItemToPackingList_Click(object sender, RoutedEventArgs e)
        {
            new AddPackingItemWindow(addedTravel).ShowDialog();

            UpdatePackingItemsListView();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}
