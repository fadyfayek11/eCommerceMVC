using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.CustomEntities
{
    public class Feature:BaseEntity
    {
        public string FeatureName { get; set; }
        public Types Type { get; set; }
        public List<FeatureValue> FeatureValues { get; set; }

    }
}
