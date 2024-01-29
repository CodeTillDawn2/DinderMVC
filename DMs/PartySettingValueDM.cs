

using DinderDLL.DTOs;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartySettingValueDM : DataModel<PartySettingValueDTO>
    {

        public int SettingID { get; set; }
        public int SettingChoiceID { get; set; }
        public string SettingChoiceName { get; set; }
        public string SettingChoiceValue { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }
        public PartySettingValueDM() { }
        public PartySettingValueDM(int settingID, int settingChoiceID, string settingChoiceName, string settingChoiceValue)
        {

            SettingID = settingID;
            SettingChoiceID = settingChoiceID;
            SettingChoiceName = settingChoiceName;
            SettingChoiceValue = settingChoiceValue;
            AddLinks();
        }

        public override PartySettingValueDTO ReturnDTO()
        {
            return new PartySettingValueDTO(SettingID, SettingChoiceID, SettingChoiceName, SettingChoiceValue, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
