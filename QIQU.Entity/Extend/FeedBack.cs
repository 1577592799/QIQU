using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIQU.Entity.Extend
{
    public class FeedBack
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime CreateTime { get; set; }
        public string FromIp { get; set; }
        public string FromArea { get; set; }

        public string VCode { get; set; }
    }

    public class FeedBackModels
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public FeedBackStatus status { get; set; }
        public int UserId { get; set; }
        public string FromIp { get; set; }
        public string FromArea { get; set; }
    }

    public class FeedBackDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime CreateTime { get; set; }
        public FeedBackStatus status { get; set; }
        public string FromIp { get; set; }
        public string FromArea { get; set; }
    }
}
