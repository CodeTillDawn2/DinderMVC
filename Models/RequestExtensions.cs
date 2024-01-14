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
                UserName = request.userName,
                DisplayName = request.displayName,

            };
        public static UserMeal ToEntity(this PostUserMealRequest request)
            => new UserMeal
            {

                CookGuid = request.userGUID,
                MealName = request.mealName,
                MealDescription = request.mealDescription,
                GlobalLink = request.globalLink,
                MadeItBefore = request.madeItBefore,
                PrivateNotes = request.privateNotes,


            };

        public static AppInstall ToEntity(this PostAppInstallRequest request)
            => new AppInstall
            {

                IPAddress = request.iPAddress,


            };

        public static Party ToEntity(this PostPartyRequest request, Guid cookGuid)
        => new Party
        {

            SessionName = request.sessionName,
            SessionMessage = request.sessionMessage,
            StatusID = 1,
            CookGuid = cookGuid
  
        };

        public static PartyMeal ToEntity(this PostPartyMealRequest request)
            => new PartyMeal
            {

                PartyID = request.partyID,
                MealID = request.mealID


            };


        public static UserFriend ToEntity(this PostUserFriendRequest request, Guid userGUID)
            => new UserFriend
            {

                UserGUID = userGUID,
                FriendGUID = request.friendGUID,
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
