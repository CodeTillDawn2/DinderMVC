using DinderDLL.DataModels;
using DinderDLL.DTOs;

namespace DinderMVC.DataStructure.Base
{
#pragma warning disable CS1591
    public interface DataStructure<T, T2> where T : DataModel<T2> where T2 : DataTransferObject
    {
        public abstract T ReturnDM();

        public abstract T2 ReturnDTO();
    }
#pragma warning restore CS1591
}
