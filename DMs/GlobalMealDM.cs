
using DinderDLL.DTOs;
using DinderDLL.Services;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class GlobalMealDM : DataModel<GlobalMealDTO>
    {


        public Guid GlobalMealGUID { get; set; }
        public string MealName { get; set; }
        public string MealDescription { get; set; }
        public Guid MealCreator { get; set; }
        public DateTime CreateDate { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public GlobalMealDM() { }
        public GlobalMealDM(Guid globalMealGuid, string mealName, string mealDesc, Guid mealCreator, DateTime createDate)
        {
            GlobalMealGUID = globalMealGuid;
            MealName = mealName;
            MealDescription = mealDesc;
            MealCreator = mealCreator;
            CreateDate = createDate;
            AddLinks();
        }

        public override void AddLinks()
        {
            Links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_globalmeal(GlobalMealGUID.ToString())));
        }

        public override GlobalMealDTO ReturnDTO()
        {
            return new GlobalMealDTO(GlobalMealGUID, MealName, MealDescription, MealCreator, CreateDate, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
