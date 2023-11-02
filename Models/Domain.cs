using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static DinderBackEndv2.Models.AppInstall;
using static DinderBackEndv2.Models.GlobalMeal;
using static DinderBackEndv2.Models.Meal;
using static DinderBackEndv2.Models.Party;
using static DinderBackEndv2.Models.PartyMeal;
using static DinderBackEndv2.Models.PartySettingMatrix;
using static DinderBackEndv2.Models.PartySettingType;
using static DinderBackEndv2.Models.PartySettingValue;
using static DinderBackEndv2.Models.SwipeChoice;
using static DinderBackEndv2.Models.User;
using static DinderBackEndv2.Models.UserFriend;

namespace DinderBackEndv2.Models
{
#pragma warning disable CS1591







    public class DinderContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public DinderContext(DbContextOptions<DinderContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations for entity

            AppInstallConfiguration AppInstallConfig = new AppInstallConfiguration();
            GlobalMealsConfiguration GlobalMealsConfiguation = new GlobalMealsConfiguration();
            MealsConfiguration MealsConfig = new MealsConfiguration();
            PartyConfiguration PartyConfig = new PartyConfiguration();
            PartyMealConfiguration PartyMealConfiguration = new PartyMealConfiguration();
            PartySettingMatrixConfiguration partySettingMatrix = new PartySettingMatrixConfiguration();
            PartySettingTypeConfiguration partySettingType = new PartySettingTypeConfiguration();
            PartySettingValueConfiguration partySettingValue = new PartySettingValueConfiguration();
            SwipeChoiceConfiguration SwipeChoiceConfiguration = new SwipeChoiceConfiguration();
            UsersConfiguration Userconfig = new UsersConfiguration();
            UserFriendConfiguration UserFriendConfiguration = new UserFriendConfiguration();
            //DataTypeConfiguration dataTypeConfiguration = new DataTypeConfiguration();


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



            base.OnModelCreating(modelBuilder);
        }


        public DbSet<AppInstall> AppInstalls { get; set; }
        public DbSet<GlobalMeal> GlobalMeals { get; set; }
        public DbSet<Meal> Meals { get; set; }

        public DbSet<Party> Parties { get; set; }
        public DbSet<PartyChoice> PartyChoices { get; set; }

        public DbSet<PartyInvite> PartyInvites { get; set; }

        public DbSet<PartyMeal> PartyMeals { get; set; }

        public DbSet<PartySettingMatrix> PartySettingMatrices { get; set; }

        public DbSet<PartySettingType> PartySettingTypes { get; set; }

        public DbSet<PartySettingValue> PartySettingValues { get; set; }

        public DbSet<SwipeChoice> SwipeChoices { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UserFriend> UserFriends { get; set; }



        //public DbSet<UserFriend> UserFriends { get; set; }




        public static string APIVersion = "v1";

        //public Microsoft.EntityFrameworkCore.DbSet<PartyChoices> PartyChoices { get; set; }
        //public Microsoft.EntityFrameworkCore.DbSet<Party> Parties { get; set; }
        //public Microsoft.EntityFrameworkCore.DbSet<Meal> Meals { get; set; }
        //public Microsoft.EntityFrameworkCore.DbSet<GlobalMeal> GlobalMeals { get; set; }

        //public Microsoft.EntityFrameworkCore.DbSet<AppInstalls> AppInstalls { get; set; }
    }
#pragma warning restore CS1591
}
