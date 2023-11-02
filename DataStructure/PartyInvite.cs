using DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DinderBackEndv2.Models
{
#pragma warning disable CS1591

    public class PartyInvite
    {

        public int PartyID { get; set; }
        public Guid UserGuid { get; set; }
        public DateTime? AcceptDate { get; set; }


        public PartyInvite()
        {
        }

        public PartyInvite(int partyid)
        {
            PartyID = partyid;
        }

        public PartyInvite(int partyID, Guid userGuid, DateTime? acceptDate)
        {
            PartyID = partyID;
            UserGuid = userGuid;
            AcceptDate = acceptDate;
        }

        public PartyInviteDM ReturnDM()
        {
            return new PartyInviteDM(PartyID, UserGuid, AcceptDate);
        }

        public class PartyInviteConfiguration : IEntityTypeConfiguration<PartyInvite>
        {
            public void Configure(EntityTypeBuilder<PartyInvite> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartyInvites", "dbo");

                // Set key for entity
                builder.HasKey(p => p.PartyID);
                builder.HasKey(p => p.UserGuid);

                // Columns with default value

                builder
                    .Property(p => p.PartyID)
                    .HasColumnType("int")
                    .IsRequired();
                builder
                    .Property(p => p.UserGuid)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();


                // Set configuration for columns
                builder.Property(p => p.AcceptDate).HasColumnType("datetime");



            }
        }

    }


#pragma warning restore CS1591
}
