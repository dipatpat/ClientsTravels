using ClientsTravels.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientsTravels.repositories;

public class ClientRepository : IClientRepository
{
    private readonly MasterContext _context;

    public ClientRepository(MasterContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Clients.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<bool> ClientHasTripsAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.ClientTrips.AnyAsync(ct => ct.IdClient == id, cancellationToken);
    }

    public async Task DeleteClientAsync(Client client, CancellationToken cancellationToken)
    {
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync(cancellationToken);
    }
}