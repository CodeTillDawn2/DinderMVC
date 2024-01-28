
using DinderDLL.DTOs;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartyStatusDM : DataModel<PartyStatusDTO>
    {

        public int PartyStatusID { get; set; }
        public string PartyStatusDescription { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public PartyStatusDM(int partyStatusID, string partyStatusDescription)
        {

            PartyStatusID = partyStatusID;
            PartyStatusDescription = partyStatusDescription;
            AddLinks();
        }


        public override PartyStatusDTO ReturnDTO()
        {
            return new PartyStatusDTO(PartyStatusID, PartyStatusDescription, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
