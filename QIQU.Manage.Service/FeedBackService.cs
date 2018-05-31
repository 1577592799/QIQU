using PagedList;
using QIQU.Entity;
using QIQU.Entity.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIQU.Manage.Service
{
    public class FeedBackService : BaseService
    {

        public List<FeedBackModels> List(FeedBackStatus? status, int pageIndex, int pageSize, out int recordCount)
        {
            IQueryable<t_feedback> query = dbContext.t_feedback.OrderByDescending(f => f.id);
            if (status != null)
            {
                query = query.Where(q => q.status == status);
            }

            IPagedList<FeedBackModels> pageList = query.Select(q => new FeedBackModels()
            {
                Id = q.id,
                CreateTime = q.create_time,
                FromArea = q.from_area,
                FromIp = q.from_ip,
                status = q.status,
                Title = q.title,
                UserId = q.user_id,
            }).ToPagedList((pageIndex <= 0 ? 1 : pageIndex), pageSize);
            recordCount = pageList.TotalItemCount;
            return pageList.ToList();
        }

        public FeedBackDetails Get(int id)
        {
            if (id <= 0) return null;

            var model = dbContext.t_feedback.FirstOrDefault(f => f.id == id);
            if (model != null)
            {
                return new FeedBackDetails()
                {
                    Id = model.id,
                    Contents = model.contents,
                    CreateTime = model.create_time,
                    FromArea = model.from_area,
                    FromIp = model.from_ip,
                    status = model.status,
                    Title = model.title,
                    UserId = model.user_id,
                    UserImage = base.DefaultImage,
                    UserName = "",
                };
            }
            return null;
        }

        public int ModifyStatus(int id, FeedBackStatus status, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "参数错误";
                return 0;
            }

            var model = dbContext.t_feedback.FirstOrDefault(f => f.id == id);
            if (model == null)
            {
                error = "不存在反馈信息";
                return 0;
            }

            try
            {
                model.status = status;
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
