
using DinderDLL.DTOs;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class SwipeChoiceDM : DataModel<SwipeChoiceDTO>
    {

        public int SwipeChoiceID { get; set; }
        public string SwipeChoiceDescription { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public SwipeChoiceDM(int swipeChoiceID, string swipeChoiceDescription)
        {

            SwipeChoiceID = swipeChoiceID;
            SwipeChoiceDescription = swipeChoiceDescription;
            AddLinks();
        }


        public override SwipeChoiceDTO ReturnDTO()
        {
            return new SwipeChoiceDTO(SwipeChoiceID, SwipeChoiceDescription, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
