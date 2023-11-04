using Dapper;
using DinderMVC;
using DinderDLL.DataModels;
using DinderMVC.Models;
using Microsoft.Data.SqlClient;
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

        public static string PaginationInsert = " OFFSET (@pageNumber - 1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY";

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

        /// <summary>
        /// Gets all users according to filters
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="DisplayName">Filter by display name</param>
        /// <returns></returns>
        public static async Task<List<UserDM>> GetUsersAsync(this DinderContext dbContext, int pageSize = 10, int pageNumber = 1, string DisplayName = null)
        {

            //Build where clause
            string whereclause = "";
            if (DisplayName != null)
                whereclause += " DisplayName like '%' + @DisplayName + '%'";

            if (whereclause != "")
                whereclause = " Where " + whereclause;


            //Build ordered by clause (required)
            string orderedbyclause = " UserName";

            string sql = @"Select UserGUID, UserName, DisplayName, CreateDate, LastActiveDate FROM dbo.Users " +
                whereclause +
                " Order by " + orderedbyclause +
                PaginationInsert;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<UserDM> data = (await db.QueryAsync<UserDM>
                    (sql, new { @DisplayName = DisplayName, @pageNumber = pageNumber, @pageSize = pageSize }
                    )).ToList();

                //Add links after the fact
                foreach (UserDM dto in data)
                    dto.AddLinks();

                return data;
            };


        }

        //public static async Task<List<DataModel<T>>> GetDataItems<T>(this DinderContext dbContext, DapperQuery dapperQuery)
        //{
        //    // Run query using Dapper
        //    using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
        //    {
        //        IEnumerable<DataModel<T>> data = (await db.QueryAsync<DataModel<T>>
        //            (dapperQuery.sql));

        //        List<DataModel<T>> result = data.ToList();

        //        //Add links after the fact
        //        foreach (DataModel<T> dm in result)
        //        {
        //            dm.AddLinks();
        //        }


        //        return result;
        //    };
        //}



        public static async Task<List<MealDM>> GetUserMealsAsync(this DinderContext dbContext, Guid userGuid, int pageSize = 10, int pageNumber = 1)
        {



            //Build where clause

            string whereclause = "";
            whereclause += " userGuid = @userGuid";
            whereclause = " Where " + whereclause;


            //Build ordered by clause (required)
            string orderedbyclause = " userGuid";

            string sql = @"Select UserGUID, MealID, MealName, MealDescription, GlobalLink,MadeItBefore,PrivateNotes FROM dbo.Meals " +
                whereclause +
                " Order by " + orderedbyclause +
                PaginationInsert;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<MealDM> data = (await db.QueryAsync<MealDM>
                    (sql, new { @userGuid = userGuid, @pageNumber = pageNumber, @pageSize = pageSize }
                    )).ToList();

                //Add links after the fact
                foreach (MealDM dto in data)
                {
                    dto.AddLinks();
                }


                return data;
            };




            //// Get query from DbSet
            //var query = dbContext.Meals.AsNoTracking().Where(x => x.UserGUID == userGuid).Select(x => new MealDTO(x)).AsQueryable();

            //return query;
        }

        public static async Task<List<PartySettingsViewCO>> GetPartySettingsAsync(this DinderContext dbContext, int partyID)
        {

            //Build where clause

            string whereclause = "";
            whereclause += " PartyID = @partyID";
            whereclause = " Where " + whereclause;

            string sql = @"Select * from dbo.PartySettingsView " +
                whereclause;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<PartySettingsViewCO> data = (await db.QueryAsync<PartySettingsViewCO>
                    (sql, new { @partyID = partyID }
                    )).ToList();

                //Add links after the fact
                foreach (PartySettingsViewCO dto in data)
                {
                    dto.AddLinks();
                }


                return data;
            };



            // Get query from DbSet
            //var query = dbContext.UserFriends.AsNoTracking().Include(c => c.Friend).Where(x => x.UserGUID == userGuid).Select(x => new FriendDTO(x)).AsQueryable();

            //return query;
        }

        public static async Task<List<PartyInviteViewCO>> GetPartyInvitesAsync(this DinderContext dbContext, int partyID)
        {

            //Build where clause

            string whereclause = "";
            whereclause += " PartyID = @partyID";
            whereclause = " Where " + whereclause;

            string sql = @"SELECT * from dbo.PartyInviteView " +
                whereclause;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<PartyInviteViewCO> data = (await db.QueryAsync<PartyInviteViewCO>
                    (sql, new { @partyID = partyID }
                    )).ToList();

                //Add links after the fact
                foreach (PartyInviteViewCO dto in data)
                {
                    dto.AddLinks();
                }


                return data;
            };
        }

        public static async Task<List<PartyChoiceDM>> GetPartyChoicesAsync(this DinderContext dbContext, int partyID, Guid userGuid)
        {

            //Build where clause

            string whereclause = "";
            whereclause += " PartyID = @partyID and UserGuid = @userGuid";
            whereclause = " Where " + whereclause;

            string sql = @"SELECT * from dbo.PartyChoices " +
                whereclause;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<PartyChoiceDM> data = (await db.QueryAsync<PartyChoiceDM>
                    (sql, new { @partyID = partyID, @userGuid = userGuid }
                    )).ToList();

                //Add links after the fact
                foreach (PartyChoiceDM dto in data)
                {
                    dto.AddLinks();
                }


                return data;
            };


            // Get query from DbSet
            //var query = dbContext.UserFriends.AsNoTracking().Include(c => c.Friend).Where(x => x.UserGUID == userGuid).Select(x => new FriendDTO(x)).AsQueryable();

            //return query;
        }

        public static async Task<List<FriendViewCO>> GetUserFriendsAsync(this DinderContext dbContext, Guid userGuid, int pageSize = 100, int pageNumber = 1)
        {

            //Build where clause

            string whereclause = "";
            whereclause += " userGuid = @userGuid";
            whereclause = " Where " + whereclause;


            //Build ordered by clause (required)
            string orderedbyclause = " FriendGUID";

            string sql = @"Select * from dbo.FriendView " +
                whereclause +
                " Order by " + orderedbyclause +
                PaginationInsert;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                try
                {
                    List<FriendViewCO> data = (await db.QueryAsync<FriendViewCO>
                   (sql, new { @userGuid = userGuid, @pageNumber = pageNumber, @pageSize = pageSize }
                   )).ToList();
                    //Add links after the fact
                    foreach (FriendViewCO dto in data)
                    {
                        dto.AddLinks();
                    }

                    return data;

                }
                catch (Exception ex)
                {
                    ex = ex;
                    throw ex;
                }




            };


        }

        public static async Task<List<MealDM>> GetPartyMealsAsync(this DinderContext dbContext, int partyID, int pageSize = 100, int pageNumber = 1)
        {

            //Build where clause

            string whereclause = "";
            whereclause += " dbo.PartyMeals.PartyID = @partyID";
            whereclause = " Where " + whereclause;


            //Build ordered by clause (required)
            string orderedbyclause = " dbo.Meals.MealID";

            string sql = @"Select dbo.Meals.UserGUID, dbo.Meals.MealID, dbo.Meals.MealName, dbo.Meals.MealDescription, dbo.Meals.GlobalLink, dbo.Meals.MadeItBefore, dbo.Meals.PrivateNotes
                 FROM dbo.Parties " +
                " INNER JOIN dbo.PartyMeals ON dbo.Parties.PartyID = dbo.PartyMeals.PartyID " +
                " INNER JOIN dbo.Meals ON dbo.PartyMeals.MealID = dbo.Meals.MealID" +
                whereclause +
                " Order by " + orderedbyclause +
                PaginationInsert;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<MealDM> data = (await db.QueryAsync<MealDM>
                    (sql, new { @partyID = partyID, @pageNumber = pageNumber, @pageSize = pageSize }
                    )).ToList();

                //Add links after the fact
                foreach (MealDM dto in data)
                {
                    dto.AddLinks();
                }


                return data;
            };


        }


        public static async Task<List<PartyDM>> GetUserPartiesAsync(this DinderContext dbContext, Guid userGuid, int pageSize = 100, int pageNumber = 1)
        {

            //Build where clause

            string whereclause = "";
            whereclause += " CookGuid = @userGuid";
            whereclause = " Where " + whereclause;


            //Build ordered by clause (required)
            string orderedbyclause = " PartyID desc";

            string sql = @"Select * from dbo.Parties  " +
                whereclause +
                " Order by " + orderedbyclause +
                PaginationInsert;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<PartyDM> data = (await db.QueryAsync<PartyDM>
                    (sql, new { @userGuid = userGuid, @pageNumber = pageNumber, @pageSize = pageSize }
                    )).ToList();

                //Add links after the fact
                foreach (PartyDM dto in data)
                {
                    dto.AddLinks();
                }


                return data;
            };




            // Get query from DbSet
            //var query = dbContext.UserFriends.AsNoTracking().Include(c => c.Friend).Where(x => x.UserGUID == userGuid).Select(x => new FriendDTO(x)).AsQueryable();

            //return query;
        }
        public static async Task<UserDM> GetUsersByUsernameAsync(this DinderContext dbContext, User entity)
        {
            User user = (await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(item => item.UserName == entity.UserName));
            if (user != null) return user.ReturnDM();
            return null;
        }


        public static async Task<UserDM> GetDetailedUserByGuidAsync(this DinderContext dbContext, User entity)
        {
            UserDM userDM = (await dbContext.Users.Include(x => x.Meals).Include(x => x.Parties).AsSplitQuery().FirstOrDefaultAsync(item => item.UserGUID == entity.UserGUID)).ReturnDM();
            userDM.FriendsList = await dbContext.GetUserFriendsAsync(userDM.UserGUID);
            //userDM.PartyList = await dbContext.GetUserPartiesAsync(userDM.UserGUID);
            return userDM;
        }


        public static async Task<AppInstallDM> VerifyInstall(this DinderContext dbContext, AppInstall entity)
        {
            AppInstallDM appInstallDM = (await dbContext.AppInstalls.AsNoTracking().FirstOrDefaultAsync(item => item.AppInstallGUID == entity.AppInstallGUID)).ReturnDTO();
            return appInstallDM;
        }

        public static IQueryable<MealDM> GetUserMeals(this DinderContext dbContext, Guid UserGUID, int? MealID, string MealName, string MealDescription, Guid? GlobalLink, bool? MadeItBefore)
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

        public static IQueryable<PartyDM> GetParties(this DinderContext dbContext, Guid? Cookguid, int? PartyID, string SessionName, string SessionMessage)
        {
            // Get query from DbSet
            var query = dbContext.Parties.Include(b => b.Meals).AsQueryable();

            // Filter by: 'Cookguid'
            if (Cookguid.HasValue)
                query = query.Where(item => item.CookGuid == Cookguid);

            // Filter by: 'PartyID'
            if (PartyID.HasValue)
                query = query.Where(item => item.PartyID == PartyID);

            // Filter by: 'SessionName'
            if (SessionName != null)
                query = query.Where(item => item.SessionName.Contains(SessionName));

            // Filter by: 'SessionMessage'
            if (SessionMessage != null)
                query = query.Where(item => item.SessionMessage.Contains(SessionMessage));

            return query.Select(x => x.ReturnDM());
        }

        public static async Task<UserFriend> GetUserFriendAsync(this DinderContext dbContext, UserFriend entity)
        {
            return (dbContext.UserFriends.Where(x => x.UserGUID == entity.UserGUID && x.FriendGUID == entity.FriendGUID)).FirstOrDefault();
        }

        public static async Task<PartyDM> GetDetailedPartyByIDAsync(this DinderContext dbContext, Party entity)
        {

            List<PartyInviteViewCO> invites = await dbContext.GetPartyInvitesAsync(entity.PartyID);
            List<PartySettingsViewCO> Settings = await dbContext.GetPartySettingsAsync(entity.PartyID);
            PartyDM party = dbContext.Parties.Include(x => x.Meals).AsSplitQuery().Where(item => item.PartyID == entity.PartyID).FirstOrDefault().ReturnDM();
            party.InviteList = invites;
            party.SettingList = Settings;
            return party;

        }


        public static async Task<PartySettingsViewCO> GetPartySettingByIDsEditableAsync(this DinderContext dbContext, int PartyID, int SettingID)
        {
            return (await dbContext.GetPartySettingsAsync(PartyID)).Where(x => x.PartyID == PartyID && x.SettingID == SettingID).FirstOrDefault();
        }
        public static async Task<Party> GetPartyByIDEditableAsync(this DinderContext dbContext, Party entity)
        => dbContext.Parties.Where(item => item.PartyID == entity.PartyID).FirstOrDefault();
        public static async Task<PartyMeal> GetPartyMealEditableAsync(this DinderContext dbContext, int PartyID, int MealID)
          => dbContext.PartyMeals.Where(item => item.PartyID == PartyID && item.MealID == MealID).FirstOrDefault();

        public static async Task<GlobalMeal> GetGlobalMealEditableAsync(this DinderContext dbContext, Guid guid)
          => dbContext.GlobalMeals.Where(item => item.GlobalMealGUID == guid).FirstOrDefault();
        public static async Task<GlobalMeal> GetGlobalMealByName(this DinderContext dbContext, string MealName)
  => dbContext.GlobalMeals.Where(item => item.MealName == MealName).FirstOrDefault();

        public static async Task<PartyInvite> GetPartyInviteEditableAsync(this DinderContext dbContext, Party entity, User userentity)
            => dbContext.PartyInvites.Where(item => item.PartyID == entity.PartyID && item.UserGuid == userentity.UserGUID).FirstOrDefault();

        public static async Task<PartyChoice> GetPartyChoiceEditableAsync(this DinderContext dbContext, Party entity, User userentity, UserMeal meal)
         => dbContext.PartyChoices.Where(item => item.PartyID == entity.PartyID && item.UserGUID == userentity.UserGUID && item.MealID == meal.MealID && item.CookGUID == meal.CookGuid).FirstOrDefault();

        public static async Task<UserMeal> GetUserMealByIDEditableAsync(this DinderContext dbContext, UserMeal entity)
            => dbContext.UserMeals.Where(item => item.MealID == entity.MealID && item.CookGuid == entity.CookGuid).FirstOrDefault();


        public static async Task<UserMeal> GetUserMealByMealNameAsync(this DinderContext dbContext, UserMeal entity) //To find any duplicates
    => await dbContext.UserMeals.AsNoTracking().FirstOrDefaultAsync(item => item.MealName == entity.MealName && item.CookGuid == entity.CookGuid);

    }

    public static class IQueryableExtensions
    {
        public static IQueryable<TModel> Paging<TModel>(this IQueryable<TModel> query, int pageSize = 0, int pageNumber = 0) where TModel : class
            => pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) : query;
    }
#pragma warning restore CS1591
}
