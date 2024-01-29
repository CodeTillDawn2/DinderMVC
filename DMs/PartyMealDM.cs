
using DinderDLL.DTOs;
using DinderDLL.Services;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartyMealDM : DataModel<PartyMealDTO>
    {

        public int PartyID { get; set; }

        public int MealID { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public PartyMealDM() { }
        public PartyMealDM(int partyID, int mealID)
        {
            PartyID = partyID;
            MealID = mealID;
            AddLinks();
        }


        public override void AddLinks()
        {
            _links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_parent_party, LinkService.HREF_party(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_parent_meal, LinkService.HREF_user_meal(MealID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_party_meal(PartyID.ToString(), MealID.ToString())));
        }

        public override PartyMealDTO ReturnDTO()
        {
            return new PartyMealDTO(PartyID, MealID, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
