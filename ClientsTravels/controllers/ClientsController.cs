using ClientsTravels.services;
using Microsoft.AspNetCore.Mvc;

namespace ClientsTravels.controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient, CancellationToken cancellationToken)
    {
        var (success, error) = await _clientService.DeleteClientAsync(idClient, cancellationToken);
        if (!success)
            return BadRequest(new { error });

        return NoContent();
    }
}