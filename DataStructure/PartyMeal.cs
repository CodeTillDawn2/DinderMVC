using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderMVC.DataStructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class PartyMeal : DataStructure<PartyMealDM, PartyMealDTO>
    {
        [NotMapped]
        public virtual Party Party { get; set; }
        [NotMapped]
        public virtual UserMeal Meal { get; set; }
        public int PartyID { get; set; }

        public int MealID { get; set; }


        public PartyMeal()
        {
        }

        public PartyMeal(int partyid, int mealid)
        {
            PartyID = partyid;
            MealID = mealid;
        }

        public PartyMealDM ReturnDM()
        {
            return new PartyMealDM(PartyID, MealID);
        }

        public PartyMealDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }

        public class PartyMealConfiguration : IEntityTypeConfiguration<PartyMeal>
        {
            public void Configure(EntityTypeBuilder<PartyMeal> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartyMeals", "dbo");

                // Set key for entity
                builder.HasKey(p => new { p.PartyID, p.MealID });

                // Columns with default value

                builder
                    .Property(p => p.PartyID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.MealID)
                    .HasColumnType("int")
                    .IsRequired();

                builder.HasOne(x => x.Party).WithMany(x => x.PartyMeals).HasForeignKey(a => a.PartyID).OnDelete(DeleteBehavior.NoAction);
                builder.HasOne(x => x.Meal).WithMany().HasForeignKey(a => a.MealID).OnDelete(DeleteBehavior.NoAction);
            }
        }

    }


#pragma warning restore CS1591
}
