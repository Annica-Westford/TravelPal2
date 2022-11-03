using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;
using TravelPal.Models;

namespace TravelPal.Managers;

public class UserManager
{
    public List<IUser> Users { get; set; } = new();
    public IUser SignedInUser { get; set; }

    public UserManager()
    {
        AddAdmin();
    }

    //Prepopulate list of users with gandalf and add two travels to him
    public void PopulateUsersList(TravelManager travelManager)
    {
        //add new user
        User gandalf = new("Gandalf", "password", Enums.Countries.Bahamas);

        //set SignedInUser to Gandalf so that the methods runs properly
        SignedInUser = gandalf;

        //add gandalf to list of users
        AddUser(gandalf);

        //add two travels to user
        Trip trip = new("Alpaca hotel", Enums.Countries.Angola, 3, new DateTime(2023, 01, 15), new DateTime(2023, 01, 27), gandalf, Enums.TripTypes.Leisure);
        trip.PackingList.Add(new OtherItem("Pants", 5));
        trip.PackingList.Add(new TravelDocument("Insurance paper", true));

        Vacation vacation = new Vacation("Palm Tree Resort", Enums.Countries.Spain, 4, new DateTime(2023, 04, 01), new DateTime(2023, 04, 08), gandalf, true);
        vacation.PackingList.Add(new OtherItem("Rubber duck", 1));
        vacation.PackingList.Add(new TravelDocument("Bucket List", false));

        travelManager.AddTravel(trip);
        travelManager.AddTravel(vacation);

    }

    //add default admin
    private void AddAdmin()
    {
        Admin admin = new("admin", "password", Enums.Countries.Antarctica);
        Users.Add(admin);
    }

    //checks if user with the same username already exists - if yes return false, else return true and add user to list
    public bool AddUser(IUser userToAdd)
    {
        if (ValidateUsername(userToAdd.UserName))
        {
            Users.Add(userToAdd);
            return true;
        }
        else
        {
            return false;
        }  
    }

    //check if user with username already exists - returns true if username is ok, ie no one else has this name
    private bool ValidateUsername(string username)
    {
        foreach (IUser user in Users)
        {
            if (user.UserName == username)
            {
                return false;
            }
        }
        return true;
    }

    //check if user with username & password exists - if ok, return true and set SignedInUser to the user, else return false
    public bool SignInUser(string username, string password)
    {
        foreach (IUser user in Users)
        {
            if (user.UserName == username && user.Password == password)
            {
                //sign in user
                SignedInUser = user;
                return true;
            }
        }
        return false;
    }

    public bool UpdateUsername (IUser userToUpdate, string username)
    {        
        if (ValidateUsername(username))
        {
            userToUpdate.UserName = username;
            return true;
        }
        return false;
    }

    

    public void UpdateUserLocation(IUser userToUpdate, Countries country, TravelManager travelManager)
    {
        //check if user location is inside Europe before change 
        bool isInsideEuropeBeforeChange = travelManager.CheckIfCountryIsInEurope(userToUpdate.Location.ToString());

        userToUpdate.Location = country;

        //check if user location is inside Europe after change
        bool isInsideEuropeAfterChange = travelManager.CheckIfCountryIsInEurope(userToUpdate.Location.ToString());

        if ((isInsideEuropeBeforeChange != isInsideEuropeAfterChange) && userToUpdate is User)
        {
            //the travel documents in packing list must change since user changed country from inside of EU to outside, or vice versa
            //if userToUpdate is admin no changes needs to be made since admin has no travels
            User user = userToUpdate as User;
            travelManager.UpdateTravelDocuments(user);
        }

    }

    public void UpdateUserPassword(IUser userToUpdate, string newPassword)
    {
        userToUpdate.Password = newPassword;
    }


    

    

    

}
