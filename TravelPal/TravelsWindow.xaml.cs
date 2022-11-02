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
    /// Interaction logic for TravelsWindow.xaml
    /// </summary>
    /// 
    
    public partial class TravelsWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        public TravelsWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();

            this.userManager = userManager;
            this.travelManager = travelManager;

            //set welcome message to signed in user's name
            lblWelcomeUser.Content = $"Welcome {userManager.SignedInUser.UserName}!";

            //load listview with travels
            UpdateTravelsListView();

            //admin does not have access to add travel-window, it's only for users
            if (userManager.SignedInUser is Admin)
            {
                btnAddTravel.IsEnabled = false;
                btnDetails.IsEnabled = false;
                btnAboutTravelPal.IsEnabled = false;
                btnUser.IsEnabled = false;
            }

        }

        //load listview with different travel data depending on if SignedInUser is Admin or User
        private void UpdateTravelsListView()
        {
            lvTravels.Items.Clear();

            //check if user is User or Admin 
            if (userManager.SignedInUser is Admin)
            {
                //show listview of ALL user's travels from list in TravelManager
                DisplayTravelList(travelManager.Travels);

            }
            else if (userManager.SignedInUser is User)
            {
                //cast to user to access list of travels
                User user = userManager.SignedInUser as User;

                //display listview of the user's travels
                DisplayTravelList(user.Travels);
            }
        }

        //load listview with travels
        private void DisplayTravelList(List<Travel> listToDisplay)
        {
            foreach (Travel travel in listToDisplay)
            {
                ListViewItem item = new();

                item.Content = travel.GetInfo();
                item.Tag = travel;

                lvTravels.Items.Add(item);
            }
        }

        //Click on User-button - send user to UserDetailsWindow
        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            new UserDetailsWindow(userManager, travelManager).ShowDialog();
        }

        //Click on Add Travel-button - send user to AddTravelWindow
        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            new AddTravelWindow(userManager, travelManager).ShowDialog();

            //when done with add travel, update listview to see added travel
            UpdateTravelsListView();
        }

        //Click on Details-button - send user to TravelDetailsWindow
        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            //if an item is selected in listview
            if (CheckIfItemIsSelectedInListView())
            {
                ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                Travel selectedTravel = selectedItem.Tag as Travel;

                //send user to TravelDetailsWindow
                new TravelDetailsWindow(selectedTravel).ShowDialog();
            }
        }

        //Click on Remove-button - removes a travel
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //if an item is selected in listview
            if (CheckIfItemIsSelectedInListView())
            {
                ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                Travel selectedTravel = selectedItem.Tag as Travel;

                travelManager.RemoveTravel(selectedTravel);

                UpdateTravelsListView();
            }
        }

        //check if user has selected an item in the list view 
        private bool CheckIfItemIsSelectedInListView()
        {
            if (lvTravels.SelectedItems.Count > 0)
            {
                return true;
            }
            else //if no item is selected
            {
                MessageBox.Show("You need to select a travel item in the list!", "Warning");
                return false;
            }
        }

        //Click on About-button - show small info-box with information about how to use the app and about TravelPal as a company
        private void btnAboutTravelPal_Click(object sender, RoutedEventArgs e)
        {
            string about = "TravelPal was founded during Middle Earth's Second Age by the Lady of Light herself, Galadriel. " +
                "She was travelling a lot at the time and couldn't find a travel app that suited her needs - so she created one! " +
                "The app has won the prize for 'best travel app outside of Middle Earth' three years in a row. " +
                "Even Sauron allegedly used the app when he needed to quickly book vacation to the Bahamas. " +
                "Apparently he said: 'Even the Dark Lord needs to rest and drink Piña Coladas sometimes' \n\n";

            string howToUse = "TravelPal is easy to use. Just click on Add Travel to add a new travel. " +
                "Click on User to see and change user details. Select a travel in the list and then either click on Details to see " +
                "the details of the travel or select Remove to remove the selected travel.";

            MessageBox.Show(about + howToUse, "About TravelPal", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Click on Sign out-button - close TravelsWindow and go back to MainWindow
        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(userManager, travelManager).Show();
            Close();
        }
    }
}
