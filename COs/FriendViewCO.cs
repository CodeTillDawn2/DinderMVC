
using DinderDLL.DTOs;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class FriendViewCO : DataModel<FriendViewDTO>
    {

        public Guid FriendGUID { get; set; }

        public DateTime FriendSinceDate { get; set; }

        public bool IsBlocked { get; set; }

        public string DisplayName { get; set; }

        private List<LinkCO> _links;

        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public FriendViewCO(Guid friendGuid, DateTime friendSinceDate, bool isBlocked, string displayName)
        {
            FriendGUID = friendGuid;
            FriendSinceDate = friendSinceDate;
            IsBlocked = isBlocked;
            DisplayName = displayName;
            AddLinks();
        }

        public void AddLinks()
        {
            _links = new List<LinkCO>();

        }

        public override FriendViewDTO ReturnDTO()
        {
            return new FriendViewDTO(FriendGUID, FriendSinceDate, IsBlocked, DisplayName, Links.ConvertAll(x => x.ReturnDTO()));
        }

    }
#pragma warning restore CS1591
}
