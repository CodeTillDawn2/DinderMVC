using DinderMVC.Models;
using Microsoft.AspNetCore.Authentication;

namespace DinderMVC.Authentication
{
    // IBearerAuthenticationOptions.cs
    public interface IBearerAuthenticationOptions
    {
        DinderContext DbContext { get; set; }
    }

    // BearerAuthenticationOptions.cs
    public class BearerAuthenticationOptions : AuthenticationSchemeOptions, IBearerAuthenticationOptions
    {
        public DinderContext DbContext { get; set; }
    }
}
