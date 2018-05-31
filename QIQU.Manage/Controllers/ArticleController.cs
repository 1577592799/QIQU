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
    public class ArticleController : BaseController
    {
        ArticleService service = new ArticleService();

        #region Article

        public ActionResult Edit(int? id)
        {
            Article model = service.Get(id.HasValue ? id.Value : 0);
            if (model == null)
            {
                model = new Article();
                model.Author = currentAdmin.RealName;
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
            }
            if (string.IsNullOrEmpty(model.Keys))
            {
                model.Keys = SCWSSegment.ParserStr(model.Title);
            }
            ViewBag.Categories = new SelectList(service.CategoriesItem(), "Id", "Name");
            ViewBag.ReferUrl = TempData["ReferUrl"];

            return View(model);
        }

        public ActionResult DelArt(int? id)
        {
            string error;
            int result = service.DeleteArticle(id ?? 0, out error);
            if (result > 0)
            {
                return Json(new { state = 1 });
            }
            else
            {
                return Json(new { state = -1, error = error });
            }
        }


        [ValidateInput(false)]
        public ActionResult Add(Article model)
        {
            string error;
            int result = service.Action(model, out error);
            if (result > 0)
            {
                return Json(new { state = 1 });
            }
            else
            {
                return Json(new { state = -1, error = error });
            }
        }

        public ActionResult List()
        {
            //string Email = Request.Params["email"];//Email
            //int State = Convert.ToInt32(Request.Params["state"]);//状态
            //DateTime? startDate = CommonCs.GetObjToDateTime2(Request.Params["startdate"]);//开始日期
            //DateTime? endDate = CommonCs.GetObjToDateTime2(Request.Params["enddate"]);//结束日期
            int page = CommonCs.GetObjToInt(Request.Params["page"]);
            int pageCount = 30;
            int recordCount = 0;

            ViewBag.ArticleList = service.List(page, pageCount, out recordCount);
            ViewBag.PageHtml = CommonCs.GetPageHtmlStr(recordCount, pageCount, page, 8, "/Article/List/", "");
            TempData["ReferUrl"] = string.Format("/Article/List?page={0}", page);

            return View();
        }
        #endregion

        #region Article Category

        public ActionResult CEdit(int? id)
        {
            Category model = service.GetCategoryById(id ?? 0);
            if (model == null)
            {
                model = new Category();
                model.CreateTime = DateTime.Now;
            }

            return View(model);
        }

        public ActionResult CAction(Category model)
        {
            string error;
            int result = service.CategoryAction(model, out error);
            if (result > 0)
            {
                return Json(new { state = 1 });
            }
            else
            {
                return Json(new { state = -1, error = error });
            }
        }

        public ActionResult Category()
        {
            ViewBag.CategoryList = service.Categories();
            return View();
        }

        #endregion

        #region Comments

        public ActionResult Comments()
        {
            int page = CommonCs.GetObjToInt(Request.Params["page"]);
            int pageCount = 30;
            int recordCount = 0;

            ViewBag.Comments = service.Comments(page, pageCount, out recordCount);
            ViewBag.PageHtml = CommonCs.GetPageHtmlStr(recordCount, pageCount, page, 8, "/Article/Comments/", "");
            ViewBag.CurrUrl = string.Format("/Article/Comments?page={0}", page);
            return View();
        }

        public ActionResult CmtStatus(int? id)
        {
            string error = "";
            int result = service.ModifyStatus(id.HasValue ? id.Value : 0, Entity.CommentStatus.Hide, out error);

            return Json(new { state = result, error = error }, JsonRequestBehavior.DenyGet);
        }

        #endregion
    }
}
