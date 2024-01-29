

using DinderDLL.DTOs;
using DinderDLL.Services;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartySettingMatrixDM : DataModel<PartySettingMatrixDTO>
    {

        public int PartyID { get; set; }
        public int SettingID { get; set; }
        public int ChoiceID { get; set; }
        public string ChoiceEntry { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public PartySettingMatrixDM() { }
        public PartySettingMatrixDM(int partyID, int settingID, int choiceID, string choiceEntry)
        {

            PartyID = partyID;
            SettingID = settingID;
            ChoiceID = choiceID;
            ChoiceEntry = choiceEntry;
            AddLinks();
        }

        public override void AddLinks()
        {
            _links = new List<LinkCO>();
            Links.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));
            Links.Add(new LinkCO(LinkService.REL_get_parent_party, LinkService.HREF_party(PartyID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_party_setting(PartyID.ToString(), SettingID.ToString())));
            Links.Add(new LinkCO(LinkService.REL_update_self, LinkService.HREF_party_setting(PartyID.ToString(), SettingID.ToString())));

        }

        public override PartySettingMatrixDTO ReturnDTO()
        {
            return new PartySettingMatrixDTO(PartyID, SettingID, ChoiceID, ChoiceEntry, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
