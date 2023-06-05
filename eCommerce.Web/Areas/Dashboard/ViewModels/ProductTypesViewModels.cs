using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class ProductTypesListingViewModel : PageViewModel
    {
        public List<ProductType> ProductTypes { get; set; }
        public int ProductID { get; set; }
    }

    public class ProductTypeActionViewModel : PageViewModel
    {
        public int ProductTypeID { get; set; }
        public int ProductRecordID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int FeatureValueID { get; set; }
        public FeatureValue FeatureValue { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Cost { get; set; }
        public bool InActive { get; set; }
        public bool IsDeleted { get; set; }
        public int StockQuantity { get; set; }
        public string Barcode { get; set; }
        public string SKU{ get; set; }

        public IList<Feature> Features { get; set; }

    }
}