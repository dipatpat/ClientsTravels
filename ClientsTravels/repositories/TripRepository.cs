using ClientsTravels.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientsTravels.repositories;

public class TripRepository : ITripRepository
{
    private readonly MasterContext _context;

    public TripRepository(MasterContext context)
    {
        _context = context;
    }

    public async Task<(List<Trip>, int)> GetPagedTripsAsync(int page, int pageSize)
    {
        var total = await _context.Trips.CountAsync();

        var trips = await _context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries) // Many-to-many countries
            .OrderByDescending(t => t.DateFrom) // ✅ No fallback needed
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (trips, total);
    }
    
    public async Task<bool> ClientExistsByPeselAsync(string pesel, CancellationToken cancellationToken)
    {
        return await _context.Clients.AnyAsync(c => c.Pesel == pesel, cancellationToken);
    }

    public async Task<bool> ClientAlreadyInTripAsync(string pesel, int idTrip, CancellationToken cancellationToken)
    {
        return await _context.ClientTrips
            .Include(ct => ct.IdClientNavigation)
            .AnyAsync(ct => ct.IdTrip == idTrip && ct.IdClientNavigation.Pesel == pesel, cancellationToken);
    }

    public async Task<Trip?> GetTripByIdAsync(int idTrip, CancellationToken cancellationToken)
    {
        return await _context.Trips.FindAsync(new object[] { idTrip }, cancellationToken);
    }

    public async Task<Client> AddClientAsync(Client client, CancellationToken cancellationToken)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync(cancellationToken);
        return client;
    }

    public async Task AddClientToTripAsync(int clientId, int tripId, DateTime? paymentDate, DateTime registeredAt, CancellationToken cancellationToken)
    {
        _context.ClientTrips.Add(new ClientTrip
        {
            IdClient = clientId,
            IdTrip = tripId,
            PaymentDate = paymentDate,
            RegisteredAt = registeredAt
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}