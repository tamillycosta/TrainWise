namespace Entities
{
    public class Exercise

    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public ValueObjects.ExerciseImage Image { get; private set; }
        private readonly List<ExerciseMuscle> _muscles = new();
        public IReadOnlyCollection<ExerciseMuscle> Muscles => _muscles;
        public Exercise(string name, ValueObjects.ExerciseImage image)
        {
            Id =  Guid.NewGuid();
            Name = name;
            Image = image;

        }
         public void AddMuscle(Muscle muscle, Enumns.MuscleRole role)
        {
            if (_muscles.Any(m => m.MuscleId == muscle.Id))
                return;

            _muscles.Add(new ExerciseMuscle(Id, muscle.Id, role));
        }
    }
}