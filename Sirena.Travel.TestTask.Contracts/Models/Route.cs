using Redis.OM.Modeling;

namespace Sirena.Travel.TestTask.Contracts.Models;

[Document(StorageType = StorageType.Json)]
public class Route
{
    // Mandatory
    // Identifier of the whole route
    [RedisIdField]
    [Indexed]
    public Guid Id { get; set; }

    // Mandatory
    // Start point of route
    [Indexed]
    public string Origin { get; set; }

    // Mandatory
    // End point of route
    [Indexed]
    public string Destination { get; set; }

    // Mandatory
    // Start date of route
    [Indexed]
    public DateTime OriginDateTime { get; set; }

    // Mandatory
    // End date of route
    [Indexed]
    public DateTime DestinationDateTime { get; set; }

    // Mandatory
    // Price of route
    [Indexed]
    public decimal Price { get; set; }

    // Mandatory
    // Timelimit. After it expires, route became not actual
    public DateTime TimeLimit { get; set; }
}
