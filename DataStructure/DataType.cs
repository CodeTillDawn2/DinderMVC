using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class DataType
    {
        public int DataTypeID { get; set; }

        public string DataTypeDescription { get; set; }


        public DataType()
        {
        }

        public DataType(int dataTypeID, string dataTypeDescription)
        {
            DataTypeID = dataTypeID;
            DataTypeDescription = dataTypeDescription;
        }

        public class DataTypeConfiguration : IEntityTypeConfiguration<DataType>
        {
            public void Configure(EntityTypeBuilder<DataType> builder)
            {


                // Set configuration for entity
                builder.ToTable("DataTypes", "dbo");

                // Set key for entity
                builder.HasKey(p => p.DataTypeID);

                // Columns with default value

                builder
                    .Property(p => p.DataTypeID)
                    .HasColumnType("int")
                    .IsRequired();

                builder
                    .Property(p => p.DataTypeDescription)
                    .HasColumnType("varchar(50)")
                    .IsRequired();



                //builder.HasOne(x => x.Friend).WithOne().HasForeignKey<User>(a => a.UserGUID).OnDelete(DeleteBehavior.Restrict);
                //builder.HasOne(x => x.Friend).WithMany(b => b.)

                //builder.HasMany(x => x.Meals).WithOne(b => b.User).HasForeignKey(b => b.UserGUID).OnDelete(DeleteBehavior.Restrict);

            }
        }

    }


#pragma warning restore CS1591
}
