using DinderDLL.DTOs;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class AppInstallDM : DataModel<AppInstallDTO>
    {


        public Guid AppInstallGUID { get; set; }



        public DateTime? InstallDate { get; set; }

        public string IPAddress { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }


        public AppInstallDM(Guid appInstallGuid, DateTime installDate, string ipAddress)
        {
            AppInstallGUID = appInstallGuid;
            InstallDate = installDate;
            IPAddress = ipAddress;
            AddLinks();

        }

        public override AppInstallDTO ReturnDTO()
        {
            return new AppInstallDTO(AppInstallGUID, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
