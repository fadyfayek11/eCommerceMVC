using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class FeaturesListingViewModel : PageViewModel
    {
        public List<Feature> Features { get; set; }
        public string SearchTerm { get; set; }
        
        public Pager Pager { get; set; }
    }
    
    public class FeatureActionViewModel : PageViewModel
    {
        public int ID { get; set; }
        public string FeatureName { get; set; }
        public int Type { get; set; }
        public IList<FeatureValue> FeatureValues { get; set; }
    }
}