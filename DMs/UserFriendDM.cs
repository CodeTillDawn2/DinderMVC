
using DinderDLL.DTOs;
using DinderDLL.Services;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class UserFriendDM : DataModel<UserFriendDTO>
    {

        public Guid UserGUID { get; set; }
        public Guid FriendGUID { get; set; }

        public DateTime? FriendSinceDate { get; set; }
        public bool IsBlocked { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }


        public UserFriendDM() { }
        public UserFriendDM(Guid userGUID, Guid friendGUID, DateTime? friendSinceDate, bool isBlocked)
        {

            UserGUID = userGUID;
            FriendGUID = friendGUID;
            FriendSinceDate = friendSinceDate;
            IsBlocked = isBlocked;
            AddLinks();
        }

        public override void AddLinks()
        {
            _links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_parent_user, LinkService.HREF_user(UserGUID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_user_friend(UserGUID.ToString(), FriendGUID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_update_self, LinkService.HREF_user_friend(UserGUID.ToString(), FriendGUID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_delete_self, LinkService.HREF_user_friend(UserGUID.ToString(), FriendGUID.ToString())));

        }

        public override UserFriendDTO ReturnDTO()
        {
            return new UserFriendDTO(UserGUID, FriendGUID, FriendSinceDate, IsBlocked, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
