using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.Core.Domain.Entities.WorkoutAnalysis;

namespace TrainWise.Core.Domain.Entities.WorkoutComponents
{
    public class Exercise
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public MuscleGroupType PrimaryMuscleGroup { get; set; }
        public List<MuscleGroupType> SecondaryMuscles { get; set; }

        // Imagem / Gifs / Videos
        public string? ImagePath { get; private set; }
        public string? MediaPath { get; private set; }
        public MediaType? MediaType { get; private set; }
        public bool IsCompound { get; set; } // Exercício composto ou isolado

        public bool IsApproved { get; set; } = false;
        public DateTime? ApprovedAt { get; set; }

        // Relacionamentos
        public ICollection<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; }
        public ICollection<PerformanceSnapshot> PerformanceSnapshots { get; set; }
        public ICollection<ProgressAnalysis> ProgressAnalyses { get; set; }

        public Exercise(string name)
        {
            Name = name;
            SecondaryMuscles = new List<MuscleGroupType>();
            WorkoutTemplateExercises = new List<WorkoutTemplateExercise>();
            PerformanceSnapshots = new List<PerformanceSnapshot>();
            ProgressAnalyses = new List<ProgressAnalysis>();
        }

        public void SetMedia(string imagePath, string mediaPath, MediaType mediaType)
        {
            ImagePath = imagePath;
            MediaPath = mediaPath;
            MediaType = mediaType;
        }


        public void Approve()
        {
            IsApproved = true;
            ApprovedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            IsApproved = false;
            ApprovedAt = null;
        }
    }

    public enum MuscleGroupType
    {
        // Principais
        Chest,          // Peito
        Back,           // Costas
        Shoulders,      // Ombros
        Biceps,         // Bíceps
        Triceps,        // Tríceps
        Forearms,       // Antebraços
        Quadriceps,     // Quadríceps
        Hamstrings,     // Posteriores de coxa
        Glutes,         // Glúteos
        Calves,         // Panturrilhas
        Abs,            // Abdômen
        LowerBack,      // Lombar
        Traps,          // Trapézio
        
        // Compostos
        UpperBody,
        LowerBody,
        FullBody
    }

    public enum MediaType{
        Image,
        Gif,
        Video
    }

  
    }
