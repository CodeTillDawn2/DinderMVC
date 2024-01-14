using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DinderMVC.Services
{
    public static class APIServices
    {
        public static UserIdentity GetUserID(IEnumerable<Claim> claims)
        {
            UserIdentity id = new UserIdentity();
            foreach (Claim claim in claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.Name:
                        id.DisplayName = claim.Value;
                        break;

                    case ClaimTypes.Expiration:
                        if (DateTime.TryParse(claim.Value, out DateTime expirationDate))
                        {
                            id.ExpirationDate = expirationDate;
                        }
                        break;

                    case ClaimTypes.Thumbprint:

                        id.UserGuid = new Guid(claim.Value);
                        break;

                    case ClaimTypes.System:
                        id.AppInstallGuid = new Guid(claim.Value);
                        break;
                    case ClaimTypes.Hash:
                        id.ActiveToken = claim.Value;
                        break;

                    default:
                        // Handle any other claims or ignore them
                        break;

                }
            }

            return id;
        }


    }
}
