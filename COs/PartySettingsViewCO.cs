using DinderDLL.DTOs;
using System;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class PartySettingsViewCO : DataModel<PartySettingsViewDTO>
    {


        public int PartyID { get; set; }

        public int SettingID { get; set; }

        public int SettingChoiceID { get; set; }
        public string DataTypeDescription { get; set; }
        public string SettingName { get; set; }
        public string SettingChoiceName { get; set; }

        public string SettingChoiceValue { get; set; }

        public String ChoiceEntry { get; set; }
        public int DataTypeID { get; set; }

        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public PartySettingsViewCO()
        {
            AddLinks();
        }


        public PartySettingsViewCO(int partyID, int settingID, int settingChoiceID, string dataTypeDescription, string settingName,
            string settingChoiceName, String choiceValue, int dataTypeID, string choiceEntry)
        {
            PartyID = partyID;
            SettingID = settingID;
            SettingChoiceID = settingChoiceID;
            SettingName = settingName;
            SettingChoiceValue = choiceValue;
            DataTypeID = dataTypeID;
            ChoiceEntry = choiceEntry;
            SettingChoiceName = settingChoiceName;
            DataTypeDescription = dataTypeDescription;
            AddLinks();

        }

        public override PartySettingsViewDTO ReturnDTO()
        {
            return new PartySettingsViewDTO(PartyID, SettingID, SettingChoiceID, DataTypeDescription, SettingName, SettingChoiceValue, ChoiceEntry, DataTypeID, Links.ConvertAll(x => x.ReturnDTO()));
        }
        public void AddLinks()
        {
            _links = new List<LinkCO>();
            //_links.Add(LinkService.Settings_Parent(PartyID));
            //_links.Add(LinkService.Settings_Setting(PartyID, SettingID));
            //_links.Add(LinkService.Settings_Self(PartyID, SettingID));
        }

    }
#pragma warning restore CS1591
}
