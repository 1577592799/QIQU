using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIQU.Web
{
    /// <summary>
    /// 点击率记录事件
    /// </summary>
    /// <param name="article"></param>
    /// <returns></returns>
    public delegate bool ArticleClickDelegate(int article);

    /// <summary>
    /// 文章点击数统计（基于cookie处理）
    /// </summary>
    public class ArticleClickCookie
    {
        static string IdentityString = "QIQUCURRENTARTICLE-";

        /// <summary>
        /// 输出当前文章的Cookie标记
        /// </summary>
        private static void OutIdentity(int articleId)
        {
            if (articleId <= 0) return;

            string cookieKey = IdentityString + articleId;

            //生成
            HttpCookie currCookie = new HttpCookie(cookieKey, articleId.ToString());
            TimeSpan tspan = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")) - DateTime.Now;
            currCookie.Expires = DateTime.Now.AddSeconds(tspan.TotalSeconds);
            HttpContext.Current.Response.Cookies.Add(currCookie);
        }

        /// <summary>
        /// 添加点击数
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public static void AddClick(int articleId, ArticleClickDelegate clickDelegate)
        {
            if (articleId <= 0 || clickDelegate == null) return;

            string cookieKey = IdentityString + articleId;
            HttpCookie currCookie = HttpContext.Current.Request.Cookies[cookieKey];
            if (currCookie != null) return;

            bool succ = clickDelegate(articleId);
            if (succ)
                OutIdentity(articleId);
        }
    }
}