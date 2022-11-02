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

    private void AddAdmin()
    {
        Admin admin = new("admin", "password", Enums.Countries.Antarctica);
        Users.Add(admin);
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
        Trip trip = new("Alpaca hotel", Enums.Countries.Angola, 3, new DateTime(2023, 01, 15), new DateTime(2023, 01, 27), Enums.TripTypes.Leisure);
        trip.PackingList.Add(new OtherItem("Pants", 5));
        trip.PackingList.Add(new TravelDocument("Insurance paper", true));

        Vacation vacation = new Vacation("Palm Tree Resort", Enums.Countries.Spain, 4, new DateTime(2023, 04, 01), new DateTime(2023, 04, 08), true);
        vacation.PackingList.Add(new OtherItem("Rubber duck", 1));
        vacation.PackingList.Add(new TravelDocument("Bucket List", false));

        //add travels to user's list of Travel
        AddTravelToUser(trip);
        AddTravelToUser(vacation);

        //add travels to travelmanager list of travels
        travelManager.AddTravel(trip);
        travelManager.AddTravel(vacation);

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

    //public void RemoveUser(IUser userToRemove)
    //{
    //    //Maybe implement - don't need to!
    //}

    public bool UpdateUsername (IUser userToUpdate, string username)
    {        
        if (ValidateUsername(username))
        {
            userToUpdate.UserName = username;
            return true;
        }
        return false;
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

    public void UpdateUserLocation(IUser userToUpdate, Countries country)
    {
        //check if user location is inside Europe before change 
        bool isInsideEuropeBeforeChange = CheckIfCountryIsInEurope(userToUpdate.Location.ToString());

        userToUpdate.Location = country;

        //check if user location is inside Europe after change
        bool isInsideEuropeAfterChange = CheckIfCountryIsInEurope(userToUpdate.Location.ToString());

        if ((isInsideEuropeBeforeChange != isInsideEuropeAfterChange) && userToUpdate is User)
        {
            //the travel documents must change
            //if userToUpdate is admin no changes needs to be made since admin has no travels

            User user = userToUpdate as User;
            UpdateTravelDocuments(user);
        }

    }

    private bool CheckIfCountryIsInEurope(string country)
    {
        foreach (string europeanCountry in Enum.GetNames(typeof(EuropeanCountries)))
        {
            if (country == europeanCountry)
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateTravelDocuments(User user)
    {
        foreach (Travel travel in user.Travels)
        {            
            foreach (IPackingListItem item in travel.PackingList)
            {
                if (item is TravelDocument && item.Name.ToLower() == "passport")
                {
                    TravelDocument passport = item as TravelDocument;

                    passport.Required = CheckIfPassportShouldBeRequired(user.Location, travel.Country);

                }
            }            
        }       
    }

    private bool CheckIfPassportShouldBeRequired(Countries userLocation, Countries travelDestination)
    {    
        bool isInsideEuropeUserLocation = CheckIfCountryIsInEurope(userLocation.ToString());
        bool isInsideEuropeTravelCountry = CheckIfCountryIsInEurope(travelDestination.ToString());

        //if travel country is outside of EU & user lives in EU or if user lives outside of EU
        if ((!isInsideEuropeUserLocation) || (!isInsideEuropeTravelCountry && isInsideEuropeUserLocation))
        {
            return true;
        }
        //else if travel country is inside of EU and the user lives in EU
        else 
        {
            return false;
        }
    }

    public void UpdateUserPassword(IUser userToUpdate, string newPassword)
    {
        userToUpdate.Password = newPassword;
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

    
    //adds a travel to the users list of travels
    public void AddTravelToUser(Travel travelToAdd)
    {
        //cast to User to access Travels
        User user = SignedInUser as User;
        user.Travels.Add(travelToAdd);

        //add default travel document to packing item list
        AddDefaultTravelDocuments(travelToAdd);  
    }

    private void AddDefaultTravelDocuments(Travel travel)
    {
        //if Required
        if (CheckIfPassportShouldBeRequired(SignedInUser.Location, travel.Country))
        {
            //add TravelDocument with name "Passport" and required set to true
            travel.PackingList.Add(new TravelDocument("Passport", true));  
        }
        //if not Required
        else if (!CheckIfPassportShouldBeRequired(SignedInUser.Location, travel.Country))
        {
            //add TravelDocument with name "Passport" and required set to false
            travel.PackingList.Add(new TravelDocument("Passport", false));
        }

    }

    

    public void RemoveTravelFromUser(Travel travelToRemove)
    {
        //if signedInUser is User, remove travel directly from the users list
        if (SignedInUser is User)
        {
            User user = SignedInUser as User;
            user.Travels.Remove(travelToRemove);
        }
        //if signedInUser is Admin, go through all users in list of users to see which travel matches the selected travel
        else if (SignedInUser is Admin)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i] is User)
                {
                    User selectedUser = Users[i] as User;

                    for (int j = 0; j < selectedUser.Travels.Count; j++)
                    {
                        if (selectedUser.Travels[j].Equals(travelToRemove))
                        {
                            selectedUser.Travels.Remove(travelToRemove);
                        }
                    }
                }
            }
            
        }
    }

}
