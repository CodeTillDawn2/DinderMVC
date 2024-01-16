using Dapper;
using DinderDLL.DataModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DinderMVC.Queries
{
#pragma warning disable CS1591
    public static class DapperQueries
    {

        public static string PaginationInsert = " OFFSET (@pageNumber - 1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY";



        /// <summary>
        /// Gets all users according to filters
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="DisplayName">Filter by display name</param>
        /// <returns></returns>
        public static async Task<List<UserDM>> GetUsersAsync(int pageSize = 10, int pageNumber = 1, string DisplayName = null)
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




        public static async Task<List<UserMealDM>> GetUserMealsAsync(Guid userGuid, int pageSize = 10, int pageNumber = 1)
        {



            //Build where clause

            string whereclause = "";
            whereclause += " userGuid = @userGuid";
            whereclause = " Where " + whereclause;


            //Build ordered by clause (required)
            string orderedbyclause = " userGuid";

            string sql = @"Select UserGUID, MealID, MealName, MealDescription, GlobalLink,MadeItBefore,PrivateNotes FROM dbo.UserMeals " +
                whereclause +
                " Order by " + orderedbyclause +
                PaginationInsert;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<UserMealDM> data = (await db.QueryAsync<UserMealDM>
                    (sql, new { @userGuid = userGuid, @pageNumber = pageNumber, @pageSize = pageSize }
                    )).ToList();

                //Add links after the fact
                foreach (UserMealDM dto in data)
                {
                    dto.AddLinks();
                }


                return data;
            };




            //// Get query from DbSet
            //var query = dbContext.Meals.AsNoTracking().Where(x => x.UserGUID == userGuid).Select(x => new MealDTO(x)).AsQueryable();

            //return query;
        }

        public static async Task<List<PartySettingsViewCO>> GetPartySettingsAsync(int partyID)
        {

            //Build where clause

            string whereclause = "";
            whereclause += " PartyID = @partyID";
            whereclause = " Where " + whereclause;

            string sql;

            sql = @"Select [PartyID],[SettingID],[SettingName],[SettingChoiceName] ,[ChoiceEntry],[SettingChoiceID],[SettingChoiceValue],[DataTypeID], [DataTypeDescription]
                    from dbo.PartySettingsView " +
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


        }

        public static async Task<List<PartyInviteViewCO>> GetPartyInvitesAsync(int partyID)
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

        public static async Task<List<PartyChoiceDM>> GetPartyChoicesAsync(int partyID, Guid userGuid)
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

        public static async Task<List<FriendViewCO>> GetUserFriendsAsync(Guid userGuid, int pageSize = 100, int pageNumber = 1)
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

                List<FriendViewCO> data = (await db.QueryAsync<FriendViewCO>
               (sql, new { @userGuid = userGuid, @pageNumber = pageNumber, @pageSize = pageSize }
               )).ToList();
                //Add links after the fact
                foreach (FriendViewCO dto in data)
                {
                    dto.AddLinks();
                }

                return data;





            };


        }

        public static async Task<Guid?> AuthenticateUser(string username, string password)
        {


            string sql = $"exec spAuthenticate '{username}','{password}'";

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                Guid userGuid = (await db.QueryAsync<Guid>(sql)).FirstOrDefault();
                if (userGuid != null)
                {
                    return userGuid;
                }

            };



            return null;
        }

        public static async Task<List<UserMealDM>> GetPartyMealsAsync(int partyID, int pageSize = 100, int pageNumber = 1)
        {

            //Build where clause

            string whereclause = "";
            whereclause += " dbo.PartyMeals.PartyID = @partyID";
            whereclause = " Where " + whereclause;


            //Build ordered by clause (required)
            string orderedbyclause = " dbo.UserMeals.MealID";

            string sql = @"Select dbo.UserMeals.CookGuid, dbo.UserMeals.MealID, dbo.UserMeals.MealName, dbo.UserMeals.MealDescription, dbo.UserMeals.GlobalLink, dbo.UserMeals.MadeItBefore, dbo.UserMeals.PrivateNotes
                 FROM dbo.Parties " +
                " INNER JOIN dbo.PartyMeals ON dbo.Parties.PartyID = dbo.PartyMeals.PartyID " +
                " INNER JOIN dbo.UserMeals ON dbo.PartyMeals.MealID = dbo.UserMeals.MealID" +
                whereclause +
                " Order by " + orderedbyclause +
                PaginationInsert;

            // Run query using Dapper
            using (IDbConnection db = new SqlConnection(Startup.Configuration["AppSettings:ConnectionString"]))
            {
                List<UserMealDM> data = (await db.QueryAsync<UserMealDM>
                    (sql, new { @partyID = partyID, @pageNumber = pageNumber, @pageSize = pageSize }
                    )).ToList();

                //Add links after the fact
                foreach (UserMealDM dto in data)
                {
                    dto.AddLinks();
                }


                return data;
            };


        }


        public static async Task<List<PartyDM>> GetUserPartiesAsync(Guid userGuid, int pageSize = 100, int pageNumber = 1)
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
    }
#pragma warning restore CS1591
}
