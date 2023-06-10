using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sirena.Travel.TestTask.Contracts.Models;

public class SearchFilters
{
    // Optional
    // End date of route
    public DateTime? DestinationDateTime { get; set; }

    // Optional
    // Maximum price of route
    public decimal? MaxPrice { get; set; }

    // Optional
    // Minimum value of timelimit for route
    public DateTime? MinTimeLimit { get; set; }

    // Optional
    // Forcibly search in cached data
    public bool? OnlyCached { get; set; }
}
