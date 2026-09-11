namespace GymTracker.Application.DTOs;

public record WorkoutDto(
    Guid Id,
    Guid ExerciseTypeId,
    string ExerciseTypeName,
    DateTime DateTime,
    int DurationMinutes,
    int CaloriesBurned,
    int IntensityRating,
    int FatigueRating,
    string? Notes
);