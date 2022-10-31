using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Models;

namespace TravelPal.Managers;

public class TravelManager
{
    public List<Travel> Travels { get; set; } = new();

    public void AddTravel(Travel travelToAdd)
    {
        Travels.Add(travelToAdd);
    }

    public void RemoveTravel(Travel travelToRemove)
    {
        Travels.Remove(travelToRemove);
    }
}
