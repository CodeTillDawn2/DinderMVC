using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class PartySettingType
    {
        public int PartySettingID { get; set; }

        public string SettingName { get; set; }

        public int SettingValueDataType { get; set; }

        //[NotMapped]
        //public virtual PartySettingValue SettingValue { get; set; }

        public PartySettingType()
        {
        }

        public PartySettingType(int partySettingID, string settingName, int settingValueDataType)
        {
            PartySettingID = partySettingID;
            SettingName = settingName;
            SettingValueDataType = settingValueDataType;
        }

        public class PartySettingTypeConfiguration : IEntityTypeConfiguration<PartySettingType>
        {
            public void Configure(EntityTypeBuilder<PartySettingType> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartySettingTypes", "dbo");

                // Set key for entity
                builder.HasKey(p => p.PartySettingID);

                // Columns with default value

                builder
                    .Property(p => p.PartySettingID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.SettingName)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                builder
                    .Property(p => p.SettingValueDataType)
                    .HasColumnType("int")
                    .IsRequired();


                //builder.HasOne(x => x.Friend).WithMany(b => b.)

                //builder.HasMany(x => x.Meals).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);

            }
        }

    }


#pragma warning restore CS1591
}
