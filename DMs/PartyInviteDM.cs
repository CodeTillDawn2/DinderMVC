
using DinderDLL.DTOs;
using DinderDLL.Services;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartyInviteDM : DataModel<PartyInviteDTO>
    {

        public int PartyID { get; set; }
        public Guid UserGuid { get; set; }
        public DateTime? AcceptDate { get; set; }
        public Boolean? RSVP { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }



        public PartyInviteDM(int partyID, Guid userGuid, DateTime? acceptDate = null, bool? rSVP = null)
        {

            PartyID = partyID;
            UserGuid = userGuid;
            AcceptDate = acceptDate;
            RSVP = rSVP;
            AddLinks();
        }

        public override void AddLinks()
        {
            _links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_parent_party, LinkService.HREF_party(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_party_invite(PartyID.ToString(), UserGuid.ToString())));
            Links.Add(new LinkCO(LinkService.REL_update_self, LinkService.HREF_party_invite(PartyID.ToString(), UserGuid.ToString())));
            Links.Add(new LinkCO(LinkService.REL_delete_self, LinkService.HREF_party_invite(PartyID.ToString(), UserGuid.ToString())));

        }

        public override PartyInviteDTO ReturnDTO()
        {
            return new PartyInviteDTO(PartyID, UserGuid, Links.ConvertAll(x => x.ReturnDTO()), AcceptDate);
        }
    }
#pragma warning restore CS1591
}
