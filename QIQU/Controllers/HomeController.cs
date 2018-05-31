using QIQU.Entity.Extend;
using QIQU.Service;
using QIQU.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QIQU.Web.Controllers
{
    public class HomeController : Controller
    {
        ArticleService service = new ArticleService();

        //首页
        public ActionResult Index(int? cate, int? page, string key = "")
        {
            page = page ?? 0;
            cate = cate ?? 0;
            int pageCount = 30;
            int totalCount = 0;

            ViewBag.ArticleList = service.List(key, cate.Value, page.Value, pageCount, out totalCount);
            ViewBag.PageHtml = Common.GetPageHtmlStr(totalCount, pageCount, page.Value, 5, "/articles", string.Format("&cate={0}&key={1}", cate, key));
            ViewBag.Categies = service.CategoriesItem();
            ViewBag.ReadHotList = service.ReadHotList();

            if (string.IsNullOrEmpty(key))//如果是查找，则取消显示推荐的数据
            {
                ViewBag.ArticleRecommandList = service.List(true, 1);
            }

            return View();
        }

        //文章详细页
        public ActionResult Article(int? id)
        {
            if (!id.HasValue || id.Value <= 0) return Redirect("/Error/E404/");
            var model = service.Get(id.Value);
            if (model == null) return Redirect("/Error/E404/");

            List<CategoryListItem> catelist = service.CategoriesItem();
            ViewBag.Categies = catelist;
            ViewBag.ReadHotList = service.ReadHotList();
            ViewBag.SameCateArticles = service.TheSameCategoryArticles(model.CategoryId, id.Value);//相关文章
            ViewBag.Comments = service.Comments(id.Value);//文章最新评论列表

            ArticleClickCookie.AddClick(id.Value, service.AddClickSuccess);

            string cateName = catelist != null ? catelist.Where(c => c.Id == model.CategoryId).Select(c => c.Name).FirstOrDefault() : "";
            ViewBag.Title = string.Format("{0} - {1}", model.Title, cateName);
            ViewBag.Description = model.Summary;
            ViewBag.Keywords = string.Format("{0} {1}", model.Keys, cateName);

            return View(model);
        }

        //public ActionResult Comments(int? art)
        //{
        //    if (!art.HasValue || art.Value <= 0) return View();  //return Redirect("/Error/E404/");

        //    return View();
        //}

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

        #region 反馈处理
        public ActionResult FeedBack()
        {
            FeedBack model = new FeedBack();
            return View(model);
        }

        public ActionResult FeedbackDo(FeedBack model)
        {
            if (string.IsNullOrEmpty(model.VCode) || model.VCode != Convert.ToString(Session["CodeImage"]))
            {
                return Json(new { state = -2, error = "验证码错误" }, JsonRequestBehavior.DenyGet);
            }

            string error = "";
            int result = new FeedBackService().Add(model, out error);
            return Json(new { state = result, error = error }, JsonRequestBehavior.DenyGet);
        }

        //输出验证码图片
        public ActionResult CodeImage()
        {
            CodeImage c = new CodeImage();
            string codeImage;
            byte[] imageByte = c.CreateCheckCodeImage(out codeImage);
            Session["CodeImage"] = codeImage;
            return File(imageByte, @"image/jpeg");
        }
        #endregion

    }
}
