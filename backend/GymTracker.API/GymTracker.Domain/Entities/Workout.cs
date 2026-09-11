namespace GymTracker.Domain.Entities;

public class Workout
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ExerciseTypeId { get; private set; }
    public ExerciseType? ExerciseType { get; private set; }

    public DateTime DateTime { get; private set; }
    public int DurationMinutes { get; private set; }
    public int CaloriesBurned { get; private set; }
    public int IntensityRating { get; private set; }
    public int FatigueRating { get; private set; }
    public string? Notes { get; private set; }

    private Workout() { } // Za Entity Framework

    public Workout(Guid userId, Guid exerciseTypeId, DateTime dateTime, int durationMinutes, int caloriesBurned, int intensityRating, int fatigueRating, string? notes)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ExerciseTypeId = exerciseTypeId;
        DateTime = dateTime;
        DurationMinutes = durationMinutes;
        CaloriesBurned = caloriesBurned;
        IntensityRating = intensityRating;
        FatigueRating = fatigueRating;
        Notes = notes;
    }
}