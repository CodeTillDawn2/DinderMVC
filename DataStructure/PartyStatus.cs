using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderMVC.DataStructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class PartyStatus : DataStructure<PartyStatusDM, PartyStatusDTO>
    {

        public int PartyStatusID { get; set; }

        public string PartyStatusDescription { get; set; }


        public PartyStatus()
        {
        }

        public PartyStatus(int partyStatusID, string partyStatusDescription)
        {
            PartyStatusID = partyStatusID;
            PartyStatusDescription = partyStatusDescription;
        }

        public class PartyStatusConfiguration : IEntityTypeConfiguration<PartyStatus>
        {
            public void Configure(EntityTypeBuilder<PartyStatus> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartyStatus", "dbo");

                // Columns with default value

                builder
                    .Property(p => p.PartyStatusID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.PartyStatusDescription)
                    .HasColumnType("varchar(50)")
                    .IsRequired();


            }
        }

        public PartyStatusDM ReturnDM()
        {
            return new PartyStatusDM(PartyStatusID, PartyStatusDescription);
        }

        public PartyStatusDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }
    }


#pragma warning restore CS1591
}
