
using DinderDLL.DTOs;
using DinderDLL.Services;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class UserMealDM : DataModel<UserMealDTO>
    {


        public Guid HostGuid { get; set; }
        public int MealID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }
        public Guid? GlobalLink { get; set; }
        public bool MadeItBefore { get; set; }

        public string PrivateNotes { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }


        public UserMealDM(string href, Guid hostGuid, int mealID, string mealName, string mealDescription, bool madeItBefore, string privateNotes, Guid? globalLink)
        {
            HostGuid = hostGuid;
            MealID = mealID;
            MealName = mealName;
            MealDescription = mealDescription;
            MadeItBefore = madeItBefore;
            PrivateNotes = privateNotes;
            GlobalLink = globalLink;
            AddLinks();

        }
        public override void AddLinks()
        {
            Links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_parent_user, LinkService.HREF_user(HostGuid.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_parent_meal, LinkService.HREF_user_meal(MealID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_user_meal(MealID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_update_self, LinkService.HREF_user_meal(MealID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_delete_self, LinkService.HREF_user_meal(MealID.ToString())));
            if (GlobalLink != null)
            {
                Links.Add(new LinkCO(LinkService.REL_get_global_meal, LinkService.HREF_globalmeal(GlobalLink.ToString())));
            }

        }


        public override UserMealDTO ReturnDTO()
        {
            return new UserMealDTO(MealID, MealName, HostGuid, MealDescription, MadeItBefore, PrivateNotes, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
