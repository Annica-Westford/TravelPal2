using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;

namespace TravelPal.Models;

public class Trip : Travel
{
    public TripTypes TripType { get; set; }
    public Trip(string destination, Countries country, int travellers, DateTime startDate, DateTime endDate, IUser travelOwner, TripTypes tripType) : base(destination, country, travellers, startDate, endDate, travelOwner)
    {
        TripType = tripType;
    }

    public override string GetInfo()
    {
        return $"{base.GetInfo()} | {TripType} Trip";
    }
}
