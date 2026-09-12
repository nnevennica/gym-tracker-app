using System.Globalization;
using GymTracker.Application.DTOs;
using GymTracker.Application.Interfaces;
using GymTracker.Domain.Common;
using MediatR;

namespace GymTracker.Application.Workouts.Queries;

public record GetWeeklyDetailsQuery(
    Guid UserId,
    int Year,
    int Month,
    int WeekNumber
) : IRequest<Result<List<WorkoutItemDto>>>;

public class GetWeeklyDetailsQueryHandler : IRequestHandler<GetWeeklyDetailsQuery, Result<List<WorkoutItemDto>>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetWeeklyDetailsQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<Result<List<WorkoutItemDto>>> Handle(GetWeeklyDetailsQuery request, CancellationToken cancellationToken)
    {
        var userWorkouts = await _workoutRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var monthlyWorkouts = userWorkouts
            .Where(w => w.DateTime.Year == request.Year && w.DateTime.Month == request.Month)
            .ToList();

        var firstDayOfMonth = new DateTime(request.Year, request.Month, 1);
        var startWeekOfPeriod = ISOWeek.GetWeekOfYear(firstDayOfMonth);

        var filteredWorkouts = monthlyWorkouts
            .Where(w => (ISOWeek.GetWeekOfYear(w.DateTime) - startWeekOfPeriod + 1) == request.WeekNumber)
            .Select(w => new WorkoutItemDto
            {
                Id = w.Id,
                ExerciseTypeName = w.ExerciseType != null ? w.ExerciseType.Name : "Trening",
                DateTime = w.DateTime,
                DurationMinutes = w.DurationMinutes,
                CaloriesBurned = w.CaloriesBurned,
                IntensityRating = w.IntensityRating,
                FatigueRating = w.FatigueRating,
                Notes = w.Notes
            })
            .ToList();

        return Result<List<WorkoutItemDto>>.Success(filteredWorkouts);
    }
}