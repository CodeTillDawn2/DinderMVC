using DinderDLL.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class AppInstall
    {
        public Guid AppInstallGUID { get; set; }



        public DateTime InstallDate { get; set; }

        public string IPAddress { get; set; }

        public AppInstall()
        {
        }

        public AppInstall(Guid appInstallGUID)
        {
            AppInstallGUID = appInstallGUID;
        }

        public AppInstallDM ReturnDTO()
        {
            return new AppInstallDM(AppInstallGUID, InstallDate, IPAddress);
        }

        public class AppInstallConfiguration : IEntityTypeConfiguration<AppInstall>
        {
            public void Configure(EntityTypeBuilder<AppInstall> builder)
            {


                // Set configuration for entity
                builder.ToTable("AppInstalls", "dbo");

                // Set key for entity
                builder.HasKey(p => p.AppInstallGUID);

                // Columns with default value

                builder
                    .Property(p => p.AppInstallGUID)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                builder
                    .Property(p => p.InstallDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                // Set configuration for columns
                builder.Property(p => p.IPAddress).HasColumnType("varchar(255)").IsRequired();

            }
        }

    }


#pragma warning restore CS1591
}
