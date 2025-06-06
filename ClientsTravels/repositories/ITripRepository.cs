using ClientsTravels.Models;

namespace ClientsTravels.repositories;

public interface ITripRepository
{
    Task<(List<Trip> Trips, int TotalCount)> GetPagedTripsAsync(int page, int pageSize);
    Task<bool> ClientExistsByPeselAsync(string pesel, CancellationToken cancellationToken);
    Task<bool> ClientAlreadyInTripAsync(string pesel, int idTrip, CancellationToken cancellationToken);
    Task<Trip?> GetTripByIdAsync(int idTrip, CancellationToken cancellationToken);
    Task<Client> AddClientAsync(Client client, CancellationToken cancellationToken);
    Task AddClientToTripAsync(int clientId, int tripId, DateTime? paymentDate, DateTime registeredAt, CancellationToken cancellationToken);
}