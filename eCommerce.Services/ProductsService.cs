using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using eCommerce.Integrations.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class ProductsService
    {
        #region Define as Singleton
        private static ProductsService _Instance;

        public static ProductsService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ProductsService();
                }

                return (_Instance);
            }
        }

        private ProductsService()
        {
        }
        #endregion

        public List<Product> GetAllProducts(int? pageNo = 1, int? recordSize = 0)
        {
            var context = DataContextHelper.GetNewContext();

            var products = context.Products
                                    .Include("ProductTypes")
                                    .Where(x => !x.IsDeleted && x.IsActive && !x.Category.IsDeleted)
                                    .OrderBy(x => x.ID)
                                    .AsQueryable();

            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                products = products.Skip(skip)
                                   .Take(recordSize.Value);
            }

            return products.ToList();
        }

        public List<Product> SearchFeaturedProducts(int? pageNo = 1, int? recordSize = 0, List<int> excludeProductIDs = null)
        {
            excludeProductIDs = excludeProductIDs ?? new List<int>();

            var context = DataContextHelper.GetNewContext();

            var products = context.Products
                                    .Where(x => !x.IsDeleted && x.IsActive && !x.Category.IsDeleted && x.isFeatured && !excludeProductIDs.Contains(x.ID))
                                    .OrderBy(x => x.ID)
                                    .AsQueryable();

            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                products = products.Skip(skip)
                                       .Take(recordSize.Value);
            }

            return products.ToList();
        }

        public List<Product> SearchProducts(List<int> categoryIDs, string searchTerm, decimal? from, decimal? to, string sortby, int? pageNo, int recordSize, bool activeOnly, out int count, int? stockCheckCount = null,bool isDicount = false)
        {
            var context = DataContextHelper.GetNewContext();

            var productTypes = context.ProductTypes
                                  .Include("Product.Category")
                                  .Include("FeatureValue")
                                  .Where(x => !x.Product.IsDeleted && (!activeOnly || x.Product.IsActive) && !x.Product.Category.IsDeleted)
                                  .AsQueryable();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                    productTypes = (IQueryable<ProductType>)context.ProductRecords
                  .Include("Product.ProductTypes")
                  //.Where(x => !x.IsDeleted && x.Name.ToLower().Contains(searchTerm.ToLower()))
                  .Where(x => !x.IsDeleted && !x.Product.IsDeleted && x.Name.ToLower().Contains(searchTerm.ToLower()))
                  .SelectMany(x => x.Product.ProductTypes)
                  .AsQueryable();
            }

            if (categoryIDs != null && categoryIDs.Count > 0)
            {
                productTypes = productTypes.Where(x => categoryIDs.Contains(x.Product.CategoryID));
            }
            if(isDicount == true)
            {
                productTypes = productTypes.Where(x => x.Discount > 0);
            }
            if (from.HasValue && from.Value > 0.0M)
            {
                productTypes = productTypes.Where(x => x.Price >= from.Value);
            }

            if (to.HasValue && to.Value > 0.0M)
            {
                productTypes = productTypes.Where(x => x.Price <= to.Value);
            }

            if(stockCheckCount.HasValue && stockCheckCount.Value > 0)
            {
                productTypes = productTypes.Where(x => x.StockQuantity <= stockCheckCount.Value);
            }

            if (!string.IsNullOrEmpty(sortby))
            {
                if (string.Equals(sortby, "price-high", StringComparison.OrdinalIgnoreCase))
                {
                    productTypes = productTypes.OrderByDescending(x => x.Price);
                }
                else
                {
                    productTypes = productTypes.OrderBy(x => x.Price);
                }
            }
            else //sortBy Product Date
            {
                productTypes = productTypes.OrderByDescending(x => x.ModifiedOn);
            }
            try
            {
                count = productTypes.Select(_ => _.Product).Distinct().Count();
            }
            catch (Exception ex)
            {
                throw;
            }
            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            var result = productTypes.Select(_ => _.Product).Distinct().Include("Category.CategoryRecords").Include("ProductPictures.Picture").ToList();
            return result.Skip(skipCount).Take(recordSize).ToList();
        }

        public List<ProductType> SearchProductTypes(int? productId)
        {
            var context = DataContextHelper.GetNewContext();

            var productTypes = context.ProductTypes
                                  .Include("Product.Category")
                                  .Include("FeatureValue")
                                  .Where(x => x.ProductID == productId && !x.IsDeleted)
                                  .AsQueryable();

            var result = productTypes.Include("Product.Category.CategoryRecords").Include("Product.ProductPictures.Picture").ToList();
            return result;
        }

        public List<ProductType> GetProductWithLessStockQuantity(List<int> categoryIDs, string searchTerm, decimal? from, decimal? to, string sortby, bool activeOnly, int stockCount, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var productTypes = context.ProductTypes
                                  .Where(x => !x.IsDeleted && (!activeOnly || x.IsActive) && !x.Product.Category.IsDeleted)
                                  .AsQueryable();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                productTypes = (IQueryable<ProductType>)context.ProductRecords
                                  .Include("Product.ProductTypes")
                                  .Where(x => !x.IsDeleted && x.Name.ToLower().Contains(searchTerm.ToLower()))
                                  .Select(x => x.Product)
                                  .Where(x => !x.IsDeleted && (!activeOnly || x.IsActive) && !x.Category.IsDeleted)
                                  .Select(x => x.ProductTypes)
                                  .AsQueryable();

            }
            if (categoryIDs != null && categoryIDs.Count > 0)
            {
                productTypes = productTypes.Where(x => categoryIDs.Contains(x.Product.CategoryID));
            }

            if (from.HasValue && from.Value > 0.0M)
            {
                productTypes = productTypes.Where(x => x.Price >= from.Value);
            }

            if (to.HasValue && to.Value > 0.0M)
            {
                productTypes = productTypes.Where(x => x.Price <= to.Value);
            }

            productTypes = productTypes.Where(x => x.StockQuantity <= stockCount);

            if (!string.IsNullOrEmpty(sortby))
            {
                if (string.Equals(sortby, "price-high", StringComparison.OrdinalIgnoreCase))
                {
                    productTypes = productTypes.OrderByDescending(x => x.Price);
                }
                else
                {
                    productTypes = productTypes.OrderBy(x => x.Price);
                }
            }
            else //sortBy Product Date
            {
                productTypes = productTypes.OrderByDescending(x => x.ModifiedOn);
            }

            count = productTypes.Count();

            try
            {
                return productTypes.Include("FeatureValue").Include("Product.Category.CategoryRecords").ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public Product GetProductByID(int ID, bool activeOnly = true)
        {
            var context = DataContextHelper.GetNewContext();

            var product = context.Products.Include("ProductTypes.FeatureValue").Include("Category.CategoryRecords").Include("ProductPictures.Picture").FirstOrDefault(x=>x.ID == ID);

            if(activeOnly)
            {
                return product != null && !product.IsDeleted && product.IsActive && !product.Category.IsDeleted ? product : null;
            }
            else return product != null && !product.IsDeleted && !product.Category.IsDeleted ? product : null;
        }
        public ProductType GetProducTypeByID(int ID, bool activeOnly = true)
        {
            var context = DataContextHelper.GetNewContext();

            var product = context.ProductTypes
                                .Include("FeatureValue.Feature")
                                .Include("Product")
                                .Include("Product.Category.CategoryRecords")
                                .Include("Product.ProductPictures.Picture")
                                .FirstOrDefault(x => x.ID == ID);

            if (activeOnly)
            {
                return product != null && !product.IsDeleted && product.IsActive && !product.Product.Category.IsDeleted ? product : null;
            }
            else return product != null && !product.IsDeleted && !product.Product.Category.IsDeleted ? product : null;
        }
        public List<ProductType> GetProductTypesByIDs(List<int> IDs)
        {
            var context = DataContextHelper.GetNewContext();
            var Products = context.ProductTypes.Where(_ => IDs.Contains(_.ID)).Include("Product").ToList();
            return IDs.Select(id => context.ProductTypes.Include("Product").Include("Product.Category").SingleOrDefault(_ => _.ID == id)).Where(x => !x.IsDeleted && x.IsActive && !x.Product.Category.IsDeleted).OrderBy(x => x.ID).ToList();
        }
        public List<Product> GetProductsByIDs(List<int> IDs)
        {
            var context = DataContextHelper.GetNewContext();

            return IDs.Select(id => context.Products.Find(id)).Where(x=>!x.IsDeleted && x.IsActive && !x.Category.IsDeleted).OrderBy(x=>x.ID).ToList();
        }

        public ProductRecord GetProductRecordByID(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var productRecord = context.ProductRecords.Find(ID);

            return productRecord != null && !productRecord.IsDeleted ? productRecord : null;
        }

        public decimal GetMaxProductPrice()
        {
            var context = DataContextHelper.GetNewContext();

            var products = context.Products.Where(x => !x.IsDeleted && x.IsActive && !x.Category.IsDeleted);

            return products.Count() > 0 ? products.Max(x => x.ProductTypes.Max(_ => _.Price)) : 0;
        }

        public bool SaveProduct(Product product)
        {
            var context = DataContextHelper.GetNewContext();

            context.Products.Add(product);

            return context.SaveChanges() > 0;
        }

        public bool SaveProductRecord(ProductRecord productRecord)
        {
            var context = DataContextHelper.GetNewContext();

            context.ProductRecords.Add(productRecord);

            return context.SaveChanges() > 0;
        }
        public bool SaveProductType(ProductType productType)
        {
            var context = DataContextHelper.GetNewContext();

            context.ProductTypes.Add(productType);

            return context.SaveChanges() > 0;
        }

        public bool UpdateProduct(Product product)
        {
            var context = DataContextHelper.GetNewContext();

            var existingProduct = context.Products.Find(product.ID);

            context.Entry(existingProduct).CurrentValues.SetValues(product);

            return context.SaveChanges() > 0;
        }
        public bool UpdateProductType(ProductType product)
        {
            var context = DataContextHelper.GetNewContext();

            var existingProduct = context.ProductTypes.Find(product.ID);

            context.Entry(existingProduct).CurrentValues.SetValues(product);

            return context.SaveChanges() > 0;
        }
        public bool UpdateProductPictures(int productID, List<ProductPicture> newPictures)
        {
            var context = DataContextHelper.GetNewContext();

            var oldPictures = context.ProductPictures.Where(p => p.ProductID == productID);

            context.ProductPictures.RemoveRange(oldPictures);

            context.ProductPictures.AddRange(newPictures);

            return context.SaveChanges() > 0;
        }

        public bool UpdateProductRecord(ProductRecord productRecord)
        {
            var context = DataContextHelper.GetNewContext();

            var existingRecord = context.ProductRecords.Find(productRecord.ID);

            context.Entry(existingRecord).CurrentValues.SetValues(productRecord);

            return context.SaveChanges() > 0;
        }

        public bool UpdateProductSpecifications(int productRecordID, List<ProductSpecification> newProductSpecification)
        {
            var context = DataContextHelper.GetNewContext();

            var oldProductSpecifications = context.ProductSpecifications.Where(p => p.ProductRecordID == productRecordID).ToList();

            context.ProductSpecifications.RemoveRange(oldProductSpecifications);

            context.ProductSpecifications.AddRange(newProductSpecification);

            return context.SaveChanges() > 0;
        }

        public bool DeleteProduct(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var product = context.Products.Find(ID);

            product.IsDeleted = true;

            context.Entry(product).State = EntityState.Modified;

            return context.SaveChanges() > 0;
        }
        public bool DeleteProductType(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var productType = context.ProductTypes.Find(ID);

            productType.IsDeleted = true;

            context.Entry(productType).State = EntityState.Modified;

            return context.SaveChanges() > 0;
        }

        public void UpdateProductQuantities(Order order, bool isReturn = false)
        {
            var context = DataContextHelper.GetNewContext();

            foreach (var orderItem in order.OrderItems)
            {
                var dbProduct = context.ProductTypes.Find(orderItem.ProductTypeID);

                dbProduct.StockQuantity = isReturn ? dbProduct.StockQuantity + orderItem.Quantity: dbProduct.StockQuantity - orderItem.Quantity;

                context.Entry(dbProduct).State = EntityState.Modified;
            }

            context.SaveChanges();
        }



        public bool SyncProducts()
        {
            var productsIntegration = Task.Run(async ()=> await new ProductIntegrationService().GetProducts()).Result;
            Hashtable alphabet = new Hashtable();
            var context = DataContextHelper.GetNewContext();

            if (productsIntegration != null)
            {
                productsIntegration.table.ForEach(r => alphabet.Add(r.id, r.amount));

                var ProductTypesDB = context.ProductTypes.Where(x => !x.IsDeleted & !string.IsNullOrEmpty(x.SKU)).ToList();
                foreach (var productType in ProductTypesDB)
                {
                    productType.StockQuantity = (int)alphabet[productType.SKU];

                    // productType.StockQuantity = productsIntegration.table.Find(r=>r.id== productType.SKU).amount;
                }
            }

            return context.SaveChanges() > 0;
        }
    }
}
