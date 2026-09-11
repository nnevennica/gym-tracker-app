using System.Globalization;
using GymTracker.Application.DTOs;
using GymTracker.Application.Interfaces;
using GymTracker.Domain.Common;
using MediatR;

namespace GymTracker.Application.Workouts.Queries;

public class GetMonthlyProgressQueryHandler : IRequestHandler<GetMonthlyProgressQuery, Result<MonthlyProgressDto>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetMonthlyProgressQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<Result<MonthlyProgressDto>> Handle(GetMonthlyProgressQuery request, CancellationToken cancellationToken)
    {
        var userWorkouts = await _workoutRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        // filter
        var filteredWorkouts = userWorkouts
            .Where(w => w.DateTime.Year == request.Year && w.DateTime.Month == request.Month)
            .ToList();

        var weeklyStatsList = new List<WeeklyStatsDto>();

        // kalendar-logika
        var calendar = CultureInfo.CurrentCulture.Calendar;
        var firstDayOfMonth = new DateTime(request.Year, request.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        // calendar week rule grupisanje
        var groupedByWeek = filteredWorkouts
            .GroupBy(w => calendar.GetWeekOfYear(w.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
            .OrderBy(g => g.Key);

        int weekCounter = 1;
        foreach (var weekGroup in groupedByWeek)
        {
            var workouts = weekGroup.ToList();

            weeklyStatsList.Add(new WeeklyStatsDto(
                WeekNumber: weekCounter++,
                StartOfWeek: workouts.Min(w => w.DateTime).Date,
                EndOfWeek: workouts.Max(w => w.DateTime).Date,
                TotalWorkouts: workouts.Count,
                TotalDurationMinutes: workouts.Sum(w => w.DurationMinutes),
                AverageIntensity: Math.Round(workouts.Average(w => w.IntensityRating), 1),
                AverageFatigue: Math.Round(workouts.Average(w => w.FatigueRating), 1)
            ));
        }

        var result = new MonthlyProgressDto(
            request.Year,
            request.Month,
            weeklyStatsList
        );

        return Result<MonthlyProgressDto>.Success(result);
    }
}