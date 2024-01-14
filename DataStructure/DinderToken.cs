using DinderDLL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text.Json.Serialization;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class DinderToken
    {
        public String BearerToken { get; set; }
        public Guid UserGuid { get; set; }
        public Guid AppInstallGuid { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string IPAddress { get; set; }

        public DinderToken()
        {
        }

        public DinderToken(String bearerToken)
        {
            BearerToken = bearerToken;
        }


        public class AppInstallConfiguration : IEntityTypeConfiguration<DinderToken>
        {
            public void Configure(EntityTypeBuilder<DinderToken> builder)
            {


                // Set configuration for entity
                builder.ToTable("Tokens", "dbo");

                // Set key for entity
                builder.HasKey(p => p.BearerToken);

                // Columns with default value
                builder
                    .Property(p => p.BearerToken)
                    .HasColumnType("varchar(512)")
                    .IsRequired();
                builder
                    .Property(p => p.UserGuid)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();
                builder
                    .Property(p => p.AppInstallGuid)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();
                builder
                    .Property(p => p.IssueDate)
                    .HasColumnType("datetime")
                    .IsRequired();
                builder
                    .Property(p => p.ExpirationDate)
                    .HasColumnType("datetime")
                    .IsRequired();


            }
        }

    }


#pragma warning restore CS1591
}
