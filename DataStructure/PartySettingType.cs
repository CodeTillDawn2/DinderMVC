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

    public class PartySettingType : DataStructure<PartySettingTypeDM, PartySettingTypeDTO>
    {
        public int PartySettingID { get; set; }

        public string SettingName { get; set; }

        public int SettingValueDataType { get; set; }
        public int DefaultSettingChoice { get; set; }
        public String DefaultSettingEntry { get; set; }

        [NotMapped]
        public DataType DataType { get; set; }

        public PartySettingType()
        {
        }

        public PartySettingType(int partySettingID, string settingName, int settingValueDataType, Int32 defaultSettingChoice, String defaultSettingEntry)
        {
            PartySettingID = partySettingID;
            SettingName = settingName;
            SettingValueDataType = settingValueDataType;
            DefaultSettingChoice = defaultSettingChoice;
            DefaultSettingEntry = defaultSettingEntry;

        }

        public class PartySettingTypeConfiguration : IEntityTypeConfiguration<PartySettingType>
        {
            public void Configure(EntityTypeBuilder<PartySettingType> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartySettingTypes", "dbo");

                // Set key for entity
                builder.HasKey(p => p.PartySettingID);


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

                builder
                    .Property(p => p.DefaultSettingChoice)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.DefaultSettingEntry)
                    .HasColumnType("varchar(255)");


                builder.HasOne(x => x.DataType).WithOne().HasForeignKey<DataType>(a => a.DataTypeID).OnDelete(DeleteBehavior.Restrict);

            }
        }

        public PartySettingTypeDM ReturnDM()
        {
            return new PartySettingTypeDM(PartySettingID, SettingName, SettingValueDataType, DefaultSettingChoice, DefaultSettingEntry);
        }

        public PartySettingTypeDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }
    }


#pragma warning restore CS1591
}
