using QIQU.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIQU.Service
{
    public class BaseService
    {
        //.Skip((pageIndex - 1) * pageSize).Take(pageSize)

        protected QiQuEntities dbContext = new QiQuEntities();
        public string defaultImgUrl = "/images/default.png";
        public string defaultUserImgUrl = "/images/default_t.png";

        public string GetDefaultImgUrl(string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl)) {
                imgUrl = defaultImgUrl;
            }
            return imgUrl;
        }

        ////根据传入的数据库名称判断数据库是否存在
        //public bool IsExistsDB(string dbname)
        //{
        //    string sql = "select name from master.dbo.sysdatabases where name in ('" + dbname + "')";
        //    var dbInfo = dbContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        //    if (dbInfo == null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        ////删除数据库
        //public int DropDB(string dbname)
        //{
        //    string sql = "drop database " + dbname;
        //    int result = dbContext.Database.ExecuteSqlCommand(sql);
        //    return result;
        //}
    }
}
