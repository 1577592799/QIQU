using QIQU.Entity.Extend;
using QIQU.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QIQU.Wap.Controllers
{
    public class ArticleController : BaseController
    {
        //
        // GET: /Article/
        private int pageCount = 20;
        ArticleService service = new ArticleService();

        public ActionResult Indexv2(int? cate, int? page, string key = "")
        {
            page = page ?? 0;
            cate = cate ?? 0;
            int recordCount = 0;

            ViewBag.ArticleList = service.WapList(cate.Value, key, page.Value, pageCount, out recordCount);
            ViewBag.RecordCount = recordCount;
            ViewBag.CategoryId = cate;
            ViewBag.Categies = service.CategoriesItem();

            return View();
        }

        public ActionResult Index(int? cate, int? page, string key = "")
        {
            page = page ?? 0;
            cate = cate ?? 0;
            int recordCount = 0;

            ViewBag.ArticleList = service.WapList(cate.Value, key, page.Value, pageCount, out recordCount);
            ViewBag.RecordCount = recordCount;
            ViewBag.CategoryId = cate;
            ViewBag.Categies = service.CategoriesItem();

            return View();
        }

        public ActionResult List(int? cate, int? page, string key = "")
        {
            page = page ?? 0;
            cate = cate ?? 0;
            int recordCount = 0;
            //System.Threading.Thread.Sleep(10000);
            var list = service.WapList(cate.Value, key, page.Value, pageCount, out recordCount);
            return JsonExt(new { list = list, count = recordCount }, "MM月dd日 HH:mm");
            //return Json(new { list = list, count = recordCount });
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue || id.Value <= 0) return Redirect("/Error/E404/");
            var model = service.Get(id.Value);
            if (model == null) return Redirect("/Error/E404/");

            if (model.CategoryId > 0)
            {
                var cateModel = service.GetCategory(model.CategoryId);
                if (cateModel != null) model.CategoryName = cateModel.Name;
            }
            else
            {
                model.CategoryName = "文章";
            }
            ViewBag.Comments = service.Comments(id.Value);//文章最新评论列表

            ArticleClickCookie.AddClick(id.Value, service.AddClickSuccess);

            ViewBag.Title = string.Format("{0} - {1}", model.Title, model.CategoryName);
            ViewBag.Description = model.Summary;
            ViewBag.Keywords = string.Format("{0} {1}", model.Keys, model.CategoryName);

            return View(model);
        }

        public ActionResult Comments()
        {
            return View();
        }

        //发布评论
        public ActionResult PublishCom(int artid, int comid, string contents)
        {
            //后期加上,用户是否已经登录

            string error = "";
            var comModel = service.AddComment(new ArticleComment()
            {
                ArticleId = artid,
                UserId = 0,
                ParentId = comid,
                Contents = contents,
            }, out error);

            return Json(new { state = comModel == null ? -1 : 1, error = error, comment = comModel }, JsonRequestBehavior.DenyGet);
        }
    }
}
