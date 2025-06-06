namespace ClientsTravels.dtos;

public class AssignClientToTripRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telephone { get; set; } = null!;
    public string Pesel { get; set; } = null!;
    public int IdTrip { get; set; }              // Included in the body
    public string TripName { get; set; } = null!; // Optional extra info
    public DateTime? PaymentDate { get; set; }   // Can be null
}