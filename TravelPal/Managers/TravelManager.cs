using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;
using TravelPal.Models;

namespace TravelPal.Managers;

public class TravelManager
{
    public List<Travel> Travels { get; set; } = new();
    private UserManager userManager;

    public TravelManager(UserManager userManager)
    {
        this.userManager = userManager;
    }

    //add travel to user and to travels-list in travelmanager
    public void AddTravel(Travel travelToAdd)
    {
        Travels.Add(travelToAdd);

        //cast to User to access Travels
        User user = userManager.SignedInUser as User;
        user.Travels.Add(travelToAdd);

        //add default travel document to packing item list
        AddDefaultTravelDocuments(travelToAdd);
    }

    //add passport with Required set to true if passport is required, else add a Passport with Required set to false
    private void AddDefaultTravelDocuments(Travel travel)
    {
        //if Required
        if (CheckIfPassportShouldBeRequired(userManager.SignedInUser.Location, travel.Country))
        {
            //add TravelDocument with name "Passport" and required set to true
            travel.PackingList.Add(new TravelDocument("Passport", true));
        }
        //if not Required
        else 
        {
            //add TravelDocument with name "Passport" and required set to false
            travel.PackingList.Add(new TravelDocument("Passport", false));
        }

    }

    //return true if passport should be required, else return false
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

    //checks if a country is inside Europe or not (based on the enums)
    public bool CheckIfCountryIsInEurope(string country)
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

    //go through the user's travels list and change the "Required" property if needed
    public void UpdateTravelDocuments(User user)
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

    //remove travel from user's list of travels and from the travels list in travelmanager
    public void RemoveTravel(Travel travelToRemove)
    {
        Travels.Remove(travelToRemove);

        User travelOwner = travelToRemove.TravelOwner as User;
        travelOwner.Travels.Remove(travelToRemove);
    }

    
}
