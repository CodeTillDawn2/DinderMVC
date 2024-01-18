using DinderDLL.DataModels;
using DinderMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DinderMVC.Queries
{
#pragma warning disable CS1591
    public static class DbContextQueries
    {


        public static IQueryable<RootDM> GetRoots(this DinderContext dbContext)
        {
            // Get query from DbSet
            List<RootDM> Roots = new List<RootDM>();
            //Roots.Add(new Root("AppInstall", "AppInstall/"));
            Roots.Add(new RootDM("Meal", "/api/" + DinderContext.APIVersion + "/Meals/"));
            Roots.Add(new RootDM("User", "/api/" + DinderContext.APIVersion + "/Users/"));
            Roots.Add(new RootDM("Party", "/api/" + DinderContext.APIVersion + "/Parties/"));

            return Roots.AsQueryable<RootDM>();
        }

        public static async Task<bool> AppInstallRegistered(this DinderContext dbContext, Guid appInstallID)
        {
            AppInstallDM AI = dbContext.AppInstalls.Where(x => x.AppInstallGUID == appInstallID).Select(x => x.ReturnDTO()).FirstOrDefault();
            if (AI == null)
                return false;
            return true;
        }

        public static async Task<bool> UserInParty(this DinderContext dbContext, int PartyID, Guid UserGuid)
        {
            Party party = dbContext.Parties.Where(x => x.PartyID == PartyID).Include(x => x.PartyInvites).FirstOrDefault();
            if (party == null)
                return false;
            if (party.CookGuid != UserGuid && !party.PartyInvites.Select(x => x.UserGuid).ToList().Contains(UserGuid))
                return false;
            return true;
        }

        public static async Task<bool> UserIsHost(this DinderContext dbContext, int PartyID, Guid UserGuid)
        {
            Party party = dbContext.Parties.Where(x => x.PartyID == PartyID).Include(x => x.PartyInvites).FirstOrDefault();
            if (party == null)
                return false;
            if (party.CookGuid != UserGuid)
                return false;
            return true;
        }


        public static async Task<UserDM> GetUsersByUsernameAsync(this DinderContext dbContext, string UserName)
        {
            User user = (await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(item => item.UserName == UserName));
            if (user != null) return user.ReturnDM();
            return null;
        }





        public static async Task<AppInstallDM> VerifyInstall(this DinderContext dbContext, Guid AppInstallID)
        {
            AppInstallDM appInstallDM = (await dbContext.AppInstalls.AsNoTracking().FirstOrDefaultAsync(item => item.AppInstallGUID == AppInstallID)).ReturnDTO();
            return appInstallDM;
        }

        public static IQueryable<UserMealDM> GetUserMeals(this DinderContext dbContext, Guid UserGUID, int? MealID, string MealName, string MealDescription, Guid? GlobalLink, bool? MadeItBefore)
        {
            // Get query from DbSet
            var query = dbContext.UserMeals.AsNoTracking().AsQueryable();

            // Filter by: 'UserGUID'
            query = query.Where(item => item.CookGuid == UserGUID);

            // Filter by: 'MealID'
            if (MealID.HasValue)
                query = query.Where(item => item.MealID == MealID);

            // Filter by: 'mealName'
            if (MealName != null)
                query = query.Where(item => item.MealName.Contains(MealName));

            // Filter by: 'mealDescription'
            if (MealDescription != null)
                query = query.Where(item => item.MealDescription.Contains(MealDescription));

            // Filter by: 'globalLink'
            if (GlobalLink.HasValue)
                query = query.Where(item => item.GlobalLink == GlobalLink);

            // Filter by: 'madeItBefore'
            if (MadeItBefore.HasValue)
                query = query.Where(item => item.MadeItBefore == MadeItBefore);

            return query.Select(x => x.ReturnDM());
        }

        public static IQueryable<GlobalMealDM> GetGlobalMeals(this DinderContext dbContext, string MealName, string MealDescription)
        {
            // Get query from DbSet
            var query = dbContext.GlobalMeals.AsNoTracking().AsQueryable();

            // Filter by: 'mealName'
            if (MealName != null)
                query = query.Where(item => item.MealName.Contains(MealName));

            // Filter by: 'mealDescription'
            if (MealDescription != null)
                query = query.Where(item => item.MealDescription.Contains(MealDescription));

            return query.Select(x => x.ReturnDM());
        }

        public static IQueryable<PartyDM> GetParties(this DinderContext dbContext, bool IsDetailed, Guid UserGuid, Guid? Cookguid, string SessionName, string SessionMessage)
        {
            // Get query from DbSet
            IQueryable<Party> query;

            if (IsDetailed)
            {
                query = DetailedPartyQuery(dbContext.Parties)
                .AsQueryable();
            }
            else
            {
                query = NotDetailedPartyQuery(dbContext.Parties)
                .AsQueryable();
            }

            // Filter by: 'Cookguid'
            if (Cookguid.HasValue)
                query = query.Where(item => item.CookGuid == Cookguid);

            List<int> combinedPartyIds = dbContext.PartyInvites
                .Where(x => x.UserGuid == UserGuid)
                .Select(x => x.PartyID)
                .Union(dbContext.Parties
                    .Where(x => x.CookGuid == UserGuid)
                    .Select(x => x.PartyID))
                .ToList();

            // Ensure that any record returned in the query has a party ID that matches the members of the invites list
            query = query.Where(item => combinedPartyIds.Contains(item.PartyID));

            // Filter by: 'SessionName'
            if (SessionName != null)
                query = query.Where(item => item.SessionName.Contains(SessionName));

            // Filter by: 'SessionMessage'
            if (SessionMessage != null)
                query = query.Where(item => item.SessionMessage.Contains(SessionMessage));

            return query.Select(x => x.ReturnDM());
        }

        public static async Task<UserFriend> GetUserFriendAsync(this DinderContext dbContext, Guid UserGuid, Guid FriendGuid)
        {
            return (dbContext.UserFriends.Where(x => x.UserGUID == UserGuid && x.FriendGUID == FriendGuid)).FirstOrDefault();
        }

        public static async Task<PartyDM> GetDetailedPartyByIDAsync(this DinderContext dbContext, int PartyID, bool IsDetailed)
        {

            List<PartyInviteViewCO> invites = await DapperQueries.GetPartyInvitesAsync(PartyID);
            List<PartySettingsViewCO> Settings = await DapperQueries.GetPartySettingsAsync(PartyID);


            PartyDM party;
            if (IsDetailed)
            {
                party = DetailedPartyQuery(dbContext.Parties).Where(item => item.PartyID == PartyID).FirstOrDefault().ReturnDM();
            }
            else
            {
                party = NotDetailedPartyQuery(dbContext.Parties).Where(item => item.PartyID == PartyID).FirstOrDefault().ReturnDM();
            }

            party.InviteList = invites.ConvertAll(x => x.ReturnDM());
            party.SettingList = Settings;
            return party;

        }

        // Define a delegate type for your lambda expression
        public delegate IQueryable<Party> DetailedPartyDelegate(IQueryable<Party> query);

        // Create a method that returns the lambda expression
        public static DetailedPartyDelegate DetailedPartyQuery = query =>
            query.Include(b => b.Meals).ThenInclude(b => b.Meal)
                .Include(b => b.PartyInvites)
                .Include(b => b.Settings).ThenInclude(c => c.Setting).ThenInclude(d => d.DataType)
                .Include(b => b.Settings).ThenInclude(c => c.Choice)
                .Include(b => b.PartyChoices);
        public static DetailedPartyDelegate NotDetailedPartyQuery = query =>
            query.Include(b => b.Meals).ThenInclude(b => b.Meal);



        public static async Task<PartySettingsViewCO> GetPartySettingByIDsEditableAsync(this DinderContext dbContext, int PartyID, int SettingID)
        {
            return (await DapperQueries.GetPartySettingsAsync(PartyID)).Where(x => x.PartyID == PartyID && x.SettingID == SettingID).FirstOrDefault();
        }
        public static async Task<Party> GetPartyByIDEditableAsync(this DinderContext dbContext, int PartyID, bool IsDetailed = false)
        {
            Party returnParty;

            if (IsDetailed)
            {
                returnParty = DetailedPartyQuery(dbContext.Parties).Where(item => item.PartyID == PartyID).FirstOrDefault();
            }
            else
            {
                returnParty = NotDetailedPartyQuery(dbContext.Parties).Where(item => item.PartyID == PartyID).FirstOrDefault();
            }

            return returnParty;
            
        }
        public static async Task<PartyMeal> GetPartyMealEditableAsync(this DinderContext dbContext, int PartyID, int MealID)
          => dbContext.PartyMeals.Where(item => item.PartyID == PartyID && item.MealID == MealID).FirstOrDefault();

        public static async Task<GlobalMeal> GetGlobalMealEditableAsync(this DinderContext dbContext, Guid guid)
          => dbContext.GlobalMeals.Where(item => item.GlobalMealGUID == guid).FirstOrDefault();
        public static async Task<GlobalMeal> GetGlobalMealByName(this DinderContext dbContext, string MealName)
  => dbContext.GlobalMeals.Where(item => item.MealName == MealName).FirstOrDefault();

        public static async Task<PartyInvite> GetPartyInviteEditableAsync(this DinderContext dbContext, int partyID, Guid userGuid)
            => dbContext.PartyInvites.Where(item => item.PartyID == partyID && item.UserGuid == userGuid).FirstOrDefault();

        public static async Task<PartyChoice> GetPartyChoiceEditableAsync(this DinderContext dbContext, int PartyID, Guid userGuid, int MealID)
         => dbContext.PartyChoices.Where(item => item.PartyID == PartyID && item.UserGUID == userGuid && item.MealID == MealID).FirstOrDefault();

        public static async Task<UserMeal> GetUserMealByIDEditableAsync(this DinderContext dbContext, Guid CookGuid, int MealID)
            => dbContext.UserMeals.Where(item => item.MealID == MealID && item.CookGuid == CookGuid).FirstOrDefault();


        public static async Task<UserMeal> GetUserMealByMealNameAsync(this DinderContext dbContext, Guid CookGuid, string MealName) //To find any duplicates
    => await dbContext.UserMeals.AsNoTracking().FirstOrDefaultAsync(item => item.MealName == MealName && item.CookGuid == CookGuid);

    }

    public static class IQueryableExtensions
    {
        public static IQueryable<TModel> Paging<TModel>(this IQueryable<TModel> query, int pageSize = 0, int pageNumber = 0) where TModel : class
            => pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) : query;
    }
#pragma warning restore CS1591
}
