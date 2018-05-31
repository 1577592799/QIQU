using PagedList;
using QIQU.Entity;
using QIQU.Entity.Extend;
using QIQU.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIQU.Manage.Service
{
    public class ArticleService : BaseService
    {
        #region 文章
        public Article Get(int id)
        {
            if (id <= 0) return null;
            var artModel = dbContext.t_article.Where(a => a.id == id).FirstOrDefault();
            return artModel != null ? artModel.ConvertTo() : null;
        }

        public List<Article> List(int pageIndex, int pageCount, out int totalCount)
        {
            totalCount = 0;

            var query = from art in dbContext.t_article
                        join cate in dbContext.t_article_category
                        on art.category equals cate.id into temp
                        from tt in temp.DefaultIfEmpty()
                        select new Article()
                        {
                            Id = art.id,
                            Author = art.author,
                            CategoryId = art.category,
                            CategoryName = tt.name,
                            Title = art.title,
                            Keys = art.keys,
                            Summary = art.summary,
                            ImgUrl = art.img_url,
                            Contents = art.contents,
                            ReadCount = art.read_count,
                            CommentCount = art.comment_count,
                            CreateTime = art.create_time,
                            UpdateTime = art.update_time,
                            Recommend = art.recommend == 1 ? true : false,
                        };

            IPagedList<Article> pageList = query.OrderByDescending(m => m.Id).ToPagedList((pageIndex <= 0 ? 1 : pageIndex), pageCount);
            totalCount = pageList.TotalItemCount;
            return pageList.ToList();
        }

        /// <summary>
        /// 更新，添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public int Action(Article model, out string error)
        {
            error = "";
            if (model == null)
            {
                error = "数据不能为空";
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                error = "标题不能为空";
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.Contents))
            {
                error = "内容不能为空";
                return 0;
            }
            //if (model.Contents.Length > 2000)
            //{
            //    error = "内容字数在2000个以内";
            //    return 0;
            //}
            if (string.IsNullOrEmpty(model.ImgUrl))
            {
                model.ImgUrl = CommonCs.GetImageSrc(model.Contents);
            }

            if (model.Id > 0)//更新
            {
                var artModel = dbContext.t_article.Where(a => a.id == model.Id).FirstOrDefault();
                if (artModel == null)
                {
                    error = "更新失败";
                    return 0;
                }
                artModel.category = model.CategoryId;
                artModel.title = model.Title;
                artModel.keys = model.Keys;
                if (string.IsNullOrEmpty(artModel.keys))//关键字处理
                {
                    artModel.keys = SCWSSegment.ParserStr(model.Title);
                }
                artModel.contents = model.Contents;
                if (string.IsNullOrEmpty(model.Summary))//摘要处理
                {
                    artModel.summary = CommonCs.GetChineseWord(model.Contents);
                }
                artModel.author = model.Author;
                artModel.read_count = model.ReadCount;
                artModel.update_time = DateTime.Now;
                artModel.recommend = model.Recommend ? 1 : 0;
                artModel.img_url = model.ImgUrl;
            }
            else //添加
            {
                if (string.IsNullOrEmpty(model.Keys))//关键字处理
                {
                    model.Keys = SCWSSegment.ParserStr(model.Title);
                }
                model.ImgUrl = CommonCs.GetImageSrc(model.Contents);
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                dbContext.t_article.Add(model.ConvertTo());
            }

            try
            {
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteArticle(int id, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "参数错误";
                return 0;
            }

            var artModel = dbContext.t_article.Where(a => a.id == id).FirstOrDefault();
            if (artModel == null)
            {
                error = "数据不存在";
                return 0;
            }


            try
            {
                dbContext.t_article.Remove(artModel);
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }

        #endregion

        #region 文章类型

        /// <summary>
        /// 获取所有文章类型
        /// </summary>
        /// <returns></returns>
        public List<Category> Categories()
        {
            return dbContext.t_article_category.OrderByDescending(c => c.id).Select(c => new Category()
            {
                Id = c.id,
                CreateTime = c.create_time,
                Name = c.name,
                Remark = c.remark,
            }).ToList();
        }

        /// <summary>
        /// 获取所有文章类型
        /// </summary>
        /// <returns></returns>
        public List<CategoryListItem> CategoriesItem()
        {
            return dbContext.t_article_category.OrderByDescending(c => c.id).Select(c => new CategoryListItem()
            {
                Id = c.id,
                Name = c.name,
            }).ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            if (categoryId <= 0) return null;

            return dbContext.t_article_category.Where(c => c.id == categoryId).Select(c => new Category()
            {
                Id = c.id,
                CreateTime = c.create_time,
                Name = c.name,
                Remark = c.remark,
            }).FirstOrDefault();
        }
        public int CategoryAction(Category model, out string error)
        {
            error = "";
            if (model == null)
            {
                error = "数据不能为空";
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                error = "名称不能为空";
                return 0;
            }

            if (model.Id > 0)//更新
            {
                var cateModel = dbContext.t_article_category.Where(a => a.id == model.Id).FirstOrDefault();
                if (cateModel == null)
                {
                    error = "更新失败";
                    return 0;
                }
                cateModel.name = model.Name;
                cateModel.remark = model.Remark;
            }
            else //添加
            {
                dbContext.t_article_category.Add(new t_article_category()
                {
                    name = model.Name,
                    remark = model.Remark,
                    create_time = DateTime.Now,
                });
            }

            try
            {
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }

        #endregion

        #region 文章审核

        /// <summary>
        /// 获取审核列表
        /// </summary>
        public List<AuditArticle> GetAuditList(ArticleAuditStatus status, int pageIndex, int pageCount, out int totalCount)
        {
            totalCount = 0;

            var query = dbContext.t_article_coll.Where(t => t.status == status).OrderByDescending(t => t.id).Select(t => new AuditArticle()
                        {
                            Id = t.id,
                            Title = t.title,
                            ImgUrl = string.IsNullOrEmpty(t.img_url) ? this.DefaultImage : t.img_url,
                            Author = t.author,
                            CreateTime = t.create_time,
                        });

            IPagedList<AuditArticle> pageList = query.OrderByDescending(m => m.Id).ToPagedList((pageIndex <= 0 ? 1 : pageIndex), pageCount);
            totalCount = pageList.TotalItemCount;
            return pageList.ToList();
        }

        /// <summary>
        /// 获取审核信息的详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuditArticleDetails GetAuditArticleDetails(int id)
        {
            if (id <= 0) return null;
            var model = dbContext.t_article_coll.Where(a => a.id == id).Select(a => new AuditArticleDetails()
            {
                Id = a.id,
                Author = a.author,
                Contents = a.contents,
                CreateTime = a.create_time,
                status = a.status,
                Title = a.title,
            }).FirstOrDefault();
            return model;
        }

        public int DeleteAuditArticle(int id, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "参数错误";
                return 0;
            }

            var model = dbContext.t_article_coll.Where(a => a.id == id).FirstOrDefault();
            if (model == null)
            {
                error = "审核数据不存在";
                return 0;
            }

            try
            {
                model.status = ArticleAuditStatus.Delete;
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 审核发布
        /// </summary>
        /// <param name="id"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public int Publish(int id, int categoryId, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "参数错误";
                return 0;
            }
            if (categoryId <= 0)
            {
                error = "发布类型错误";
                return 0;
            }

            var model = dbContext.t_article_coll.Where(a => a.id == id).FirstOrDefault();
            if (model == null)
            {
                error = "发布的数据不存在";
                return 0;
            }
            if (model.status != ArticleAuditStatus.Auditing)
            {
                error = "发布数据的状态错误";
                return 0;
            }

            int result = this.Action(new Article()
            {
                Id = 0,
                Author = model.author,
                CategoryId = categoryId,
                Contents = model.contents,
                Summary = CommonCs.GetChineseWord(model.contents),
                Keys = "",
                Title = model.title,
                Recommend = false,
            }, out error);

            if (result <= 0)
            {
                //error = "发布失败";
                return 0;
            }

            try
            {
                model.status = ArticleAuditStatus.Complete;
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }

        #endregion

        #region 评论审核

        /// <summary>
        /// 获取文章评论列表
        /// </summary>
        /// <returns></returns>
        public List<CommentAuditModel> Comments(int pageIndex, int pageCount, out int totalCount)
        {
            //获取所有需要审核的评论
            IQueryable<t_article_comments> query = dbContext.t_article_comments.OrderByDescending(c => c.id);

            IPagedList<CommentAuditModel> pageList = query.Select(c => new CommentAuditModel()
            {
                ArticleId = c.article_id,
                ChildCount = c.child_count,
                Id = c.id,
                Contents = c.contents,
                status = c.status,
                ParentId = c.parent_id,
                CreateTime = c.create_time,
                FromIP = c.from_ip,
                FromArea = c.from_area,
                UserId = c.user_id,
                UserImage = defaultUserImgUrl,
                UserName = "游客",
            }).ToPagedList((pageIndex <= 0 ? 1 : pageIndex), pageCount);
            totalCount = pageList.TotalItemCount;
            return pageList.ToList();
        }

        public int ModifyStatus(int id, CommentStatus status, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "参数错误";
                return 0;
            }

            var model = dbContext.t_article_comments.FirstOrDefault(f => f.id == id);
            if (model == null)
            {
                error = "不存在评论信息";
                return 0;
            }

            try
            {
#warning 当评论被隐藏或者删除的时候，是否需要将对应文章的评论减少？？？思考ing...
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

        #endregion
    }
}
