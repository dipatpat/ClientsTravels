namespace ClientsTravels.services;

public interface IClientService
{
    
    Task<(bool Success, string? ErrorMessage)> DeleteClientAsync(int idClient, CancellationToken cancellationToken);    
}

