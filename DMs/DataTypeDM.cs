

using DinderDLL.DTOs;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class DataTypeDM : DataModel<DataTypeDTO>
    {

        public int DataTypeID { get; set; }
        public string DataTypeDescription { get; set; }


        private List<LinkCO> _links;
        public override List<LinkCO> Links { get { return _links; } set { _links = value; } }

        public DataTypeDM() { }
        public DataTypeDM(int dataTypeID, string dataTypeDescription)
        {

            DataTypeID = dataTypeID;
            DataTypeDescription = dataTypeDescription;
            AddLinks();

        }


        public override DataTypeDTO ReturnDTO()
        {
            return new DataTypeDTO(DataTypeID, DataTypeDescription, Links.ConvertAll(x => x.ReturnDTO()));
        }
    }
#pragma warning restore CS1591
}
