﻿using System.Web;
using System.Web.Mvc;

namespace QIQU.Manage.Wap
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}