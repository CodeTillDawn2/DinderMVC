

using DinderDLL.DTOs;
using DinderDLL.Services;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class UserDM : DataModel<UserDTO>
    {

        public Guid UserGUID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastActiveDate { get; set; }

        private List<PartyDM> _partyList = new List<PartyDM>();

        public List<PartyDM> PartyList
        {
            get { return _partyList; }
            set
            {
                _partyList = value;
            }
        }
        private List<UserMealDM> _mealList = new List<UserMealDM>();

        public List<UserMealDM> MealList
        {
            get { return _mealList; }
            set
            {
                _mealList = value;
            }
        }

        private List<FriendViewCO> _friendsList = new List<FriendViewCO>();

        public List<FriendViewCO> FriendsList
        {
            get { return _friendsList; }
            set
            {
                _friendsList = value;
            }
        }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }
        public UserDM() { }
        public UserDM(Guid userGuid, string userName, string displayName, DateTime? createDate, DateTime? lastActiveDate, List<UserMealDM> meals, List<PartyDM> parties)
        {
            UserGUID = userGuid;
            UserName = userName;
            DisplayName = displayName;
            CreateDate = createDate;
            LastActiveDate = lastActiveDate;
            MealList = meals;
            PartyList = parties;
            AddLinks();
        }

        public override void AddLinks()
        {
            _links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_user(UserGUID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_update_self, LinkService.HREF_user(UserGUID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_delete_self, LinkService.HREF_user(UserGUID.ToString())));
        }

        public override UserDTO ReturnDTO()
        {
            return new UserDTO(UserGUID, DisplayName, Links.ConvertAll(x => x.ReturnDTO()), MealList != null ? MealList.ConvertAll(x => x.ReturnDTO()) : null);
        }

    }




#pragma warning restore CS1591
}
