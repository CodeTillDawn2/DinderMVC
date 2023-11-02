using DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderBackEndv2.Models
{
#pragma warning disable CS1591

    public class Meal
    {

        public Guid UserGUID { get; set; }
        public int MealID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }

        public Guid? GlobalLink { get; set; }

        public bool MadeItBefore { get; set; }

        public string PrivateNotes { get; set; }
        [NotMapped]
        public virtual User User { get; set; }


        public Meal()
        {

        }

        public Meal(int mealID)
        {
            MealID = mealID;
        }

        public MealDM ReturnDM()
        {
            return new MealDM(UserGUID, MealID, MealName, MealDescription, MadeItBefore, PrivateNotes, GlobalLink);
        }

        public class MealsConfiguration : IEntityTypeConfiguration<Meal>
        {
            public void Configure(EntityTypeBuilder<Meal> builder)
            {


                // Set configuration for entity
                builder.ToTable("Meals", "dbo");

                // Set key for entity
                builder.HasKey(p => p.MealID);

                // Columns with default value

                builder
                    .Property(p => p.UserGUID)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired()
                    .HasDefaultValueSql("(newid())");

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
