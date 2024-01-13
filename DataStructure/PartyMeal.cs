using DinderDLL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class PartyMeal
    {
        [NotMapped]
        public virtual Party Party { get; set; }
        [NotMapped]
        public virtual UserMeal Meal { get; set; }
        public int PartyID { get; set; }
        public Guid CookGuid { get; set; }

        public int MealID { get; set; }


        public PartyMeal()
        {
        }

        public PartyMeal(int partyid, Guid cookGuid, int mealid)
        {
            PartyID = partyid;
            CookGuid = cookGuid;
            MealID = mealid;
        }

        public PartyMealDM ReturnDM()
        {
            return new PartyMealDM(PartyID, CookGuid, MealID);
        }

        public class PartyMealConfiguration : IEntityTypeConfiguration<PartyMeal>
        {
            public void Configure(EntityTypeBuilder<PartyMeal> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartyMeals", "dbo");

                // Set key for entity
                builder.HasKey(p => p.PartyID);
                builder.HasKey(p => p.CookGuid);
                builder.HasKey(p => p.MealID);

                // Columns with default value

                builder
                    .Property(p => p.PartyID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.CookGuid)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                builder
                    .Property(p => p.MealID)
                    .HasColumnType("int")
                    .IsRequired();

                builder.HasOne(x => x.Party).WithMany(x => x.Meals).HasForeignKey(a => a.PartyID).OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(x => x.Meal).WithOne().HasForeignKey<UserMeal>(a => a.MealID).OnDelete(DeleteBehavior.Restrict);

            }
        }

    }


#pragma warning restore CS1591
}
