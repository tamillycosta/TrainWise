using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainWise.Core.Application.Dtos
{
    public class ExternalExerciseDto
    {
        public  string Id { get; set; }
        public string Name { get; set; }
        public string Target { get; set; } // músculo principal
        public string BodyPart { get; set; }
        public string Equipment { get; set; }
        public List<string> SecondaryMuscles { get; set; }
        public  List<string> Instructions { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }
        public string GifUrl {get;set;}
        

    }
}