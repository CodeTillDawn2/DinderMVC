using DinderDLL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class Party
    {

        public int PartyID { get; set; }
        public Guid CookGuid { get; set; }
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
            CookGuid = partyModel.CookGuid;
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

            List<PartySettingsViewCO> settingsList = new List<PartySettingsViewCO>();

            if (PartyInvites != null)
            {
                foreach (PartySettingMatrix set in Settings)
                {
                    settingsList.Add(set.ReturnCO());
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

            return new PartyDM(PartyID, CookGuid, SessionName, SessionMessage, StatusID, mealList, InvitedGuidList, settingsList, partyChoices);
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
                    .Property(p => p.CookGuid)
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
