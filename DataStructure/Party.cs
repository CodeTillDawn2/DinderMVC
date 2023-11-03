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
        public virtual List<PartySettingMatrix> Settings { get; set; }

        [NotMapped]
        public virtual List<PartyInvite> PartyInvites { get; set; }

        [NotMapped]
        public virtual User Cook { get; set; }

        [NotMapped]
        public virtual List<PartyMeal> Meals { get; set; }

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
            List<int> MealIDList = new List<int>();

            if (Meals != null)
            {
                foreach (PartyMeal pm in Meals)
                {
                    MealIDList.Add(pm.MealID);
                }

            }



            return new PartyDM(PartyID, CookGuid, SessionName, SessionMessage, StatusID, MealIDList);
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


                //builder.HasMany(x => x.Friends).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);
                builder.HasMany(x => x.Settings).WithOne(b => b.Party).HasForeignKey(b => b.PartyID).OnDelete(DeleteBehavior.Restrict);
                builder.HasMany(x => x.Meals).WithOne(b => b.Party).HasForeignKey(b => b.PartyID).OnDelete(DeleteBehavior.Restrict);

                // Set configuration for columns
                builder.Property(p => p.PartyID).HasColumnType("int").IsRequired().UseIdentityColumn();
                builder.Property(p => p.SessionName).HasColumnType("varchar(50)").IsRequired();
                builder.Property(p => p.SessionMessage).HasColumnType("varchar(255)").IsRequired();
                builder.Property(p => p.StatusID).HasColumnType("int").HasDefaultValueSql("((1))");


                //builder
                //    .Property(p => p.CreateDate)
                //    .HasColumnType("datetime")
                //    .IsRequired()
                //    .HasDefaultValueSql("(getdate())");

                //builder
                //    .Property(p => p.LastActiveDate)
                //    .HasColumnType("datetime")
                //    .IsRequired()
                //    .HasDefaultValueSql("(getdate())");

                //// Computed columns

                //builder
                //    .Property(p => p.Tags)
                //    .HasColumnType("nvarchar(max)")
                //    .HasComputedColumnSql("json_query([CustomFields],N'$.Tags')");

                //builder
                //    .Property(p => p.SearchDetails)
                //    .HasColumnType("nvarchar(max)")
                //    .IsRequired()
                //    .HasComputedColumnSql("concat([StockItemName],N' ',[MarketingComments])");

                // Columns with generated value on add or update

                //builder
                //    .Property(p => p.ValidFrom)
                //    .HasColumnType("datetime2")
                //    .IsRequired()
                //    .ValueGeneratedOnAddOrUpdate();

                //builder
                //    .Property(p => p.ValidTo)
                //    .HasColumnType("datetime2")
                //    .IsRequired()
                //    .ValueGeneratedOnAddOrUpdate();
            }
        }

    }


#pragma warning restore CS1591
}
