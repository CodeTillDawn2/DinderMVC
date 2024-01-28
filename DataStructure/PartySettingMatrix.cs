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

    public class PartySettingMatrix : DataStructure<PartySettingMatrixDM, PartySettingMatrixDTO>
    {
        public int PartyID { get; set; }

        public int SettingID { get; set; }

        public int ChoiceID { get; set; }

        public String ChoiceEntry { get; set; }

        [NotMapped]
        public virtual Party Party { get; set; }

        [NotMapped]
        public virtual PartySettingType Setting { get; set; }

        [NotMapped]
        public virtual PartySettingValue Choice { get; set; }

        public PartySettingMatrix()
        {
        }

        public PartySettingMatrix(int partyID, int settingID, int choiceID, String choiceEntry = null)
        {
            PartyID = partyID;
            SettingID = settingID;
            ChoiceID = choiceID;
            ChoiceEntry = choiceEntry;
        }



        public PartySettingMatrixDM ReturnDM()
        {
            return new PartySettingMatrixDM(PartyID, SettingID, ChoiceID, ChoiceEntry);
        }

        public PartySettingMatrixDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }

        public class PartySettingMatrixConfiguration : IEntityTypeConfiguration<PartySettingMatrix>
        {
            public void Configure(EntityTypeBuilder<PartySettingMatrix> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartySettingMatrix", "dbo");

                // Set key for entity
                builder.HasKey(p => new { p.PartyID, p.SettingID });

                // Columns with default value

                builder
                    .Property(p => p.PartyID)
                    .HasColumnType("int")
                    .IsRequired();



                builder
                    .Property(p => p.SettingID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.ChoiceID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                   .Property(p => p.ChoiceEntry)
                   .HasColumnType("varchar(255)");

                builder.HasOne(x => x.Setting).WithMany().HasForeignKey(a => a.SettingID).OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(x => x.Party).WithMany(b => b.Settings).HasForeignKey(a => a.PartyID).OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(x => x.Choice).WithMany().HasForeignKey(a => new { a.SettingID, a.ChoiceID }).OnDelete(DeleteBehavior.Restrict);
                //builder.HasOne(x => x.Friend).WithMany(b => b.)

                //builder.HasMany(x => x.Meals).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);

            }
        }

    }


#pragma warning restore CS1591
}
