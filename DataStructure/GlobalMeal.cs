using DinderDLL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class GlobalMeal
    {

        public Guid GlobalMealGUID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }

        public GlobalMeal()
        {

        }

        public GlobalMeal(Guid globalMealGuid)
        {
            GlobalMealGUID = globalMealGuid;
        }

        public GlobalMealDM ReturnDM()
        {
            return new GlobalMealDM(GlobalMealGUID, MealName, MealDescription);
        }

        public class GlobalMealsConfiguration : IEntityTypeConfiguration<GlobalMeal>
        {
            public void Configure(EntityTypeBuilder<GlobalMeal> builder)
            {


                // Set configuration for entity
                builder.ToTable("GlobalMeals", "dbo");

                // Set key for entity
                builder.HasKey(p => p.GlobalMealGUID);

                // Columns with default value

                builder
                    .Property(p => p.GlobalMealGUID)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired()
                    .HasDefaultValueSql("(newid())");

                // Set configuration for columns
                builder.Property(p => p.MealName).HasColumnType("varchar(50)").IsRequired();
                builder.Property(p => p.MealDescription).HasColumnType("varchar(255)");
            }
        }

    }


#pragma warning restore CS1591
}
