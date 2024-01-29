

using DinderDLL.DTOs;
using DinderDLL.Services;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartyChoiceDM : DataModel<PartyChoiceDTO>
    {

        public int PartyID { get; set; }
        public Guid UserGUID { get; set; }
        public int MealID { get; set; }
        public int SwipeChoice { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }


        public PartyChoiceDM() { }
        public PartyChoiceDM(int partyID, Guid userGuid, int mealID, int swipeChoice)
        {

            PartyID = partyID;
            UserGUID = userGuid;
            MealID = mealID;
            SwipeChoice = swipeChoice;
            AddLinks();
        }

        public override void AddLinks()
        {
            _links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_parent_party, LinkService.HREF_party(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_party_choice(PartyID.ToString(), UserGUID.ToString(), MealID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_update_self, LinkService.HREF_party_choice(PartyID.ToString(), UserGUID.ToString(), MealID.ToString())));

        }

        public override PartyChoiceDTO ReturnDTO()
        {
            return new PartyChoiceDTO(PartyID, UserGUID, MealID, SwipeChoice, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
