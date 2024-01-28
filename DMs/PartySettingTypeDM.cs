

using DinderDLL.DTOs;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartySettingTypeDM : DataModel<PartySettingTypeDTO>
    {

        public int PartySettingID { get; set; }

        public string SettingName { get; set; }

        public int SettingValueDataType { get; set; }
        public int DefaultSettingChoice { get; set; }
        public string DefaultSettingEntry { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public PartySettingTypeDM(int partySettingID, string settingName, int settingValueDataType, int defaultSettingChoice, string defaultSettingEntry)
        {
            PartySettingID = partySettingID;
            SettingName = settingName;
            SettingValueDataType = settingValueDataType;
            DefaultSettingChoice = defaultSettingChoice;
            DefaultSettingEntry = defaultSettingEntry;
            AddLinks();
        }

        public override PartySettingTypeDTO ReturnDTO()
        {
            return new PartySettingTypeDTO(PartySettingID, SettingName, SettingValueDataType, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
