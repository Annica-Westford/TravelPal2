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

namespace TravelPal;

//Note till Albin:
//Jag gjorde ett GitHub repo första dagen på denna uppgiften. Committade hela tiden. Sen skulle jag såklart förstöra allt genom att försöka
//reverta en commit, gjorde fel och fick panik. Ser ju att man kan un-reverta en revert men det är jag för stressad för att försöka mig på
//Till slut laddade jag ner den tidigare version av koden som jag ville återgå till, gjorde ett nytt repo av den och gjorde en mega-commit 
//Så nu syns det inte längre att jag har gjort en massa commits under hela projektets gång...
//Och ja, nu har jag lärt mig att om jag ska försöka implementera något jag är lite osäker på ska jag använda BRANCH framöver :)

public partial class MainWindow : Window
{
    private TravelManager travelManager;
    private UserManager userManager = new();
    public MainWindow()
    {
        InitializeComponent();

        tbxUserName.Focus();

        travelManager = new(userManager);

        userManager.PopulateUsersList(travelManager); 

    }

    //create another constructor so I can return to main window with the same copy of usermanager and travelmanager when closing down other windows
    public MainWindow(UserManager userManager, TravelManager travelManager) 
    {
        this.userManager = userManager;
        this.travelManager = travelManager;

        InitializeComponent();

        tbxUserName.Focus();
    }

    //Opens up register window 
    private void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        new RegisterWindow(userManager, travelManager).Show();

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
