using DinderDLL.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public interface PartiesInterface
    {

        public abstract Task<IActionResult> GetPartiesAsync(int pageSize = 10, int pageNumber = 1, Guid? cookGuid = null,
             string sessionName = null, string sessionMessage = null);


        public abstract Task<IActionResult> GetPartyAsync(int PartyID);

        public abstract Task<IActionResult> PostPartyAsync([FromBody] PostPartyRequest request);


        public abstract Task<IActionResult> PutPartyAsync(int PartyID, [FromBody] PutPartyRequest request);

        public abstract Task<IActionResult> DeletePartyAsync(int PartyID);


        public abstract Task<IActionResult> GetPartySettingsAsync(int PartyID);
        public abstract Task<IActionResult> PutPartySettingAsync(int PartyID,
            int SettingID, int ChoiceID, string ChoiceEntry);

        public abstract Task<IActionResult> GetPartyInvitesAsync(int PartyID);
        public abstract Task<IActionResult> PutPartyInviteAsync(int PartyID, PutPartyInviteRequest request);

        public abstract Task<IActionResult> PostPartyInviteAsync(int PartyID, PostPartyInviteRequest request);

        public abstract Task<IActionResult> DeletePartyInviteAsync(int PartyID);



        public abstract Task<IActionResult> GetPartyChoicesAsync(int PartyID);

        public abstract Task<IActionResult> PutPartyChoiceAsync(int partyID, Guid userGuid, int mealID, PutPartyChoiceRequest request);
        public abstract Task<IActionResult> PostPartyChoiceAsync(int partyID, Guid userGuid, PostPartyChoiceRequest request);
        public abstract Task<IActionResult> DeletePartyChoiceAsync(int PartyID, Guid UserGuid, int MealID);

        public abstract Task<IActionResult> GetPartyMealsAsync(int partyID);

        public abstract Task<IActionResult> PostPartyMealAsync(PostPartyMealRequest request);
        public abstract Task<IActionResult> DeletePartyMealAsync(int partyID, int MealID);

#pragma warning restore CS1591
    }
}
