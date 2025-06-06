using ClientsTravels.dtos;
using ClientsTravels.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientsTravels.controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _tripService.GetTripsAsync(page, pageSize);
        return Ok(result);
    }
    
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] AssignClientToTripRequest request)
    {
        if (idTrip != request.IdTrip)
        {
            return BadRequest(new
            {
                error = "The route trip ID does not match the trip ID in the request body."
            });
        }
        var (success, error) = await _tripService.AssignClientToTripAsync(idTrip, request, HttpContext.RequestAborted);

        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Client assigned to trip successfully." });
    }
}