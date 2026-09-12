using GymTracker.Application.DTOs;
using GymTracker.Application.Workouts.Commands;
using GymTracker.Application.Workouts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkoutsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Unos novog treninga
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateWorkout([FromBody] CreateWorkoutCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.Error });
        }

        return CreatedAtAction(nameof(GetMonthlyProgress), new { userId = command.UserId, year = command.DateTime.Year, month = command.DateTime.Month }, new { id = result.Value });
    }

    /// <summary>
    /// Pracenjee mesec napretka (nedeljna statistika)
    /// </summary>
    [HttpGet("progress")]
    public async Task<IActionResult> GetMonthlyProgress([FromQuery] Guid userId, [FromQuery] int year, [FromQuery] int month, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty || year < 2000 || month < 1 || month > 12)
        {
            return BadRequest(new { error = "Neispravni parametri za godinu, mesec ili ID korisnika." });
        }

        var query = new GetMonthlyProgressQuery(userId, year, month);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Pregled pojed. treninga za izabranu nedelju
    /// </summary>
    [HttpGet("weekly-details")]
    public async Task<IActionResult> GetWeeklyDetails([FromQuery] Guid userId, [FromQuery] int year, [FromQuery] int month, [FromQuery] int weekNumber, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty || year < 2000 || month < 1 || month > 12 || weekNumber < 1)
        {
            return BadRequest(new { error = "Neispravni parametri." });
        }

        var query = new GetWeeklyDetailsQuery(userId, year, month, weekNumber);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }
}