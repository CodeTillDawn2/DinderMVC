
using DinderDLL.DTOs;
using DinderDLL.Services;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartyDM : DataModel<PartyDTO>
    {

        public int PartyID { get; set; }
        public Guid HostGuid { get; set; }
        public string SessionName { get; set; }
        public string SessionMessage { get; set; }

        public int StatusID { get; set; }

        private List<UserMealDM> _mealList;
        public List<UserMealDM> MealList { get { return _mealList; } set { _mealList = value; } }

        private List<PartyInviteDM> _inviteList;
        public List<PartyInviteDM> InviteList { get { return _inviteList; } set { _inviteList = value; } }

        private List<PartyChoiceDM> _partyChoices;
        public List<PartyChoiceDM> PartyChoices { get { return _partyChoices; } set { _partyChoices = value; } }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public PartyDM() { }
        public PartyDM(int partyID, Guid hostGuid, string sessionName, string sessionMessage, int statusID, List<UserMealDM> mealList,
            List<PartyInviteDM> partyInvites, List<PartyChoiceDM> partyChoices)
        {
            PartyID = partyID;
            HostGuid = hostGuid;
            SessionName = sessionName;
            SessionMessage = sessionMessage;
            StatusID = statusID;
            MealList = mealList;
            InviteList = partyInvites;
            PartyChoices = partyChoices;
            AddLinks();
        }


        public override void AddLinks()
        {
            _links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_party(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_delete_self, LinkService.HREF_party(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_update_self, LinkService.HREF_party(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_party_meals, LinkService.HREF_party_meals(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_create_user_meal, LinkService.HREF_party_meals(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_party_invites, LinkService.HREF_party_invites(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_create_party_invite, LinkService.HREF_party_invites(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_party_choices, LinkService.HREF_party_choices(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_create_party_choice, LinkService.HREF_party_choices(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_party_settings, LinkService.HREF_party_settings(PartyID.ToString())));
        }

        public override PartyDTO ReturnDTO()
        {
            return new PartyDTO(PartyID, HostGuid, SessionName, SessionMessage, _links.ConvertAll(x => x.ReturnDTO()),
                _inviteList.ConvertAll(x => x.ReturnDTO()), _partyChoices.ConvertAll(x => x.ReturnDTO()), MealList.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
