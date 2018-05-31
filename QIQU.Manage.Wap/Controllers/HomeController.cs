using QIQU.Entity.Extend;
using QIQU.Manage.Service;
using System;
using System.Web.Mvc;

namespace QIQU.Manage.Wap.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginDo()
        {
            string name = Request.Params["name"];
            string pwd = Request.Params["pwd"];

            AdminService service = new AdminService();
            Admin result = service.Login(name, pwd);


            if (result.LoginStatus != AdminLoginStatus.Success)
            {
                return Json(new { state = -1, error = "用户名不存在或者密码错误" });
            }
            else
            {
                AdminSession.Set(result);
                return Json(new { state = 1, gto = "/Auditing/Index" });
            }
        }

    }
}
