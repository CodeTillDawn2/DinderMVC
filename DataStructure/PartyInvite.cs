using DinderDLL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class PartyInvite
    {
        [NotMapped]
        public virtual Party Party { get; set; }
        public int PartyID { get; set; }
        public Guid UserGuid { get; set; }
        public DateTime? AcceptDate { get; set; }
        public Boolean? RSVP { get; set; }


        public PartyInvite()
        {
        }

        public PartyInvite(int partyid)
        {
            PartyID = partyid;
        }

        public PartyInvite(int partyID, Guid userGuid, DateTime? acceptDate, bool? rSVP)
        {
            PartyID = partyID;
            UserGuid = userGuid;
            AcceptDate = acceptDate;
            RSVP = rSVP;
        }

        public PartyInviteDM ReturnDM()
        {
            return new PartyInviteDM(PartyID, AcceptDate, RSVP);
        }

        public class PartyInviteConfiguration : IEntityTypeConfiguration<PartyInvite>
        {
            public void Configure(EntityTypeBuilder<PartyInvite> builder)
            {


                // Set configuration for entity
                builder.ToTable("PartyInvites", "dbo");

                // Set key for entity
                builder.HasKey(p => new { p.PartyID, p.UserGuid });

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

                builder
                    .Property(p => p.RSVP)
                    .HasColumnType("bit");

            }
        }

    }


#pragma warning restore CS1591
}
