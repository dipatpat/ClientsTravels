using ClientsTravels.Models;
using ClientsTravels.repositories;

namespace ClientsTravels.services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteClientAsync(int idClient, CancellationToken cancellationToken)
    {
        var client = await _repository.GetClientByIdAsync(idClient, cancellationToken);
        if (client == null)
            return (false, "Client not found.");

        if (await _repository.ClientHasTripsAsync(idClient, cancellationToken))
            return (false, "Client has assigned trips and cannot be deleted.");

        await _repository.DeleteClientAsync(client, cancellationToken);
        return (true, null);
    }

}
