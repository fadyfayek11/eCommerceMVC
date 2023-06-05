using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class FeaturesService
    {
        #region Define as Singleton
        private static FeaturesService _Instance;

        public static FeaturesService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new FeaturesService();
                }

                return (_Instance);
            }
        }

        private FeaturesService()
        {
        }
        #endregion

        public List<Feature> GetAllFeatures(int? pageNo = 1, int? recordSize = 0)
        {
            var context = DataContextHelper.GetNewContext();

            var features = context.Features.Include("FeatureValues")
                                    .Where(x => !x.IsDeleted && x.IsActive)
                                    .OrderBy(x => x.ID)
                                    .AsQueryable();
            foreach(var feature in features)
            {
                feature.FeatureValues = feature.FeatureValues.Where(x => x.IsActive && !x.IsDeleted).ToList();
            }
            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                features = features.Skip(skip)
                                   .Take(recordSize.Value);
            }

            return features.ToList();
        }

        public List<Feature> SearchFeatures(string searchTerm, int? pageNo, int recordSize, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var Features = context.Features
                                .Where(x => !x.IsDeleted)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Features = Features.Where(x => x.FeatureName.ToLower().Contains(searchTerm.ToLower()));
            }

            count = Features.Count();

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            return Features.OrderByDescending(x => x.FeatureName).Skip(skipCount).Take(recordSize).ToList();
        }

        public Feature GetFeatureByID(int ID)
        {

            var context = DataContextHelper.GetNewContext();

            return context.Features.FirstOrDefault(x => !x.IsDeleted && x.ID == ID);
        }
        public FeatureValue GetFeatureValueByID(int ID)
        {

            var context = DataContextHelper.GetNewContext();

            return context.FeatureValues.FirstOrDefault(x => !x.IsDeleted && x.ID == ID);
        }
        public Feature GetFeatureWithValuesByID(int ID)
        {

            var context = DataContextHelper.GetNewContext();

            return context.Features.Include("FeatureValues").FirstOrDefault(x => !x.IsDeleted && x.ID == ID);
        }
        public bool SaveFeature(Feature Feature)
        {
            var context = DataContextHelper.GetNewContext();

            context.Features.Add(Feature);

            return context.SaveChanges() > 0;
        }
        public bool SaveFeatureValue(FeatureValue featureValue)
        {
            var context = DataContextHelper.GetNewContext();

            context.FeatureValues.Add(featureValue);

            return context.SaveChanges() > 0;
        }

        public bool UpdateFeature(Feature Feature)
        {
                var context = DataContextHelper.GetNewContext();

                var existingFeature = context.Features.Find(Feature.ID);

                context.Entry(existingFeature).CurrentValues.SetValues(Feature);

                return context.SaveChanges() > 0;
        }
        public bool UpdateFeatureValue(FeatureValue featureValue)
        {
            var context = DataContextHelper.GetNewContext();

            var existingFeatureValue = context.FeatureValues.Find(featureValue.ID);

            context.Entry(existingFeatureValue).CurrentValues.SetValues(featureValue);

            return context.SaveChanges() > 0;
        }

        public bool DeleteFeature(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var Features = context.Features.Find(ID);

            Features.IsDeleted = true;

            context.Entry(Features).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }
    }
}
