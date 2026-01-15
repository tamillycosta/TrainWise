namespace ValueObjects
{
    public class ExerciseImage
    {
        public string Path { get; }

        public ExerciseImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Image path cannot be empty");

            Path = path;
        }
    }
}