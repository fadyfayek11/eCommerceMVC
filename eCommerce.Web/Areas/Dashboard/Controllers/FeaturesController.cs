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
    public class FeaturesController : DashboardBaseController
    {
        public ActionResult Index(string searchTerm, int? pageNo)
        {
            var pageSize = (int)RecordSizeEnums.Size10;

            FeaturesListingViewModel model = new FeaturesListingViewModel
            {
                SearchTerm = searchTerm
            };

            model.Features = FeaturesService.Instance.SearchFeatures(searchTerm, pageNo, pageSize, out int count);

            model.Pager = new Pager(count, pageNo, pageSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            FeatureActionViewModel model = new FeatureActionViewModel();

            if (ID.HasValue)
            {
                var Feature = FeaturesService.Instance.GetFeatureWithValuesByID(ID.Value);

                if (Feature == null) return HttpNotFound();

                model.ID = Feature.ID;
                model.FeatureName = Feature.FeatureName;
                model.Type = (int)Feature.Type;
                model.FeatureValues = Feature.FeatureValues.Where(_ => !_.IsDeleted).ToList();
            }

            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Action(FormCollection fromData)
        {
            JsonResult json = new JsonResult();

            var model = GetFeatureActionViewModelFromForm(fromData);
            try
            {
                if (model.ID > 0)
                {
                    var Feature = FeaturesService.Instance.GetFeatureWithValuesByID(model.ID);

                    if (Feature == null)
                    {
                        throw new Exception("Dashboard.Features.Action.Validation.FeatureNotFound".LocalizedString());
                    }
                    if (!(Feature.FeatureName == model.FeatureName && Feature.Type == (Types)model.Type))
                    {
                        Feature.ID = model.ID;
                        Feature.FeatureName = model.FeatureName;
                        Feature.Type = (Types)model.Type;

                        Feature.ModifiedOn = DateTime.Now;
                        if (!FeaturesService.Instance.UpdateFeature(Feature))
                        {
                            throw new Exception("Dashboard.Features.Action.Validation.UnableToUpdateFeature".LocalizedString());
                        }

                        json.Data = new { Success = true };
                    }
                    else
                    {
                        json.Data = new { Success = true };
                    }
                    if(model.FeatureValues != null && model.FeatureValues.Count > 0)
                    {
                        var oldFeatureValues = Feature.FeatureValues.Where(_ => !_.IsDeleted && !model.FeatureValues.Select(a => a.ID).Contains(_.ID)).ToList();
                        //delete Old Feature Values
                        foreach(var value in oldFeatureValues)
                        {
                            value.IsDeleted = true;
                            FeaturesService.Instance.UpdateFeatureValue(value);
                        }
                        foreach (var value in model.FeatureValues)
                        {
                            if(value.ID > 0)
                            {
                                var featureValue = FeaturesService.Instance.GetFeatureValueByID(value.ID);
                                if (featureValue.Value != value.Value)
                                {
                                    featureValue.Value = value.Value;
                                    featureValue.ModifiedOn= DateTime.Now;
                                    if (!FeaturesService.Instance.UpdateFeatureValue(featureValue))
                                    {
                                        throw new Exception("Dashboard.Features.Action.Validation.UnableToUpdateFeatureValue".LocalizedString());
                                    }

                                    json.Data = new { Success = true };
                                }
                            }
                            else
                            {
                                var newFeatureValue = new FeatureValue
                                {
                                    Value = value.Value,
                                    FeatureID = value.FeatureID,
                                    IsActive = true,
                                    IsDeleted = false,
                                    ModifiedOn = DateTime.Now
                                };
                                if (!FeaturesService.Instance.SaveFeatureValue(newFeatureValue))
                                {
                                    throw new Exception("Dashboard.Features.Action.Validation.UnableToCreateFeatureValue".LocalizedString());
                                }

                                json.Data = new { Success = true };
                            }

                        }
                    }
                }
                else
                {
                    Feature feature = new Feature
                    {
                        ID = model.ID,
                        FeatureName = model.FeatureName,
                        Type = (Types)model.Type,
                        IsActive = true,
                        IsDeleted = false
                    };

                    if (!FeaturesService.Instance.SaveFeature(feature))
                    {
                        throw new Exception("Dashboard.Features.Action.Validation.UnableToCreateFeature".LocalizedString());
                    }
                    json.Data = new { Success = true };
                }

            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, Message = ex.Message };
            }

            return json;
        }

        private FeatureActionViewModel GetFeatureActionViewModelFromForm(FormCollection formCollection)
        {
            var model = new FeatureActionViewModel
            {
                ID = !string.IsNullOrEmpty(formCollection["ID"]) ? int.Parse(formCollection["ID"]) : 0,
                FeatureName= formCollection["FeatureName"],
                Type = !string.IsNullOrEmpty(formCollection["Type"]) ? int.Parse(formCollection["Type"]) : 0,

                FeatureValues = new List<FeatureValue>()
            };

            foreach (string key in formCollection)
            {
                if (key.Contains("featureValue"))
                {
                    var value = formCollection[key];

                    if (!string.IsNullOrEmpty(value))
                    {
                        var values = value.Split('~');
                        var featureId = values[1];
                        var featureValue = values[0];
                        var id = values.Length > 2? values[2] : "";
                        if (!string.IsNullOrEmpty(featureValue))
                        {
                            if(id.Length> 0)
                                model.FeatureValues.Add(new FeatureValue() { FeatureID = int.Parse(featureId),ID = int.Parse(id),Value= featureValue });
                            else
                                model.FeatureValues.Add(new FeatureValue() { FeatureID = int.Parse(featureId),Value= featureValue });
                        }
                    }
                }
            }

            return model;
        }



        [HttpPost]
        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult();

            try
            {
                var operation = FeaturesService.Instance.DeleteFeature(ID);

                result.Data = new { Success = operation, Message = operation ? string.Empty : "Dashboard.Features.Action.Validation.UnableToDeleteFeature".LocalizedString() };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = string.Format("{0}", ex.Message) };
            }

            return result;
        }
    }
}