using QIQU.Entity;
using QIQU.Entity.Extend;
using QIQU.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIQU.Service
{
    public class FeedBackService : BaseService
    {
        public int Add(FeedBack model, out string error)
        {
            error = "";
            if (model == null || string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Contents))
            {
                error = "内容不能为空";
                return 0;
            }

            if (model.Title.Length > 50 || model.Contents.Length > 300)
            {
                error = "反馈内容的字数超过范围";
                return 0;
            }

            string ip = CommonCs.GetHostAddress();
            t_feedback entity = new t_feedback()
            {
                contents = model.Contents,
                title = model.Title,
                from_ip = ip,
                from_area = BaiduIPAdressAPI.GetAddress(ip),
                create_time = DateTime.Now,
                status = FeedBackStatus.Unread,
            };

            try
            {
                dbContext.t_feedback.Add(entity);
                dbContext.SaveChanges();


                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }

        }

    }
}
