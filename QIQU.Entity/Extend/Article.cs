using QIQU.Tools;
using System;
using System.Collections.Generic;

namespace QIQU.Entity.Extend
{

    public static class ArticleConvert
    {
        public static Article ConvertTo(this t_article entity)
        {
            return new Article()
            {
                Id = entity.id,
                Author = entity.author,
                CategoryId = entity.category,
                Title = entity.title,
                Keys = entity.keys,
                Summary = entity.summary,
                ImgUrl = entity.img_url,
                Contents = entity.contents,
                ReadCount = entity.read_count,
                CommentCount = entity.comment_count,
                CreateTime = entity.create_time,
                UpdateTime = entity.update_time,
                Recommend = entity.recommend == 1 ? true : false,
            };
        }
        public static t_article ConvertTo(this Article model)
        {
            return new t_article()
            {
                id = model.Id,
                author = model.Author,
                category = model.CategoryId,
                title = model.Title,
                img_url = model.ImgUrl,
                keys = model.Keys,
                summary = model.Summary,
                contents = model.Contents,
                read_count = model.ReadCount,
                comment_count = model.CommentCount,
                create_time = model.CreateTime,
                update_time = model.UpdateTime,
                recommend = model.Recommend ? 1 : 0,
            };
        }


        //public static ArticleIndex ConvertToV2(this t_article entity)
        //{
        //    return new ArticleIndex()
        //    {
        //        Id = entity.id,
        //        Author = entity.author,
        //        Title = entity.title,
        //        ImgUrl = entity.img_url,
        //        Category = entity.category,
        //        ReadCount = entity.read_count,
        //        CreateTime = entity.create_time,
        //        Summary = entity.summary,
        //    };
        //}
        //public static t_article ConvertTo(this ArticleIndex model)
        //{
        //    return new t_article()
        //    {
        //        id = model.Id,
        //        author = model.Author,
        //        title = model.Title,
        //        img_url = model.ImgUrl,
        //        category = model.Category,
        //        read_count = model.ReadCount,
        //        create_time = model.CreateTime,
        //        summary = model.Summary,
        //    };
        //}
    }

    /// <summary>
    /// 详细信息实体
    /// </summary>
    public class Article
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Keys { get; set; }
        public string Contents { get; set; }
        public int ReadCount { get; set; }
        public int CommentCount { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Summary { get; set; }

        /// <summary>
        /// 是否推荐 1是 0否
        /// </summary>
        public bool Recommend { get; set; }
    }

    /// <summary>
    /// web首页列表实体
    /// </summary>
    public class ArticleIndex
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int ReadCount { get; set; }
        public string ImgUrl { get; set; }
        public string Summary { get; set; }
        public DateTime CreateTime { get; set; }
    }


    /// <summary>
    /// wap首页列表实体
    /// </summary>
    public class ArticleIndexWap
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int ReadCount { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 最新热门信息实体
    /// </summary>
    public class ArticleHot
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// 同类型信息实体
    /// </summary>
    public class ArticleSameCategory
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// 信息审核实体
    /// </summary>
    public class AuditArticle
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class AuditArticleDetails
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime CreateTime { get; set; }
        public ArticleAuditStatus status { get; set; }
    }

    /// <summary>
    /// 发表评论模型
    /// </summary>
    public class ArticleComment
    {
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public int ParentId { get; set; }
        public string Contents { get; set; }
    }

    /// <summary>
    /// 文章评论列表模型
    /// </summary>
    public class ArticleComments
    {
        public int CommentId { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public int ChildCount { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string Contents { get; set; }
        public string FromArea { get; set; }
        public DateTime CreateTime { get; set; }
        public List<ArticleChildComments> Comments { get; set; }

        public string CreateTimeStr()
        {
            return CommonCs.DateFormatToString(this.CreateTime);
        }
    }

    /// <summary>
    /// 文章回复评论模型
    /// </summary>
    public class ArticleChildComments
    {
        public int CommentId { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Contents { get; set; }
        public string FromArea { get; set; }
        public DateTime CreateTime { get; set; }

        public string CreateTimeStr()
        {
            return CommonCs.DateFormatToString(this.CreateTime);
        }
    }

    public class CommentAuditModel
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int ParentId { get; set; }
        public int ChildCount { get; set; }
        public CommentStatus status { get; set; }
        public string Contents { get; set; }
        public string FromIP { get; set; }
        public string FromArea { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
    }
}
