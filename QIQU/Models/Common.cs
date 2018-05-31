using System;
using System.Text;
using System.Web;

namespace QIQU.Web
{
    public class Common
    {
        
        public static string GetPageHtmlStr(int RecordCount, int PageSize, int PageIndex, int PageList, string strWhere,string urlParam)
        {
            string cssClass = "page-numbers";
            StringBuilder strPage = new StringBuilder();
            if (PageSize <= 0) PageSize = 1;
            int PageCount = (RecordCount + PageSize - 1) / PageSize;
            int PageTemp = 0;
            if (PageIndex > PageCount)
            {
                PageIndex = PageCount;
            }
            else if (PageIndex <= 0)
            {
                PageIndex = 1;
            }

            strPage.AppendFormat("共有{0}条记录 每页{1}条 {2}/{3} &nbsp;", RecordCount, PageSize, PageIndex, PageCount);
            strPage.AppendFormat("<a href=\"{0}?page=1{1}\" class=\"page-numbers\">首页</a>&nbsp;", strWhere, urlParam);
            strPage.AppendFormat("<a href=\"{1}?page={0}{2}\" class=\"page-numbers\" onfocus=\"this.blur()\">上一页</a> &nbsp;", PageIndex - 1 <= 0 ? 1 : PageIndex - 1, strWhere, urlParam);

            PageTemp = ((PageIndex - 1) / PageList) * PageList + 1;
            int i = 1;
            while (i <= PageList && PageTemp <= PageCount)
            {
                i++;
                strPage.AppendFormat(PageTemp == PageIndex ? "<a href='javascript:void(0)' class='page-numbers current'>{0}</a>&nbsp;" : "<a href=\"{1}?page={0}{2}\" class=\"page-numbers\" onfocus=\"this.blur()\">{0}</a>&nbsp;\n", PageTemp++, strWhere, urlParam);
            }
            strPage.AppendFormat("<a href=\"{1}?page={0}{3}\" class=\"{2}\" onfocus=\"this.blur()\">下一页</a>&nbsp;\n", PageIndex + 1 > PageCount ? PageCount : PageIndex + 1, strWhere, cssClass, urlParam);
            strPage.AppendFormat("<a href=\"{1}?page={0}{3}\" class=\"{2}\" onfocus=\"this.blur()\">尾页</a>", PageCount, strWhere, cssClass, urlParam);
            //strPage.AppendFormat(" <input type=\"text\" align=\"absmiddle\" id=\"goPage\" name=\"goPage\" class=\"input_blue\" size=\"2\" Width=\"50px\" value=\"{0}\" />\n", PageIndex);
            //strPage.AppendFormat(" <input type=\"button\" name=\"go\" value=\"go\"   style=\"border:#999999 1px solid;\" onfocus=\"this.blur()\" onClick=\"JavaScript:window.location.href='{0}?page='+document.getElementById('goPage').value +'{1}'\" />\n", strWhere, urlParam);
            return strPage.ToString();

        }
    }
}