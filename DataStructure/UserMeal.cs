using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderMVC.DataStructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class UserMeal : DataStructure<UserMealDM, UserMealDTO>
    {

        public Guid HostGuid { get; set; }
        public int MealID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }

        public Guid? GlobalLink { get; set; }

        public bool MadeItBefore { get; set; }

        public string PrivateNotes { get; set; }
        [NotMapped]
        public virtual User Cook { get; set; }


        public UserMeal()
        {

        }

        public UserMeal(Guid hostGuid, int mealID)
        {
            MealID = mealID;
            HostGuid = hostGuid;
        }

        public UserMealDM ReturnDM()
        {
            return new UserMealDM("", HostGuid, MealID, MealName, MealDescription, MadeItBefore, PrivateNotes, GlobalLink);
        }

        public UserMealDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }

        public class UserMealsConfiguration : IEntityTypeConfiguration<UserMeal>
        {
            public void Configure(EntityTypeBuilder<UserMeal> builder)
            {


                // Set configuration for entity
                builder.ToTable("UserMeals", "dbo");

                // Set key for entity
                builder.HasKey(p => new { p.MealID });

                // Columns with default value

                builder
                    .Property(p => p.HostGuid)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                builder.Property(p => p.MealID).HasColumnType("int").UseIdentityColumn();

                // Set configuration for columns
                builder.Property(p => p.MealName).HasColumnType("varchar(50)").IsRequired();
                builder.Property(p => p.MealDescription).HasColumnType("varchar(255)");
                builder.Property(p => p.GlobalLink).HasColumnType("uniqueidentifier");
                builder.Property(p => p.MadeItBefore).HasColumnType("bit").IsRequired();
                builder.Property(p => p.PrivateNotes).HasColumnType("varchar(255)");
            }
        }

    }


#pragma warning restore CS1591
}
