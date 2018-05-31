using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QIQU.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //文章详情
            routes.MapRoute(
                name: "Details",
                url: "article/{id}",
                defaults: new { controller = "Home", action = "Article", id = UrlParameter.Optional }
            );

            //文章分类列表
            routes.MapRoute(
                name: "CategoryList",
                url: "category/{cate}",
                defaults: new { controller = "Home", action = "Index", cate = UrlParameter.Optional }
            );

            //文章关键字查找列表
            routes.MapRoute(
                name: "KeyList",
                url: "articles/{key}",
                defaults: new { controller = "Home", action = "Index", key = UrlParameter.Optional }
            );

            //评论列表
            routes.MapRoute(
                name: "Comments",
                url: "comments/{art}",
                defaults: new { controller = "Home", action = "Comments", art = UrlParameter.Optional }
            );

            //feedback
            routes.MapRoute(
                name: "FeedBack",
                url: "feedback",
                defaults: new { controller = "Home", action = "FeedBack" }
            );

            //首页列表
            routes.MapRoute(
                name: "Index",
                url: "index",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}