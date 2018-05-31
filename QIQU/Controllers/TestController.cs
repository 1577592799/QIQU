using QIQU.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QIQU.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            //string format="<p style='text-align: center; '>   <a href='http://www.ishayou.com/wp-content/uploads/2016/07/kiy7.png'><img class='alignnone size-full wp-image-1086' src=\"/wp-content/uploads/2016/07/kiy7.png\" alt='kiy7' width='1000' height='600' style='width: 721px; height: 514px;'/></a></p>";
            //string str = QIQU.Tools.CommonCs.GetImageSrc(format);

            //return Content(str);


            string theIP = "222.211.22.245"; //BaiduIPAdressAPI.GetHostAddress();
            string msg = BaiduIPAdressAPI.GetInfoByUrl(BaiduIPAdressAPI.GetPostUrl(theIP));
            msg = System.Text.RegularExpressions.Regex.Unescape(msg);
            return Content(msg);
        }


        public ActionResult FC()
        {
            List<string> list = SCWSSegment.Parser("2023年宝马超级摩托");
            if (list == null) return Content("NULL");
            string s = "";
            foreach (var item in list)
            {
                s += "  " + item;
            }
            return Content(s);

        }
    }
}
