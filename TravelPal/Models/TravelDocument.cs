using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;

namespace TravelPal.Models;

public class TravelDocument : IPackingListItem
{
    public string Name { get; set; }
    public bool Required { get; set; }

    public TravelDocument(string name, bool required)
    {
        Name = name;
        Required = required;
    }

    public string GetInfo()
    {
        StringBuilder sb = new($"{Name} - Required: ");
        if (Required)
        {
            sb.Append("Yes");
        }
        else
        {
            sb.Append("No");
        }

        return sb.ToString();
    }
}
