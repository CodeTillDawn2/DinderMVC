using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderMVC.DataStructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class AppInstall : DataStructure<AppInstallDM, AppInstallDTO>
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

        public AppInstallDM ReturnDM()
        {
            return new AppInstallDM(AppInstallGUID, InstallDate, IPAddress);
        }

        public AppInstallDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }
    }


#pragma warning restore CS1591
}
