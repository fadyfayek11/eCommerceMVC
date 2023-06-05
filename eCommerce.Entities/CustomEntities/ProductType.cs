using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.CustomEntities
{
    public class ProductType : BaseEntity
    {
        public int ProductID { get; set; }
        public int? FeatureValueID { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
        public FeatureValue FeatureValue { get; set; }
        public int StockQuantity { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Discount { get; set; }
        public string Barcode { get; set; }

        public string SKU { get; set; }

    }
}
