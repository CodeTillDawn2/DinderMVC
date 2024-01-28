using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderMVC.DataStructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class Party : DataStructure<PartyDM, PartyDTO>
    {

        public int PartyID { get; set; }
        public Guid HostGuid { get; set; }
        public string SessionName { get; set; }
        public string SessionMessage { get; set; }

        public int StatusID { get; set; }

        [NotMapped]
        public virtual List<PartySettingMatrix> Settings { get; set; } = new List<PartySettingMatrix>();

        [NotMapped]
        public virtual List<PartyInvite> PartyInvites { get; set; } = new List<PartyInvite>();

        [NotMapped]
        public virtual User Cook { get; set; }

        [NotMapped]
        public virtual List<PartyMeal> PartyMeals { get; set; } = new List<PartyMeal>();

        [NotMapped]
        public virtual List<PartyChoice> PartyChoices { get; set; } = new List<PartyChoice>();

        //[NotMapped]
        //public virtual List<PartyInvite> Invites { get; set; }

        public Party()
        {
        }

        public Party(int partyid)
        {
            PartyID = partyid;
        }

        public Party(PartyDM partyModel)
        {
            PartyID = partyModel.PartyID;
            HostGuid = partyModel.HostGuid;
            SessionName = partyModel.SessionName;
            SessionMessage = partyModel.SessionMessage;
            StatusID = partyModel.StatusID;
        }



        public PartyDM ReturnDM()
        {
            List<UserMealDM> mealList = new List<UserMealDM>();

            if (PartyMeals != null)
            {
                foreach (PartyMeal pm in PartyMeals)
                {
                    mealList.Add(pm.Meal.ReturnDM());
                }

            }
            List<PartyInviteDM> InvitedGuidList = new List<PartyInviteDM>();

            if (PartyInvites != null)
            {
                foreach (PartyInvite pi in PartyInvites)
                {
                    InvitedGuidList.Add(pi.ReturnDM());
                }

            }



            List<PartyChoiceDM> partyChoices = new List<PartyChoiceDM>();

            if (PartyInvites != null)
            {
                foreach (PartyChoice pc in PartyChoices)
                {
                    partyChoices.Add(pc.ReturnDM());
                }

            }

            return new PartyDM(PartyID, HostGuid, SessionName, SessionMessage, StatusID, mealList, InvitedGuidList, partyChoices);
        }

        public PartyDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }

        public class PartyConfiguration : IEntityTypeConfiguration<Party>
        {
            public void Configure(EntityTypeBuilder<Party> builder)
            {


                // Set configuration for entity
                builder.ToTable("Parties", "dbo");

                // Set key for entity
                builder.HasKey(p => p.PartyID);

                // Columns with default value

                builder
                    .Property(p => p.HostGuid)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();




                // Set configuration for columns
                builder.Property(p => p.PartyID).HasColumnType("int").IsRequired().UseIdentityColumn();
                builder.Property(p => p.SessionName).HasColumnType("varchar(50)").IsRequired();
                builder.Property(p => p.SessionMessage).HasColumnType("varchar(255)").IsRequired();
                builder.Property(p => p.StatusID).HasColumnType("int");

                builder.HasMany(x => x.Settings).WithOne().HasForeignKey(b => b.PartyID).OnDelete(DeleteBehavior.Cascade);
                builder.HasMany(x => x.PartyMeals).WithOne(b => b.Party).HasForeignKey(b => b.PartyID).OnDelete(DeleteBehavior.Cascade);
                builder.HasMany(x => x.PartyInvites).WithOne(b => b.Party).HasForeignKey(b => b.PartyID).OnDelete(DeleteBehavior.Cascade);
                builder.HasMany(x => x.PartyChoices).WithOne(b => b.Party).HasForeignKey(b => b.PartyID).OnDelete(DeleteBehavior.Cascade);
            }
        }

    }


#pragma warning restore CS1591
}
