using DinderDLL.Requests;
using System;

namespace DinderMVC.Models
{
#pragma warning disable CS1591

    public static class Extensions
    {
        public static User ToEntity(this PostUserRequest request)
            => new User
            {
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                CreateDate = DateTime.Now,
                LastActiveDate = DateTime.Now

            };
        public static UserMeal ToEntity(this PostUserMealRequest request, Guid hostGuid)
            => new UserMeal
            {

                MealName = request.mealName,
                MealDescription = request.mealDescription,
                GlobalLink = request.globalLink,
                MadeItBefore = request.madeItBefore,
                PrivateNotes = request.privateNotes,
                HostGuid = hostGuid


            };

        public static AppInstall ToEntity(this PostAppInstallRequest request)
            => new AppInstall
            {

                IPAddress = request.iPAddress,


            };

        public static Party ToEntity(this PostPartyRequest request, Guid hostGuid)
        => new Party
        {

            SessionName = request.sessionName,
            SessionMessage = request.sessionMessage,
            StatusID = 1,
            HostGuid = hostGuid

        };




        public static UserFriend ToEntity(this PostUserFriendRequest request, Guid userGUID)
            => new UserFriend
            {

                UserGUID = userGUID,
                FriendGUID = request.FriendGUID,
                FriendSinceDate = DateTime.Now,
                IsBlocked = false
            };

        public static PartyChoice ToEntity(this PostPartyChoiceRequest request, int partyID, Guid userGuid)
        => new PartyChoice
        {

            MealID = request.MealID,
            PartyID = partyID,
            SwipeChoice = request.SwipeChoice,
            UserGUID = userGuid
        };
        public static PartyInvite ToEntity(this PostPartyInviteRequest request, int partyID)
        => new PartyInvite
        {

            PartyID = partyID,
            UserGuid = request.UserGuid
        };

    }


#pragma warning restore CS1591
}
