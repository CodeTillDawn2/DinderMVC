using System.Collections.Generic;

namespace DinderDLL.DataModels
{
    public abstract class DataModel<T>
    {

        public abstract T ReturnDTO();
        public abstract List<LinkCO> Links { get; set; }

        public virtual void AddLinks()
        {
            Links = new List<LinkCO>();
        }

    }
}
