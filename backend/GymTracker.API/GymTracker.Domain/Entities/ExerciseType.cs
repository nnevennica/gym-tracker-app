namespace GymTracker.Domain.Entities;

public class ExerciseType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private ExerciseType() { }

    public ExerciseType(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}