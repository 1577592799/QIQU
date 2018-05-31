using System;
using System.ComponentModel;
using System.Reflection;

namespace QIQU.Entity
{
    /// <summary>
    /// 0,审核中 -1,已删除 1,已发布
    /// </summary>
    public enum ArticleAuditStatus
    {
        Auditing = 0,
        Delete = -1,
        Complete = 1,
    }

    /// <summary>
    /// 评论状态 1,显示 2,隐藏
    /// </summary>
    public enum CommentStatus
    {
        [Description("显示")]
        Show = 1,
        [Description("<font color='red'>隐藏</font>")]
        Hide = 2,
    }

    /// <summary>
    /// 0,未读 1,已读
    /// </summary>
    public enum FeedBackStatus
    {
        [Description("<font color='red'>未读</font>")]
        Unread = 0,

        [Description("已读")]
        Read = 1,
    }

    /// <summary>
    /// EnumDescription.Get(status);
    /// </summary>
    public class EnumDescription
    {
        public static string Get(Enum enumValue)
        {
            string str = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }
    }

}
