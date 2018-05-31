using QIQU.Entity;
using QIQU.Entity.Extend;
using QIQU.Manage.Service;
using QIQU.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QIQU.Manage.Wap.Controllers
{
    public class AuditingController : BaseController
    {
        //
        // GET: /Auditing/

        ArticleService service = new ArticleService();

        public ActionResult Index(int? page)
        {
            page = page ?? 0;
            int pageCount = 8;
            int recordCount = 0;

            ViewBag.AuditList = service.GetAuditList(ArticleAuditStatus.Auditing, page.Value, pageCount, out recordCount);
            ViewBag.RecordCount = recordCount;

            return View();
        }

        public ActionResult List(int? page, int? status)
        {
            page = page ?? 0;
            status = status ?? 0;
            int pageCount = 8;
            int recordCount = 0;

            List<AuditArticle> arts = service.GetAuditList((ArticleAuditStatus)status, page.Value, pageCount, out recordCount);

            return JsonExt(new { list = arts, count = recordCount });
        }

        public JsonResult Category()
        {
            var list = service.CategoriesItem();
            return Json(list);
        }

        public ActionResult DelAud(int? id)
        {
            id = id ?? 0;

            string error = "";
            int result = service.DeleteAuditArticle(id.Value, out error);
            //int result = -1;
            //error = "错误测试";
            //System.Threading.Thread.Sleep(3000);
            return Json(new { state = result, error = error });
        }

        public ActionResult Publish(int? id, int? cateid)
        {
            id = id ?? 0;
            cateid = cateid ?? 0;
            string error = "";
            int result = service.Publish(id.Value,cateid.Value, out error);

            return Json(new { state = result, error = error });
        }

        public ActionResult Details(int? id)
        {
            AuditArticleDetails model = service.GetAuditArticleDetails(id.HasValue ? id.Value : 0);
            if (model == null)
            {
                model = new AuditArticleDetails();
                //model.Author = currentAdmin.RealName;
                model.CreateTime = DateTime.Now;
            }
            return View(model);
        }


    }
}
