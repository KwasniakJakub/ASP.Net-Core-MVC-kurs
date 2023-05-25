using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.Models;

public class Address
{
    [Key] 
    public int Int { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}