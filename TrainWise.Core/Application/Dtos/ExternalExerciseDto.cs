namespace TrainWise.Core.Application.Dtos
{
    public class ExternalExerciseDto
    {
      
        public string Id { get; set; }
        public string Name { get; set; }
        public string Force { get; set; }      
        public string Level { get; set; }        
        public string Mechanic { get; set; }     
        public string Equipment { get; set; }
        public List<string> PrimaryMuscles { get; set; }
        public List<string> SecondaryMuscles { get; set; }
        public List<string> Instructions { get; set; }
        public string Category { get; set; }
        public List<string> Images { get; set; }
        
        // URL base das imagens
        private const string ImageBaseUrl = 
            "https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/exercises/";
        
       
        public string? ThumbnailUrl => Images?.Any() == true 
            ? $"{ImageBaseUrl}{Images[0]}" 
            : null;
        
    
        public string? GifUrl => Images?.Count > 1 
            ? $"{ImageBaseUrl}{Images[1]}" 
            : ThumbnailUrl;
    }
}