using DinderDLL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class UserFriend
    {
        public Guid UserGUID { get; set; }

        public Guid FriendGUID { get; set; }

        public DateTime? FriendSinceDate { get; set; }

        public bool IsBlocked { get; set; }
        [NotMapped]
        public virtual User User { get; set; }
        [NotMapped]
        public virtual User Friend { get; set; }

        public UserFriend()
        {
        }

        public UserFriend(Guid UsergUID, Guid FriendgUID)
        {
            UserGUID = UsergUID;
            FriendGUID = FriendgUID;
        }

        public UserFriendDM ReturnDM()
        {
            return new UserFriendDM(UserGUID, FriendGUID, FriendSinceDate, IsBlocked);
        }

        public class UserFriendConfiguration : IEntityTypeConfiguration<UserFriend>
        {
            public void Configure(EntityTypeBuilder<UserFriend> builder)
            {


                // Set configuration for entity
                builder.ToTable("UserFriends", "dbo");

                // Set key for entity
                builder.HasKey(p => new { p.UserGUID, p.FriendGUID });

                // Columns with default value

                builder
                    .Property(p => p.UserGUID)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                builder
                    .Property(p => p.FriendGUID)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                builder
                    .Property(p => p.FriendSinceDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                builder
                    .Property(p => p.IsBlocked)
                    .HasColumnType("bit")
                    .IsRequired();
                builder.HasOne(x => x.Friend).WithMany().HasForeignKey(a => a.UserGUID).OnDelete(DeleteBehavior.Restrict);
                //builder.HasOne(x => x.Friend).WithOne().HasForeignKey<User>(a => a.UserGUID).OnDelete(DeleteBehavior.Restrict);
                //builder.HasOne(x => x.Friend).WithMany(b => b.)

                //builder.HasMany(x => x.Meals).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);

            }
        }

    }


#pragma warning restore CS1591
}
