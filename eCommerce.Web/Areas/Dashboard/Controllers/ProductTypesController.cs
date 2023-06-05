using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.Shared.Enums;
using eCommerce.Entities.CustomEntities;
using eCommerce.Shared.Extensions;


namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class ProductTypesController : DashboardBaseController
    {
        public ActionResult Index(int? productId)
        {
            ProductTypesListingViewModel model = new ProductTypesListingViewModel
            {
                ProductTypes = ProductsService.Instance.SearchProductTypes(productId),
                ProductID = productId.GetValueOrDefault(),
            };
            return View(model);
        }


        [HttpGet]
        public ActionResult Action(int? ID,int? productID)
        {


            ProductTypeActionViewModel model = new ProductTypeActionViewModel();

            if (ID.HasValue)
            {
                var productType = ProductsService.Instance.GetProducTypeByID(ID.Value, activeOnly: false);

                if (productType == null) return HttpNotFound();

                model.ProductID = productType.ProductID;
                model.ProductTypeID = productType.ID;
                model.Price = productType.Price;
                model.Discount = productType.Discount;
                model.Cost = productType.Cost;
                model.SKU = productType.SKU;
                model.StockQuantity = productType.StockQuantity;
                model.Barcode = productType.Barcode;
                model.InActive = !productType.IsActive;
                model.Product = productType.Product;
                model.FeatureValue = productType.FeatureValue;
                model.FeatureValueID = productType.FeatureValueID.GetValueOrDefault();
            }
            else
            {
                model.Product = ProductsService.Instance.GetProductByID(productID.GetValueOrDefault());
                model.ProductID = productID.GetValueOrDefault();
            }

            model.Features = FeaturesService.Instance.GetAllFeatures();
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Action(FormCollection formCollection)
        {
            JsonResult json = new JsonResult();

            try
            {
                ProductTypeActionViewModel model = GetProductActionViewModelFromForm(formCollection);

                if (model.ProductTypeID > 0)
                {
                    var product = ProductsService.Instance.GetProducTypeByID(model.ProductTypeID, activeOnly: false);

                    if (product == null)
                    {
                        throw new Exception("Dashboard.Products.Action.Validation.ProductTypeNotFound".LocalizedString());
                    }

                    product.ID = model.ProductTypeID;
                    product.ProductID = model.ProductID;
                    product.Price = model.Price;

                    product.Discount = model.Discount;
                    product.Cost = model.Cost;
                    product.SKU=model.SKU;
                    product.Barcode = model.Barcode;

                    product.StockQuantity = model.StockQuantity;

                    product.ModifiedOn = DateTime.Now;
                    product.FeatureValueID = model.FeatureValueID > 0 ? model.FeatureValueID : (int?)null;

                    var result = ProductsService.Instance.UpdateProductType(product);
                    
                    if (!result)
                    {
                        throw new Exception("Dashboard.Products.Action.Validation.UnableToUpdateProductType".LocalizedString());
                    }
                }
                else
                {
                    ProductType productType = new ProductType
                    {
                        Price = model.Price,

                        Discount = model.Discount,
                        Cost = model.Cost,
                        SKU = model.SKU,
                        Barcode = model.Barcode,
                        IsActive = !model.InActive,
                        StockQuantity = model.StockQuantity,
                        ModifiedOn = DateTime.Now,
                        IsDeleted = model.IsDeleted,
                        ProductID = model.ProductID,
                        FeatureValueID = model.FeatureValueID
                    };
                    var result = ProductsService.Instance.SaveProductType(productType);

                    if (!result)
                    {
                        throw new Exception("Dashboard.Products.Action.Validation.UnableToCreateProductType".LocalizedString());
                    }
                }

                json.Data = new { Success = true };
            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, ex.Message };
            }

            return json;
        }


        [HttpPost]
        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult();

            try
            {
                var productType = ProductsService.Instance.GetProducTypeByID(ID);
                if (ProductsService.Instance.GetProductByID(productType.ProductID).ProductTypes.Where(x => !x.IsDeleted).ToList().Count > 1)
                {
                    var operation = ProductsService.Instance.DeleteProductType(ID);

                    result.Data = new { Success = operation, Message = operation ? string.Empty : "Dashboard.Products.Action.Validation.UnableToDeleteProduct".LocalizedString() };
                }
                else
                {
                    result.Data = new { Success = false, Message = "Dashboard.Products.Action.Validation.UnableToDeleteDefaultProduct".LocalizedString() };
                }

            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = ex.Message };
            }

            return result;
        }

        private ProductTypeActionViewModel GetProductActionViewModelFromForm(FormCollection formCollection)
        {
            var model = new ProductTypeActionViewModel
            {
                ProductID = !string.IsNullOrEmpty(formCollection["ProductID"]) ? int.Parse(formCollection["ProductID"]) : 0,
                ProductTypeID = !string.IsNullOrEmpty(formCollection["ProductTypeID"]) ? int.Parse(formCollection["ProductTypeID"]) : 0,
                Cost = !string.IsNullOrEmpty(formCollection["Cost"]) ? decimal.Parse(formCollection["Cost"]) : 0,
                Discount = !string.IsNullOrEmpty(formCollection["Discount"]) ? decimal.Parse(formCollection["Discount"]) : 0,
                Price = !string.IsNullOrEmpty(formCollection["Price"]) ? decimal.Parse(formCollection["Price"]) : 0,
                StockQuantity = !string.IsNullOrEmpty(formCollection["StockQuantity"]) ? int.Parse(formCollection["StockQuantity"]) : 0,
                FeatureValueID = !string.IsNullOrEmpty(formCollection["FeatureValueID"]) ? int.Parse(formCollection["FeatureValueID"]) : 0,
                Barcode = !string.IsNullOrEmpty(formCollection["Barcode"]) ? formCollection["Barcode"] : "",
                SKU = !string.IsNullOrEmpty(formCollection["SKU"]) ? formCollection["SKU"] : "",
            };
            return model;
        }

    }
}