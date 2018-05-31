using QIQU.Entity.Extend;
using QIQU.Manage.Service;
using QIQU.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QIQU.Manage.Controllers
{
    public class UserController : BaseController
    {
        FeedBackService fdService = new FeedBackService();

        #region 反馈
        public ActionResult FeedBack()
        {
            int page = CommonCs.GetObjToInt(Request.Params["page"]);
            int pageCount = 30;
            int recordCount = 0;

            ViewBag.FeedBacks = fdService.List(null, page, pageCount, out recordCount);
            ViewBag.PageHtml = CommonCs.GetPageHtmlStr(recordCount, pageCount, page, 8, "/User/FeedBack/", "");
            TempData["ReferUrl"] = string.Format("/User/FeedBack?page={0}", page);

            return View();
        }

        public ActionResult FBDetails(int? id)
        {
            FeedBackDetails model = fdService.Get(id.HasValue ? id.Value : 0);
            if (model == null)
            {
                model = new FeedBackDetails();
                model.CreateTime = DateTime.Now;
            }
            ViewBag.ReferUrl = TempData["ReferUrl"];

            return View(model);
        }

        //标记为已读
        public ActionResult FBStatus(int? id)
        {
            string error = "";
            int result = fdService.ModifyStatus(id.HasValue ? id.Value : 0, Entity.FeedBackStatus.Read, out error);

            return Json(new { state = result, error = error }, JsonRequestBehavior.DenyGet);
        }
        #endregion
    }
}
