namespace GymTracker.Application.DTOs;

public record WeeklyStatsDto(
    int WeekNumber,
    DateTime StartOfWeek,
    DateTime EndOfWeek,
    int TotalWorkouts,
    int TotalDurationMinutes,
    double AverageIntensity,
    double AverageFatigue
);