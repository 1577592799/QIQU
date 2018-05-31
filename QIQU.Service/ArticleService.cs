using PagedList;
using QIQU.Entity;
using QIQU.Entity.Extend;
using QIQU.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIQU.Service
{
    public class ArticleService : BaseService
    {
        #region select
        public Article Get(int id)
        {
            if (id <= 0) return null;
            var artModel = dbContext.t_article.Where(a => a.id == id).FirstOrDefault();
            return artModel != null ? artModel.ConvertTo() : null;
        }

        /// <summary>
        /// 获取列表，默认返回推荐的数据
        /// </summary>
        /// <param name="isRecommand">默认isRecommand = true</param>
        /// <param name="count">默认count=3</param>
        /// <returns></returns>
        public List<ArticleIndex> List(bool isRecommand = true, int count = 3)
        {
            var list = dbContext.t_article.Where(m => m.recommend == (isRecommand ? 1 : 0)).OrderByDescending(m => m.id).Take(count).ToList();
            if (list == null) return null;

            return list.Select(m => new ArticleIndex
            {
                Id = m.id,
                CategoryId = m.category,
                Author = m.author,
                Title = m.title,
                ReadCount = m.read_count,
                ImgUrl = GetDefaultImgUrl(m.img_url),
                Summary = m.summary,
                CreateTime = m.create_time,
            }).ToList();
        }

        /// <summary>
        /// 获取文章列表，根据阅读数排序
        /// </summary>
        public List<ArticleHot> ReadHotList(int count = 8)
        {
            return dbContext.t_article.OrderByDescending(m => m.read_count).Select(m => new ArticleHot
            {
                Id = m.id,
                Title = m.title,
            }).Take(count).ToList();
        }

        /// <summary>
        /// 同类型文章列表
        /// </summary>
        /// <param name="category"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ArticleSameCategory> TheSameCategoryArticles(int category, int currArticleId = 0, int count = 3)
        {
            IQueryable<t_article> query = dbContext.t_article.Where(m => m.category == category);

            if (currArticleId > 0)
            {
                query = query.Where(q => q.id != currArticleId);
            }

            return query.OrderByDescending(m => m.id).Select(m => new ArticleSameCategory
            {
                Id = m.id,
                Title = m.title,
                ImgUrl = m.img_url,
            }).Take(count).ToList();
        }

        /// <summary>
        /// 获取分页数据，默认返回不是推荐的数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="totalCount"></param>
        /// <param name="isRecommand">默认isRecommand = false</param>
        /// <returns></returns>
        public List<ArticleIndex> List(string key, int category, int pageIndex, int pageCount, out int totalCount, bool isRecommand = false)
        {
            totalCount = 0;

            var query = from art in dbContext.t_article
                        join cate in dbContext.t_article_category
                        on art.category equals cate.id into temp
                        from tt in temp.DefaultIfEmpty()
                        select new ArticleIndex()
                        {
                            Id = art.id,
                            Author = art.author,
                            CategoryId = art.category,
                            CategoryName = tt.name,
                            Title = art.title,
                            Summary = art.summary,
                            ImgUrl = art.img_url,
                            ReadCount = art.read_count,
                            CreateTime = art.create_time,
                        };
            if (category > 0)
            {
                query = query.Where(q => q.CategoryId == category);
            }
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(q => q.Title.Contains(key));
            }

            IPagedList<ArticleIndex> pageList = query.OrderByDescending(m => m.Id).ToPagedList((pageIndex <= 0 ? 1 : pageIndex), pageCount);
            totalCount = pageList.TotalItemCount;
            return pageList.ToList();
        }

        /// <summary>
        /// 获取分页数据，默认返回不是推荐的数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="totalCount"></param>
        /// <param name="isRecommand">默认isRecommand = false</param>
        /// <returns></returns>
        public List<ArticleIndexWap> WapList(int category, string key, int pageIndex, int pageCount, out int totalCount)
        {
            totalCount = 0;
            IQueryable<t_article> query = dbContext.t_article.OrderByDescending(m => m.id);
            if (category > 0)
            {
                query = query.Where(q => q.category == category);
            }
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(q => q.title.Contains(key));
            }

            var pageList = query.ToPagedList((pageIndex <= 0 ? 1 : pageIndex), pageCount);

            totalCount = pageList.TotalItemCount;
            return pageList.Select(art => new ArticleIndexWap()
            {
                Id = art.id,
                Author = art.author,
                Title = art.title,
                ImgUrl = GetDefaultImgUrl(art.img_url),
                ReadCount = art.read_count,
                CreateTime = art.create_time,
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

        public CategoryListItem GetCategory(int categoryId)
        {
            if (categoryId <= 0) return null;
            return dbContext.t_article_category.Where(c => c.id == categoryId).Select(c => new CategoryListItem()
            {
                Id = c.id,
                Name = c.name,
            }).FirstOrDefault();
        }

        #endregion

        #region action

        /// <summary>
        /// 增加文章点击数
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool AddClickSuccess(int article)
        {
            if (article <= 0) return false;

            var model = dbContext.t_article.Where(a => a.id == article).FirstOrDefault();
            if (model == null) return false;

            try
            {
                model.read_count = model.read_count + 1;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 文章评论

        /// <summary>
        /// 获取文章评论列表
        /// </summary>
        /// <returns></returns>
        public List<ArticleComments> Comments(int articleId, int count = 20)
        {
            if (articleId <= 0) return null;

            //获取一级评论列表
            List<ArticleComments> comlist = dbContext.t_article_comments.OrderByDescending(c => c.id)
                .Where(c =>
                    c.status == CommentStatus.Show
                    && c.parent_id == 0
                    && c.article_id == articleId)
                .Select(c => new ArticleComments()
                {
                    ArticleId = c.article_id,
                    ChildCount = c.child_count,
                    CommentId = c.id,
                    Contents = c.contents,
                    CreateTime = c.create_time,
                    FromArea = c.from_area,
                    UserId = c.user_id,
                    UserImage = defaultUserImgUrl,
                    UserName = "游客",
                }).ToList();//.Take(count)

            if (comlist == null || comlist.Count <= 0) return comlist;

            //根据一级评论列表获取对应的回复列表
            int[] commentIdArray = comlist.Where(c => c.ChildCount > 0).Select(c => c.CommentId).ToArray();
            if (commentIdArray == null || commentIdArray.Length <= 0) return comlist;

            List<ArticleChildComments> childComments = dbContext.t_article_comments.OrderBy(c => c.id)
                 .Where(c =>
                     c.status == CommentStatus.Show
                     && c.parent_id > 0
                     && c.article_id == articleId
                     && commentIdArray.Contains(c.parent_id))
                 .Select(c => new ArticleChildComments()
                 {
                     CommentId = c.id,
                     ParentId = c.parent_id,
                     Contents = c.contents,
                     CreateTime = c.create_time,
                     FromArea = c.from_area,
                     UserId = c.user_id,
                     UserName = "游客",
                 }).ToList();

            if (childComments == null || childComments.Count <= 0) return comlist;

            foreach (var item in comlist)
            {
                item.Comments = childComments.Where(c => c.ParentId == item.CommentId).ToList();
            }

            return comlist;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public ArticleComments AddComment(ArticleComment model, out string error)
        {
            error = "";
            if (model == null)
            {
                error = "评论模型不能为空";
                return null;
            }
            if (model.ArticleId <= 0)
            {
                error = "参数错误";
                return null;
            }
            if (string.IsNullOrEmpty(model.Contents))
            {
                error = "评论内容不能为空";
                return null;
            }
            if (model.Contents.Length > 200)
            {
                error = "评论字数只能在200字以内";
                return null;
            }

            string ip = CommonCs.GetIP();
            t_article_comments entity = new t_article_comments()
            {
                article_id = model.ArticleId,
                contents = model.Contents,
                parent_id = model.ParentId,
                user_id = model.UserId,
                from_ip = ip,
                from_area = BaiduIPAdressAPI.GetAddress(ip),

                child_count = 0,
                status = CommentStatus.Show,
                create_time = DateTime.Now,
            };

            try
            {
                dbContext.t_article_comments.Add(entity);
                dbContext.SaveChanges();

                #region 更新文章评论数，更新评论回复数

                if (entity.id > 0)//表示添加评论成功
                {
                    //更新文章评论数
                    if (entity.article_id > 0)
                        this.UpdateArticleCommentCount(entity.article_id);

                    //更新评论回复数
                    if (entity.parent_id > 0)
                        this.UpdateCommentCount(entity.parent_id);
                }


                #endregion

                return new ArticleComments()
                {
                    ArticleId = entity.article_id,
                    ChildCount = entity.child_count,
                    CommentId = entity.id,
                    Contents = entity.contents,
                    CreateTime = entity.create_time,
                    FromArea = entity.from_area,
                    UserId = entity.user_id,
                    UserImage = base.defaultUserImgUrl,
                    UserName = "游客",
                };
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }

        }

        /// <summary>
        /// 更新文章评论数量
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public int UpdateArticleCommentCount(int articleId)
        {
            if (articleId <= 0) return 0;

            var model = dbContext.t_article.Where(a => a.id == articleId).FirstOrDefault();
            if (model == null) return 0;

            try
            {
                model.comment_count = model.comment_count + 1;
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新评论回复数量
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public int UpdateCommentCount(int commentId)
        {
            if (commentId <= 0) return 0;

            var model = dbContext.t_article_comments.Where(a => a.id == commentId).FirstOrDefault();
            if (model == null) return 0;

            try
            {
                model.child_count = model.child_count + 1;
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion
    }
}
