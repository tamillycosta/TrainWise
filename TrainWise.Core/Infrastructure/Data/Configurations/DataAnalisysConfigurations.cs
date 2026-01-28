using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.Core.Domain.Entities.UsersComponets;
using TrainWise.Core.Domain.Entities.WorkoutAnalysis;
using TrainWise.Core.Domain.Entities.WorkoutComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace TrainWise.Core.Infrastructure.Data.Configurations {
    public class ProgressAnalysisConfigurations : IEntityTypeConfiguration<ProgressAnalysis>{

        public void Configure(EntityTypeBuilder<ProgressAnalysis> builder){
                builder.HasKey(e => e.Id);
                
                builder.Property(e => e.Observations)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>()
                    );
                
                builder.HasOne(e => e.User)
                    .WithMany(u => u.ProgressAnalyses)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                builder.HasOne(e => e.Exercise)
                    .WithMany(ex => ex.ProgressAnalyses)
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);
            }

        }

    public class AiRecommendationConfigurations : IEntityTypeConfiguration<AiRecommendation>{
        
        public void Configure(EntityTypeBuilder<AiRecommendation> builder){
               builder.HasKey(e => e.Id);
               builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
                
               builder.HasOne(e => e.User)
                    .WithMany(u => u.AIRecommendations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class VolumeAnalysisConfigurations : IEntityTypeConfiguration<VolumeAnalysis> {

        public void Configure(EntityTypeBuilder<VolumeAnalysis> builder){
              builder.HasKey(e => e.Id);
                
              builder.HasOne(e => e.User)
                    .WithMany(u => u.VolumeAnalyses)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
        



 }



