using DinderDLL.DTOs;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartyInviteViewCO : DataModel<PartyInviteViewDTO>
    {

        public int PartyID { get; set; }
        public Guid UserGuid { get; set; }
        public DateTime? AcceptDate { get; set; }

        private string _displayName = "";
        public string DisplayName { get { return _displayName; } set { _displayName = value; } }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public PartyInviteViewCO()
        {

        }



        public PartyInviteViewCO(int partyID, Guid userGuid, DateTime? acceptDate = null)
        {

            PartyID = partyID;
            UserGuid = userGuid;
            AcceptDate = acceptDate;
            AddLinks();
        }

        public void AddLinks()
        {
            _links = new List<LinkCO>();


        }

        public override PartyInviteViewDTO ReturnDTO()
        {
            return new PartyInviteViewDTO(PartyID, UserGuid, DisplayName, Links.ConvertAll(x => x.ReturnDTO()), AcceptDate);
        }

        public PartyInviteDM ReturnDM()
        {
            return new PartyInviteDM(PartyID, UserGuid, AcceptDate);
        }
    }
#pragma warning restore CS1591
}
