using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIQU.Entity.Extend
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class CategoryListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
