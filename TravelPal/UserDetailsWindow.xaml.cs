﻿using System;
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
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        private UserManager userManager;

        private bool hasSuccessfullyUpdatedUser;

        private bool hasNoNewInputs;

        public UserDetailsWindow(UserManager userManager)
        {
            InitializeComponent();

            this.userManager = userManager;

            //load combobox with countries
            string[] countries = Enum.GetNames(typeof(Countries));
            cbCountry.ItemsSource = countries;

            PreLoadFieldsWithUserData();

            HideErrorMessages();

            tbxUserName.Focus();
        }

        //load fields with the signed in user's data
        private void PreLoadFieldsWithUserData()
        {
            tbxUserName.Text = userManager.SignedInUser.UserName;
            cbCountry.Text = userManager.SignedInUser.Location.ToString();
            cbCountry.IsEditable = true;
            cbCountry.IsReadOnly = true;
        }
        
        //hide error messages - they will pop up if user types in something wrong
        private void HideErrorMessages()
        {
            lblWarningUsernameLength.Visibility = Visibility.Hidden;
            lblWarningPasswordLength.Visibility = Visibility.Hidden;
            txbWarningPasswordNoMatch.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            HideErrorMessages();

            string username = tbxUserName.Text;
            string newPassword = tbxNewPassword.Text;
            string confirmPassword = tbxConfirmPassword.Text;
            string country = cbCountry.SelectedItem as string;

            //give bool the initial value true
            hasSuccessfullyUpdatedUser = true;

            //these methods set bool to false if user typed in something wrong
            TryUpdateUsername(username);
            TryUpdateLocation(country);
            TryUpdatePassWord(newPassword, confirmPassword);

            //if user clicks save but hasn't made any changes, just close window
            if (hasNoNewInputs)
            {
                Close();
            }
            else if (hasSuccessfullyUpdatedUser)
            {
                MessageBox.Show("Your user details have been updated!");
                Close();
            }   

        }

        //if user has typed in a new name - update name property if valid
        private void TryUpdateUsername(string username)
        {
            //if username equals SignedInUser.UserName no change has been made - don't do anything
            if (username != userManager.SignedInUser.UserName)
            {
                hasNoNewInputs = false; //user has changed an input

                //if username length is OK
                if (ValidateUserNameLength(username))
                {
                    //check if username can be updated
                    hasSuccessfullyUpdatedUser = userManager.UpdateUsername(userManager.SignedInUser, username);

                    if (!hasSuccessfullyUpdatedUser)
                    {
                        lblWarningUsernameLength.Visibility = Visibility.Visible;
                        lblWarningUsernameLength.Content = "UserName already exists";
                    }
                }
            }
            else
            {
                hasNoNewInputs = true;
            }
        }


        //check if username length is more than 3 characters
        private bool ValidateUserNameLength(string username)
        {
            if (username.Length >= 3)
            {
                return true;
            }
            else
            {
                hasSuccessfullyUpdatedUser = false;
                lblWarningUsernameLength.Visibility = Visibility.Visible;
                lblWarningUsernameLength.Content = "UserName must be at least 3 characters long";
                return false;
            }
        }


        //if user has selected a new country - update location property
        private void TryUpdateLocation(string? country)
        {
            //if selected country equals SignedInUser.Location no changes has been made 
            if (country != userManager.SignedInUser.Location.ToString())
            {
                hasNoNewInputs = false;

                Countries countryEnum = (Countries)Enum.Parse(typeof(Countries), country);
                userManager.UpdateUserLocation(userManager.SignedInUser, countryEnum);
            }
        }


        //if user has entered a new password, update password property if valid
        private void TryUpdatePassWord(string newPassword, string confirmPassword)
        {
            //check if any of the password fields have something written in them, if both are null or empty, don't do anything
            if (!string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword))
            {
                hasNoNewInputs = false;

                //check that password is 5 or more characters
                if (ValidatePasswordLength(newPassword))
                {
                    //check if newPassword equals confirmPassword 
                    if (newPassword == confirmPassword)
                    {
                        userManager.UpdateUserPassword(userManager.SignedInUser, newPassword);
                    }
                    else
                    {
                        hasSuccessfullyUpdatedUser = false;
                        txbWarningPasswordNoMatch.Visibility = Visibility.Visible;
                        txbWarningPasswordNoMatch.Content = "The password inputs doesn't match";
                    }
                }

            }
        }


        //check if password inputs match and if password length is less than 5
        private bool ValidatePasswordLength(string newPassword)
        {
            if (newPassword.Length >= 5)
            {
                return true;
            }
            else
            {
                hasSuccessfullyUpdatedUser = false;
                lblWarningPasswordLength.Visibility = Visibility.Visible;
                lblWarningPasswordLength.Content = "Password must be at least 5 characters long";
                return false;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}