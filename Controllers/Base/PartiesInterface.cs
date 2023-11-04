using DinderDLL.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public interface PartiesInterface
    {

        public abstract Task<IActionResult> GetPartiesAsync(Guid appInstallID, int pageSize = 10, int pageNumber = 1, Guid? cookGuid = null,
            int? partyID = null, string sessionName = null, string sessionMessage = null);


        public abstract Task<IActionResult> GetPartyAsync(Guid appInstallID, int PartyID, Guid UserGuid);

        public abstract Task<IActionResult> PostPartyAsync(Guid UserGuid, [FromBody] PostPartyRequest request);


        public abstract Task<IActionResult> PutPartyAsync(int PartyID, [FromBody] PutPartyRequest request);

        public abstract Task<IActionResult> DeletePartyAsync(Guid appInstallID, Guid userGuid, int PartyID);

        public abstract Task<IActionResult> GetPartySettingAsync(Guid appInstallID, int PartyID, int SettingID, Guid UserGuid);
        public abstract Task<IActionResult> GetPartySettingsAsync(Guid appInstallID, int PartyID, Guid UserGuid);
        public abstract Task<IActionResult> PutPartySettingsAsync(int PartyID, [FromBody] PutPartySettingsRequest request);

        public abstract Task<IActionResult> GetPartyInvitesAsync(Guid appInstallID, int PartyID, Guid UserGuid);
        public abstract Task<IActionResult> PutPartyInviteAsync(int PartyID, Guid userGuid, PutPartyInviteRequest request);

        public abstract Task<IActionResult> PostPartyInviteAsync(int PartyID, PostPartyInviteRequest request);

        public abstract Task<IActionResult> DeletePartyInviteAsync(Guid appInstallID, int PartyID, Guid UserGuid);



        public abstract Task<IActionResult> GetPartyChoicesAsync(Guid appInstallID, int PartyID, Guid UserGuid);

        public abstract Task<IActionResult> PutPartyChoiceAsync(int partyID, Guid userGuid, int mealID, PutPartyChoiceRequest request);
        public abstract Task<IActionResult> PostPartyChoiceAsync(int partyID, Guid userGuid, PostPartyChoiceRequest request);
        public abstract Task<IActionResult> DeletePartyChoiceAsync(Guid appInstallID, int PartyID, Guid UserGuid, int MealID);

        public abstract Task<IActionResult> GetPartyMealsAsync(Guid appInstallID, int partyID);

        public abstract Task<IActionResult> PostPartyMealAsync(PostPartyMealRequest request);
        public abstract Task<IActionResult> DeletePartyMealAsync(Guid appInstallID, int partyID, int MealID, Guid userGuid);

#pragma warning restore CS1591
    }
}
