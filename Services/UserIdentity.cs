using System;

namespace DinderMVC.Services
{
    public class UserIdentity
    {
        public Guid UserGuid { get; set; }
        public string DisplayName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid AppInstallGuid { get; set; }
        public string ActiveToken { get; set; }


    }
}
