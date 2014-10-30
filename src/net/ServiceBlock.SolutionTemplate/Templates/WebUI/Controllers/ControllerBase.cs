using System;
using System.Linq;
using System.Threading;
using System.Text;
using System.Web.Mvc;
using System.Web.Configuration;
using $safeprojectname$.Communication;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace $safeprojectname$.Controllers
{
    public abstract class ControllerBase : Controller
    {

        private readonly IServiceClient _repository;
        private readonly Func<dynamic, object> _selectListItemFunc;

        public ControllerBase(string entityName, Func<dynamic, object> selectListItemFunc)
        {
            _repository = new RestServiceClient(ServiceSettings.GetRestEndpointForEntity(entityName)); ;
            _selectListItemFunc = selectListItemFunc;
        }


        [HttpPost]
        public virtual JsonResult List(
            string filterFieldName = "",
            string filterFieldValue = "",
            int jtStartIndex = 0,
            int jtPageSize = 0,
            string jtSorting = null)
        {
            try
            {
                Criteria criteria = new Criteria
                {
                    PageSize = jtPageSize,
                    PageNumber = (jtStartIndex / jtPageSize) + 1
                };

                if (!String.IsNullOrEmpty(jtSorting))
                {
                    var sortOptions = jtSorting.Split(' ');

                    criteria.SortFieldName = sortOptions[0];

                    if (sortOptions.Length > 1)
                        criteria.SortDirection = (sortOptions[1] == "ASC") ?
                            SortDirection.Ascending :
                            SortDirection.Descending;
                }

                if (!String.IsNullOrEmpty(filterFieldValue) && !String.IsNullOrEmpty(filterFieldName))
                {
                    criteria.FilterFieldName = filterFieldName;
                    criteria.FilterFieldValue = filterFieldValue;
                }

                var request = new GetWithCriteriaRequest { Criteria = criteria };

                var response = _repository.GetWithCriteria<GetWithCriteriaRequest, GetWithCriteriaResponse>(request);

                var toReturn = Json(new
                {
                    Result = "OK",
                    Records = response.Result.Body,
                    TotalRecordCount = response.Result.TotalCount
                });

                return toReturn;
            }
            catch (InvalidOperationException ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = String.Format("{0}<br/>Details:{1}", ex.Message, ex.Data["DetailHtml"])
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public virtual JsonResult GetAsOptions()
        {
            try
            {
                var results = _repository.GetAll<GetAllRequest, GetAllResponse>
                    (new GetAllRequest())
                        .Result
                        .Select(x => _selectListItemFunc(x));

                return Json(new { Result = "OK", Options = results });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = String.Format("{0}<br/>Details:{1}", ex.Message, ex.Data["DetailHtml"])
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public virtual JsonResult Create()
        {
            var entity = Request.GetPostData();

            try
            {
                if (!ModelState.IsValid)
                {
                    var sb = new StringBuilder(@"<p>Please correct the following errors:</p><ul>");
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var err in errors) sb.AppendFormat("<li>{0}</li>", err.ErrorMessage);
                    sb.Append("</ul>");
                    return Json(new { Result = "ERROR", Message = sb.ToString() });
                }

                var request = new AddOrUpdateRequest { Entities = new dynamic[] { entity } };
                var response = _repository.Add<AddOrUpdateRequest, AddOrUpdateResponse>(request);
                return Json(new { Result = "OK", Record = response.Result[0] });

            }
            catch (InvalidOperationException ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = String.Format("{0}<br/>Details:{1}", ex.Message, ex.Data["DetailHtml"])
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public virtual JsonResult Update()
        {
            var entity = Request.GetPostData();

            try
            {
                if (!ModelState.IsValid)
                {
                    var sb = new StringBuilder(@"<p>Please correct the following errors:</p><ul>");
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var err in errors) sb.AppendFormat("<li>{0}</li>", err.ErrorMessage);
                    sb.Append("</ul>");
                    return Json(new { Result = "ERROR", Message = sb.ToString() });
                }

                var request = new AddOrUpdateRequest { Entities = new dynamic[] { entity } };

                var response = _repository.Update
                    <AddOrUpdateRequest, AddOrUpdateResponse>(request);

                return Json(new { Result = "OK", Record = response.Result });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = String.Format("{0}<br/>Details:{1}", ex.Message, ex.Data["DetailHtml"])
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public virtual JsonResult Delete(int id)
        {
            try
            {
                var request = new DeleteRequest { Id = id };
                _repository.Delete<DeleteRequest, DeleteResponse>(request);
                return Json(new { Result = "OK" });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = String.Format("{0}<br/>Details:{1}", ex.Message, ex.Data["DetailHtml"])
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = ex.Message
                });
            }
        }

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
