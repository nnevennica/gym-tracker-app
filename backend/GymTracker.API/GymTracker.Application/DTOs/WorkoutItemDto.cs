namespace GymTracker.Application.DTOs;

public class WorkoutItemDto
{
    public Guid Id { get; set; }
    public string ExerciseTypeName { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public int DurationMinutes { get; set; }
    public int CaloriesBurned { get; set; }
    public int IntensityRating { get; set; }
    public int FatigueRating { get; set; }
    public string? Notes { get; set; }
}
