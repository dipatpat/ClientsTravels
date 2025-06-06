using ClientsTravels.dtos;

namespace ClientsTravels.services;

public interface ITripService
{
    Task<TripResponseDto> GetTripsAsync(int page, int pageSize);
    Task<(bool Success, string? ErrorMessage)> AssignClientToTripAsync(int idTrip, AssignClientToTripRequest dto, CancellationToken cancellationToken);
}