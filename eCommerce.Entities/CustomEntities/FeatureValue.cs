using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.CustomEntities
{
    public class FeatureValue:BaseEntity
    {
        public string Value { get; set; }
        public int FeatureID { get; set; }
        public virtual Feature Feature { get; set; }
    }
}
