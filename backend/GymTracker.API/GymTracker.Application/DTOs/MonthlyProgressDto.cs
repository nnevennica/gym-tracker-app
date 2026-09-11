namespace GymTracker.Application.DTOs;

public record MonthlyProgressDto(
    int Year,
    int Month,
    List<WeeklyStatsDto> WeeklyStats
);