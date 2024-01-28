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

    public class PartyChoice : DataStructure<PartyChoiceDM, PartyChoiceDTO>
    {

        public int PartyID { get; set; }

        public Guid UserGUID { get; set; }

        public int MealID { get; set; }

        public int SwipeChoice { get; set; }

        [NotMapped]
        public virtual Party Party { get; set; }


        public PartyChoice()
        {
        }

        public PartyChoice(int partyid, Guid userGUID, int mealid, int swipeChoice)
        {
            PartyID = partyid;
            UserGUID = userGUID;
            MealID = mealid;
            SwipeChoice = swipeChoice;
        }

        public PartyChoiceDM ReturnDM()
        {
            return new PartyChoiceDM(PartyID, UserGUID, MealID, SwipeChoice);
        }

        public PartyChoiceDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }

        public class PartyChoiceConfiguration : IEntityTypeConfiguration<PartyChoice>
        {
            public void Configure(EntityTypeBuilder<PartyChoice> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartyChoices", "dbo");

                // Set key for entity
                builder.HasKey(p => new { p.PartyID, p.UserGUID, p.MealID });


                // Columns with default value

                builder
                    .Property(p => p.PartyID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.UserGUID)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                builder
                    .Property(p => p.MealID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                 .Property(p => p.SwipeChoice)
                 .HasColumnType("int")
                 .IsRequired();

            }
        }

    }


#pragma warning restore CS1591
}
