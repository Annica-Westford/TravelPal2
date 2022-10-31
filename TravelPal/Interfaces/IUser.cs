using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;

namespace TravelPal.Interfaces;

public interface IUser
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public Countries Location { get; set; } 

}
