//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace QIQU.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_article_coll
    {
        public int id { get; set; }
        public int category { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string keys { get; set; }
        public string summary { get; set; }
        public string contents { get; set; }
        public string img_url { get; set; }
        public int read_count { get; set; }
        public System.DateTime create_time { get; set; }
        public System.DateTime update_time { get; set; }
        public int recommend { get; set; }
        public string org_link { get; set; }
        public QIQU.Entity.ArticleAuditStatus status { get; set; }
    }
}