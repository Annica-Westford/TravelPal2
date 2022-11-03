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
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal;

/// <summary>
/// Interaction logic for RegisterWindow.xaml
/// </summary>
public partial class RegisterWindow : Window
{
    private UserManager userManager;
    private TravelManager travelManager;

    private bool isOKFields;

    public RegisterWindow(UserManager userManager, TravelManager travelManager)
    {
        InitializeComponent();

        this.userManager = userManager;
        this.travelManager = travelManager;

        //load combobox with countries
        string[] countries = Enum.GetNames(typeof(Countries));
        cbCountry.ItemsSource = countries; 

        HideErrorMessages();

        tbxUserName.Focus();
    }

    //hides the error messages - they will pop up if user writes something wrong
    private void HideErrorMessages()
    {
        lblWarningUsernameLength.Visibility = Visibility.Hidden;
        lblWarningPasswordLength.Visibility = Visibility.Hidden;
        lblWarningPasswordNoMatch.Visibility = Visibility.Hidden;
        lblWarningCountry.Visibility = Visibility.Hidden;
    }

    //when user clicks on Register button validate field inputs and create a new user
    private void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        HideErrorMessages();

        string username = tbxUserName.Text;
        string password = tbxPassword.Text;
        string confirmPassword = tbxConfirmPassword.Text;
        string country = cbCountry.SelectedItem as string;

        //set initial value of isOKFields to true
        isOKFields = true;

        //These methods change isOKFields to false if user has typed on something wrong
        ValidateUsernameLength(username);
        ValidatePassword(password, confirmPassword);
        ValidateLocation(country);

        if (isOKFields)
        {
            Countries countryEnum = (Countries)Enum.Parse(typeof(Countries), country);

            User newUser = new(username, password, countryEnum);

            //try to add user
            bool isSuccessfullyAdded = userManager.AddUser(newUser);

            if (isSuccessfullyAdded)
            {
                MessageBox.Show("Thanks for signing up! 😀", "Thanks!");

                //go back to main window & close this window
                new MainWindow(userManager, travelManager).Show();
                Close();
            }
            else
            {
                MessageBox.Show("User with the same user name already exists", "Warning");
            }
        }                            
    }


    //check if username is less than 3 characters
    private void ValidateUsernameLength(string username)
    {
        if (username.Length < 3)
        {
            lblWarningUsernameLength.Visibility = Visibility.Visible;
            lblWarningUsernameLength.Content = "UserName must be at least 3 characters long";
            isOKFields = false;
        }
    }

    //check if password inputs match and if password length is less than 5
    private void ValidatePassword(string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            lblWarningPasswordNoMatch.Visibility = Visibility.Visible;
            lblWarningPasswordNoMatch.Content = "The password inputs doesn't match";
            isOKFields = false;
        }

        if (password.Length < 5)
        {
            lblWarningPasswordLength.Visibility = Visibility.Visible;
            lblWarningPasswordLength.Content = "Password must be at least 5 characters long";
            isOKFields = false;
        }
    }

    //check if user has selected a country/location in combobox
    private void ValidateLocation(string? country)
    {
        if (string.IsNullOrEmpty(country))
        {
            lblWarningCountry.Visibility = Visibility.Visible;
            lblWarningCountry.Content = "You need to select a country";
            isOKFields = false;
        }
    }
}
