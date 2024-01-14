using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static DinderMVC.Models.AppInstall;
using static DinderMVC.Models.GlobalMeal;
using static DinderMVC.Models.UserMeal;
using static DinderMVC.Models.Party;
using static DinderMVC.Models.PartyMeal;
using static DinderMVC.Models.PartySettingMatrix;
using static DinderMVC.Models.PartySettingType;
using static DinderMVC.Models.PartySettingValue;
using static DinderMVC.Models.SwipeChoice;
using static DinderMVC.Models.User;
using static DinderMVC.Models.UserFriend;
using System.Threading.Tasks;
using System;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DinderMVC.Models
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
            UserMealsConfiguration UserMealsConfig = new UserMealsConfiguration();
            PartyConfiguration PartyConfig = new PartyConfiguration();
            PartyMealConfiguration PartyMealConfig = new PartyMealConfiguration();
            PartySettingMatrixConfiguration partySettingMatrix = new PartySettingMatrixConfiguration();
            PartySettingTypeConfiguration partySettingType = new PartySettingTypeConfiguration();
            PartySettingValueConfiguration partySettingValue = new PartySettingValueConfiguration();
            SwipeChoiceConfiguration SwipeChoiceConfig = new SwipeChoiceConfiguration();
            UsersConfiguration Userconfig = new UsersConfiguration();
            UserFriendConfiguration UserFriendConfig = new UserFriendConfiguration();
            //DataTypeConfiguration dataTypeConfiguration = new DataTypeConfiguration();


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



            base.OnModelCreating(modelBuilder);
        }

 


        public DbSet<AppInstall> AppInstalls { get; set; }
        public DbSet<GlobalMeal> GlobalMeals { get; set; }
        public DbSet<UserMeal> UserMeals { get; set; }

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
        public DbSet<DinderToken> DinderTokens { get; set; }



        //public DbSet<UserFriend> UserFriends { get; set; }




        public static string APIVersion = "v1";


    }
#pragma warning restore CS1591
}
