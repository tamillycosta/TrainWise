namespace Entities
{
    public class ExerciseMuscle
    {
        public Guid ExerciseId { get; private set; }
        public Guid MuscleId { get; private set; }
        public Enumns.MuscleRole Role { get; private set; }

        public ExerciseMuscle(Guid exerciseId, Guid muscleId, Enumns.MuscleRole role)
        {
            ExerciseId = exerciseId;
            MuscleId = muscleId;
            Role = role;
        }
}

}