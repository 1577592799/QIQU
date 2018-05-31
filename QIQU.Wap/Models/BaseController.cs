using QIQU.Entity.Extend;
using QIQU.Tools;
using System;
using System.Web.Mvc;

namespace QIQU.Wap
{
    public class BaseController : Controller
    {

        ///// <summary>
        ///// 返回JsonResult     
        ///// </summary>
        ///// <param name="data">数据</param>
        ///// <param name="format">数据格式</param>
        ///// <returns>Json</returns>
        //protected JsonResult JsonExt(object data, string format, string contentType = "json")
        //{
        //    return new CustomJsonResult
        //    {
        //        Data = data,
        //        FormateStr = format,
        //        ContentType = contentType,
        //    };
        //}

        protected JsonResult JsonExt(object data, string format, string contentType = "json", JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = behavior,
                FormateStr = format
            };
        }
    }
}