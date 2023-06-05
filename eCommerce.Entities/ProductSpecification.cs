using eCommerce.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class ProductSpecification : BaseEntity
    {
        public int ProductRecordID { get; set; }
        public virtual ProductRecord ProductRecord { get; set; }
        public int FeatureValueID { get; set; }
        public virtual FeatureValue FeatureValue { get; set; }
    }

    public enum ProductSpecifications
    {
        Brand = 1,
        Model = 2,
        Material = 3,
        Color = 4,
        Weight = 5,
        Shape = 6,
        Size = 7,
        Dimensions = 8,
        Height = 9,
        Length = 10,
    }
}
