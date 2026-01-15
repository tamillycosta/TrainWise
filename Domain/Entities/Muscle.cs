namespace Entities
{
    public class Muscle
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Enumns.MuscleGroup Group { get; private set; }
        public Muscle(string name, Enumns.MuscleGroup group)
        {
            Id = Guid.NewGuid();
            Name = name;
            Group = group;
    }
    }

    
}