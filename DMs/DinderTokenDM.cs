using DinderDLL.DTOs;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591

    public class DinderTokenDM : DataModel<DinderTokenDTO>
    {
        public String BearerToken { get; set; }
        public Guid UserGuid { get; set; }
        public DateTime ExpirationDate { get; set; }


        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }


        public DinderTokenDM(String bearerToken, Guid userGuid, DateTime expirationDate)
        {
            BearerToken = bearerToken;
            ExpirationDate = expirationDate;
            UserGuid = userGuid;
            AddLinks();
        }

        public override DinderTokenDTO ReturnDTO()
        {
            return new DinderTokenDTO(BearerToken, UserGuid, ExpirationDate, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }


#pragma warning restore CS1591
}
