using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinderBackEndv2.Models
{
#pragma warning disable CS1591

    public class PartyStatus
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

    }


#pragma warning restore CS1591
}
