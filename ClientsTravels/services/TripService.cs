using ClientsTravels.dtos;
using ClientsTravels.Models;
using ClientsTravels.repositories;

namespace ClientsTravels.services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<TripResponseDto> GetTripsAsync(int page, int pageSize)
    {
        var (trips, totalCount) = await _tripRepository.GetPagedTripsAsync(page, pageSize);

        var tripDtos = trips.Select(t => new TripDto
        {
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom,
            DateTo = t.DateTo,
            MaxPeople = t.MaxPeople,

            Countries = t.IdCountries.Select(c => new CountryDto
            {
                Name = c.Name
            }).ToList(),

            Clients = t.ClientTrips.Select(ct => new ClientDto
            {
                FirstName = ct.IdClientNavigation.FirstName,  // not ct.Client
                LastName = ct.IdClientNavigation.LastName
            }).ToList()

        }).ToList();

        return new TripResponseDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            Trips = tripDtos
        };
    }
    
    public async Task<(bool Success, string? ErrorMessage)> AssignClientToTripAsync(int idTrip, AssignClientToTripRequest dto, CancellationToken cancellationToken)
    {
        if (await _tripRepository.ClientExistsByPeselAsync(dto.Pesel, cancellationToken))
        {
            return (false, "Client with given PESEL already exists.");
        }

        if (await _tripRepository.ClientAlreadyInTripAsync(dto.Pesel, idTrip, cancellationToken))
        {
            return (false, "Client with this PESEL is already registered for this trip.");
        }

        var trip = await _tripRepository.GetTripByIdAsync(idTrip, cancellationToken);
        if (trip == null)
            return (false, "Trip not found.");
        if (trip.DateFrom <= DateTime.Now)
            return (false, "Cannot assign to a trip that has already started or ended.");

        var client = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };

        var created = await _tripRepository.AddClientAsync(client, cancellationToken);
        await _tripRepository.AddClientToTripAsync(
            created.IdClient,
            idTrip,
            dto.PaymentDate,
            DateTime.Now,
            cancellationToken
        );

        return (true, null);
    }
}