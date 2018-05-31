using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QIQU.Wap
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
                defaults: new { controller = "Article", action = "Details", id = UrlParameter.Optional }
            );

            //文章分类列表
            routes.MapRoute(
                name: "CategoryList",
                url: "category/{cate}",
                defaults: new { controller = "Article", action = "Index", cate = UrlParameter.Optional }
            );

            //文章列表
            routes.MapRoute(
                name: "ArticleList",
                url: "articles",
                defaults: new { controller = "Article", action = "List", cate = UrlParameter.Optional }
            );

            //发布评论
            routes.MapRoute(
                name: "PublishComment",
                url: "publish",
                defaults: new { controller = "Article", action = "PublishCom", cate = UrlParameter.Optional }
            );

            ////文章关键字查找列表
            //routes.MapRoute(
            //    name: "KeyList",
            //    url: "articles/{key}",
            //    defaults: new { controller = "Home", action = "Index", key = UrlParameter.Optional }
            //);

            //首页列表
            routes.MapRoute(
                name: "Index",
                url: "index",
                defaults: new { controller = "Article", action = "Index" }
            );

            //首页列表V2
            routes.MapRoute(
                name: "Index V2",
                url: "indexv2",
                defaults: new { controller = "Article", action = "Indexv2" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}