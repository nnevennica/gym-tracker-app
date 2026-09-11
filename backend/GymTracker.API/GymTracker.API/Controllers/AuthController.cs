using GymTracker.Application.Auth;
using GymTracker.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(dto.Email, dto.Password, dto.FirstName, dto.LastName);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(new { message = "Registracija uspešna. Proverite email za potvrdu.", userId = result.Value });
    }
}