using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinderBackEndv2.Models
{
#pragma warning disable CS1591

    public class PartySettingValue
    {
        public int SettingID { get; set; }

        public int SettingChoiceID { get; set; }

        public string SettingChoiceName { get; set; }

        public string SettingChoiceValue { get; set; }

        public PartySettingValue()
        {
        }

        public PartySettingValue(int settingID, int settingChoiceID, string settingChoiceName, string settingChoiceValue)
        {
            SettingID = settingID;
            SettingChoiceID = settingChoiceID;
            SettingChoiceName = settingChoiceName;
            SettingChoiceValue = settingChoiceValue;
        }

        public class PartySettingValueConfiguration : IEntityTypeConfiguration<PartySettingValue>
        {
            public void Configure(EntityTypeBuilder<PartySettingValue> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartySettingValue", "dbo");

                // Set key for entity
                builder.HasKey(p => p.SettingChoiceID);

                // Columns with default value

                builder
                    .Property(p => p.SettingID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.SettingChoiceID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.SettingChoiceName)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                builder
                    .Property(p => p.SettingChoiceValue)
                    .HasColumnType("varchar(100)")
                    .IsRequired();


                //builder.HasOne(x => x.Friend).WithOne().HasForeignKey<User>(a => a.UserGUID).OnDelete(DeleteBehavior.Restrict);
                //builder.HasOne(x => x.Friend).WithMany(b => b.)

                //builder.HasMany(x => x.Meals).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);

            }
        }

    }


#pragma warning restore CS1591
}
