namespace Sirena.Travel.TestTask.Contracts.Models.ProviderTwo;

// HTTP POST http://provider-two/api/v1/search
public class ProviderTwoSearchRequest
{
    // Mandatory
    // Start point of route, e.g. Moscow 
    public string Departure { get; set; }

    // Mandatory
    // End point of route, e.g. Sochi
    public string Arrival { get; set; }

    // Mandatory
    // Start date of route
    public DateTime DepartureDate { get; set; }

    // Optional
    // Minimum value of timelimit for route
    public DateTime? MinTimeLimit { get; set; }
}

// HTTP GET http://provider-two/api/v1/ping
//      - HTTP 200 if provider is ready
//      - HTTP 500 if provider is down