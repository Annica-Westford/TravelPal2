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

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for AboutTravelPalWindow.xaml
    /// </summary>
    public partial class AboutTravelPalWindow : Window
    {
        public AboutTravelPalWindow()
        {
            InitializeComponent();

            txbAbout.Text = "TravelPal was founded during Middle Earth's Second Age by the Lady of Light herself, Galadriel. " + 
                "She was travelling a lot at the time and couldn't find a travel app that suited her needs - so she created one! " +
                "The app has won the prize for 'best travel app outside of Middle Earth' three years in a row. " +
                "Even Sauron allegedly used the app when he needed to quickly book vacation to the Bahamas. " +
                "Apparently he said: 'Even the Dark Lord needs to chillax and drink Piña Coladas sometimes'";

            txbHowToUse.Text = "TravelPal is easy to use. Just click on Add Travel to add a new travel. " +
                "Click on User to see and change user details. Select a travel in the list and then either click on Details to see " +
                "the details of the travel or select Remove to remove the selected travel.";

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
