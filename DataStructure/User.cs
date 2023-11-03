using DinderDLL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class User
    {
        public Guid UserGUID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastActiveDate { get; set; }

        [NotMapped]
        public virtual List<Meal> Meals { get; set; }

        [NotMapped]
        public virtual List<Party> Parties { get; set; }

        public User()
        {
        }

        public User(Guid userGUID)
        {
            UserGUID = userGUID;
        }

        public UserDM ReturnDM()
        {
            return new UserDM(UserGUID, UserName, DisplayName, CreateDate, LastActiveDate, Meals?.ConvertAll(x => x.ReturnDM()), Parties?.ConvertAll(x => x.ReturnDM()));
        }

        public class UsersConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {


                // Set configuration for entity
                builder.ToTable("Users", "dbo");

                // Set key for entity
                builder.HasKey(p => p.UserGUID);

                // Columns with default value

                builder
                    .Property(p => p.UserGUID)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired()
                    .HasDefaultValueSql("(newid())");



                builder.HasMany(x => x.Meals).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);
                builder.HasMany(x => x.Parties).WithOne(b => b.Cook).HasForeignKey(b => b.CookGuid).OnDelete(DeleteBehavior.Restrict);
                //builder.HasMany(x => x.Friends).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);


                // Set configuration for columns
                builder.Property(p => p.UserName).HasColumnType("varchar(100)").IsRequired();
                builder.Property(p => p.DisplayName).HasColumnType("varchar(50)").IsRequired();

                builder
                    .Property(p => p.CreateDate)
                    .HasColumnType("datetime")
                    .IsRequired()
                    .HasDefaultValueSql("(getdate())");

                builder
                    .Property(p => p.LastActiveDate)
                    .HasColumnType("datetime")
                    .IsRequired()
                    .HasDefaultValueSql("(getdate())");

            }
        }

    }


#pragma warning restore CS1591
}
