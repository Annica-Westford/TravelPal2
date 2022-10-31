using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;

namespace TravelPal.Models;

public class Vacation : Travel
{
    public bool IsAllInclusive { get; set; }
    public Vacation(string destination, Countries country, int travellers, DateTime startDate, DateTime endDate, bool isAllInclusive) : base(destination, country, travellers, startDate, endDate)
    {
        IsAllInclusive = isAllInclusive;
    }

    public override string GetInfo()
    {
        if (IsAllInclusive)
        {
            return $"{base.GetInfo()} | All-Inclusive";
        }
        else
        {
            return $"{base.GetInfo()} | Not All-Inclusive";
        }
        
    }
}
