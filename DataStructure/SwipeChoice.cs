using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderMVC.DataStructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public class SwipeChoice : DataStructure<SwipeChoiceDM, SwipeChoiceDTO>
    {
        public int SwipeChoiceID { get; set; }

        public string SwipeChoiceDescription { get; set; }

        public SwipeChoice()
        {
        }

        public SwipeChoice(int swipeChoiceID, string swipeChoiceDescription)
        {
            SwipeChoiceID = swipeChoiceID;
            SwipeChoiceDescription = swipeChoiceDescription;
        }

        public class SwipeChoiceConfiguration : IEntityTypeConfiguration<SwipeChoice>
        {
            public void Configure(EntityTypeBuilder<SwipeChoice> builder)
            {


                // Set configuration for entity
                builder.ToTable("SwipeChoices", "dbo");

                // Set key for entity
                builder.HasKey(p => p.SwipeChoiceID);

                // Columns with default value

                builder
                    .Property(p => p.SwipeChoiceID)
                    .HasColumnType("int")
                    .IsRequired();



                builder
                    .Property(p => p.SwipeChoiceDescription)
                    .HasColumnType("varchar(50)")
                    .IsRequired();



            }
        }

        public SwipeChoiceDM ReturnDM()
        {
            return new SwipeChoiceDM(SwipeChoiceID, SwipeChoiceDescription);
        }

        public SwipeChoiceDTO ReturnDTO()
        {
            return ReturnDM().ReturnDTO();
        }
    }


#pragma warning restore CS1591
}
