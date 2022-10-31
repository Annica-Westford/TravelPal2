using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;

namespace TravelPal.Models;

public class User : IUser
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public Countries Location { get; set; }
    public List<Travel> Travels { get; set; } = new(); 

    public User(string userName, string password, Countries location)
    {
        UserName = userName;
        Password = password;
        Location = location;
    }
}
