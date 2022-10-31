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
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    //TODO
    //Should I add user as a property of Travel so I know which user has added which travel?
    //TODO - Should I create properties or methods somewhere what checks username length and password length?? - Register Window
    public partial class MainWindow : Window
    {
        private TravelManager travelManager = new();
        private UserManager userManager = new();
        public MainWindow()
        {
            InitializeComponent();

            userManager.PopulateUsersList(travelManager);

            tbxUserName.Focus();
        }

        //create two more constructors so I can return to main window when closing down other windows
        public MainWindow(UserManager userManager) : this()
        {
            this.userManager = userManager;
        }

        public MainWindow(UserManager userManager, TravelManager travelManager) : this()
        {
            this.userManager = userManager;
            this.travelManager = travelManager;
        }

        //Opens up register window 
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new(userManager);
            registerWindow.Show();

            //close Main Window
            Close();
        }

        //check if username and password exist - if yes, open travels window, else show warning message
        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            string username = tbxUserName.Text;
            string password = pswPassword.Password;

            bool isSuccessfullySignedIn = userManager.SignInUser(username, password);

            if (isSuccessfullySignedIn)
            {
                //open TravelsWindow
                TravelsWindow travelsWindow = new(userManager, travelManager); 
                travelsWindow.Show();

                //Close Main Window
                Close();
            }
            else
            {
                MessageBox.Show("Username or password is incorrect", "Warning");
            }
            
        }
    }
}
