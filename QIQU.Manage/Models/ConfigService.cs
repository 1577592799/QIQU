using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIQU.Manage.Models
{
    public class ConfigService
    {
        /// <summary>
        /// 环境地址（正式，测试）
        /// </summary>
        public static string EnvironmentAddress
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["EnvironmentAddress"].ToString(); }
        }
    }
}