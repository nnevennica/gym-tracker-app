using GymTracker.Domain.Common;
using MediatR;

namespace GymTracker.Application.Workouts.Commands;

public record CreateWorkoutCommand(
    Guid UserId,
    Guid ExerciseTypeId,
    DateTime DateTime,
    int DurationMinutes,
    int CaloriesBurned,
    int IntensityRating,
    int FatigueRating,
    string? Notes
) : IRequest<Result<Guid>>;