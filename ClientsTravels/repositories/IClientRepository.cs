using ClientsTravels.Models;

namespace ClientsTravels.repositories;

public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> ClientHasTripsAsync(int id, CancellationToken cancellationToken);
    Task DeleteClientAsync(Client client, CancellationToken cancellationToken);
}