using QIQU.Manage.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QIQU.Manage.Controllers
{
    public class ResxController : BaseController
    {
        public ResxController()
        {
        }

        //public JsonResult DelFile()
        //{
        //    string fileId = Request.Params["fileid"];
        //    string fileName = Request.Params["filename"];
        //    if (fileId != "" && fileId != null && Convert.ToInt32(fileId) > 0)
        //    {
        //        //从数据库中删除记录信息,删除后其它报价项中将不会显示
        //        fs.DelAttachment(Convert.ToInt32(fileId));
        //    }

        //    if (fileName != "" && fileName != null)
        //    {
        //        try
        //        {
        //            System.IO.File.Delete(Server.MapPath(fileName.Substring(fileName.IndexOf("UploadFile") - 1)));
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json("{state:'-1',error:'" + ex.Message + "'}");
        //        }
        //    }
        //    else
        //    {
        //        return Json("{state:'-1',error:'请选择删除文件'}");
        //    }

        //    return Json("{state:'1'}");
        //}

        //文件保存目录路径
        private String savePath = "~/Upfile/";
        //文件保存目录URL
        private String saveUrl = ConfigService.EnvironmentAddress + "/Upfile/";
        //定义允许上传的文件扩展名
        private String fileTypes = "gif,jpg,jpeg,png,bmp";
        //最大文件大小
        private int maxSize = 5 * 1024 * 1024;//默认5M
        public JsonResult Upfile(string folder = "")
        {
            if (string.IsNullOrEmpty(folder))//存储的目录名称
            {
                folder = "Default";
            }

            //检测路径中是否包含除英文，数字，下划线以外的字符
            folder = System.Text.RegularExpressions.Regex.Replace(folder, @"[^\u4e00-\u9fa5_a-zA-Z0-9]", "");
            if (folder == "" || folder == null)
            {
                return Json(new { state = -1, error = "目录名错误" }, JsonRequestBehavior.AllowGet);
            }

            //判断当前目录中的文件数量是否超过1000个，如果超过就提示不让上传
            //string[] dirFileArray = Directory.GetFiles(Server.MapPath(savePath));
            //if (dirFileArray.Length >= 1000)
            //{
            //    jsonResult = "{state:'-1',error:'目录中的文件已经超过1000个'}";
            //    return Json(jsonResult);
            //}

            HttpPostedFileBase imgFile = Request.Files[0];
            if (imgFile == null)
            {
                return Json(new { state = -1, error = "请选择文件" }, JsonRequestBehavior.AllowGet);
            }

            saveUrl = saveUrl + folder + "/" + DateTime.Now.ToString("yyyyMM");//根目录/目录名/时间（年/月）
            String dirPath = Server.MapPath(savePath) + folder + "/" + DateTime.Now.ToString("yyyyMM");//根目录/目录名/时间（年/月）
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string fileUrl = "";
            ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));
            String newFileName = "";
            //判断文件是否超过上传大小
            if (imgFile.InputStream != null && imgFile.InputStream.Length <= maxSize)
            {
                String fileExt = Path.GetExtension(imgFile.FileName).ToLower();
                newFileName = System.Text.RegularExpressions.Regex.Replace(imgFile.FileName, fileExt, "");
                newFileName = System.Text.RegularExpressions.Regex.Replace(newFileName, @"[^\u4e00-\u9fa5_a-zA-Z0-9]", "");
                newFileName = newFileName + DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + fileExt;

                //判断文件的扩展名是否在指定的范围内
                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    return Json(new { state = -2, error = "上传文件扩展名是不允许的扩展名" });
                }
                else
                {
                    String filePath = dirPath + "/" + newFileName;
                    try
                    {
                        imgFile.SaveAs(filePath);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { state = -2, error = ex.Message });
                    }
                    fileUrl = saveUrl + "/" + newFileName;
                }
            }
            else
            {
                return Json(new { state = -2, error = "超出上传大小5M" }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new { state = 1, url = fileUrl, size = imgFile.InputStream.Length }, JsonRequestBehavior.AllowGet);
            //return Content(fileUrl);
        }
    }
}