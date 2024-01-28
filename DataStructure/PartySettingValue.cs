using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderMVC.DataStructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class PartySettingValue : DataStructure<PartySettingValueDM, PartySettingValueDTO>
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
                builder.HasKey(p => new { p.SettingID, p.SettingChoiceID });

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



            }
        }

        public PartySettingValueDM ReturnDM()
        {
            return new PartySettingValueDM(SettingID, SettingChoiceID, SettingChoiceName, SettingChoiceValue);
        }

        public PartySettingValueDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }
    }


#pragma warning restore CS1591
}
