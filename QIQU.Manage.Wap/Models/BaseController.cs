using QIQU.Entity.Extend;
using QIQU.Tools;
using System;
using System.Web.Mvc;

namespace QIQU.Manage.Wap
{
    public class BaseController : Controller
    {

        protected Admin currentAdmin { get; set; }

        public BaseController()
        {
            currentAdmin = AdminSession.Get();//从session中获取
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (currentAdmin == null)
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 406;
                    filterContext.Result = new JsonResult()
                    {
                        Data = new { status = 406, error = "登录超时，请重新登录！" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Home/Login");
                }
            }

            base.OnActionExecuting(filterContext);
        }

        //protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        //{
        //    if (currentAdmin == null)
        //    {
        //        if (!requestContext.HttpContext.Response.IsRequestBeingRedirected)
        //        {
        //            requestContext.HttpContext.Response.Write("<script>window.location='/Home/Login?null'</script>");
        //            requestContext.HttpContext.Response.End();
        //        }
        //        return null;
        //    }
        //    return base.BeginExecute(requestContext, callback, state);
        //}

        /// <summary>
        /// 返回JsonResult     
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="format">数据格式</param>
        /// <returns>Json</returns>
        protected JsonResult JsonExt(object data, string format)
        {
            return new CustomJsonResult
            {
                Data = data,
                FormateStr = format
            };
        }

        protected JsonResult JsonExt(object data, string contentType = "json", JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = behavior,
                FormateStr = "yyyy-MM-dd HH:mm:ss"
            };
        }
    }
}