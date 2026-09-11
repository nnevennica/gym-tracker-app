using GymTracker.Application.DTOs;
using GymTracker.Domain.Common;
using MediatR;

namespace GymTracker.Application.Workouts.Queries;

public record GetMonthlyProgressQuery(
    Guid UserId,
    int Year,
    int Month
) : IRequest<Result<MonthlyProgressDto>>;