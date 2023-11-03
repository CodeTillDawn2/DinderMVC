using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class PartySettingMatrix
    {
        public int PartyID { get; set; }

        public int SettingID { get; set; }

        public int ChoiceID { get; set; }

        public string ChoiceEntry { get; set; }

        [NotMapped]
        public virtual Party Party { get; set; }

        [NotMapped]
        public virtual PartySettingType Setting { get; set; }

        [NotMapped]
        public virtual PartySettingValue Choice { get; set; }

        public PartySettingMatrix()
        {
        }

        public PartySettingMatrix(int partyID, int settingID, int choiceID, string choiceEntry)
        {
            PartyID = partyID;
            SettingID = settingID;
            ChoiceID = choiceID;
            ChoiceEntry = choiceEntry;
        }

        public class PartySettingMatrixConfiguration : IEntityTypeConfiguration<PartySettingMatrix>
        {
            public void Configure(EntityTypeBuilder<PartySettingMatrix> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartySettingMatrix", "dbo");

                // Set key for entity
                builder.HasKey(p => p.PartyID);
                builder.HasKey(p => p.SettingID);

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
                   .HasColumnType("varchar(255)")
                   .IsRequired();

                builder.HasOne(x => x.Party).WithMany(b => b.Settings).HasForeignKey(a => a.PartyID).OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(x => x.Setting).WithOne().HasForeignKey<PartySettingType>(a => a.PartySettingID).OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(x => x.Choice).WithOne().HasForeignKey<PartySettingValue>(a => a.SettingChoiceID).OnDelete(DeleteBehavior.Restrict);
                //builder.HasOne(x => x.Friend).WithMany(b => b.)

                //builder.HasMany(x => x.Meals).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);

            }
        }

    }


#pragma warning restore CS1591
}
