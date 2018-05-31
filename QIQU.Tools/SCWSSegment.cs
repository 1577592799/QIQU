using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace QIQU.Tools
{
    /// <summary>
    /// 分词后返回的数据模型
    /// </summary>
    public class SCWSModel
    {
        public string status { get; set; }
        public Word[] words { get; set; }
    }

    public class Word
    {
        public string word { get; set; }
        public int off { get; set; }
        public int len { get; set; }
        public float idf { get; set; }
        public string attr { get; set; }
    }

    public class SCWSSegment
    {

        public static string ParserStr(string str)
        {
            List<string> list = Parser(str);
            if (list != null && list.Count > 0)
            {
                string s = "";
                foreach (var item in list)
                {
                    s += item + ",";
                }
                return s.Substring(0, s.Length - 1);
            }
            return "";
        }

        /// 利用SCWS进行中文分词
        /// 1638988@gmail.com
        /// </summary>
        /// <param name="str">需要分词的字符串</param>
        /// <returns>用空格分开的分词结果</returns>
        public static List<string> Parser(string str, int extractWordCount = 2)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                string s = string.Empty;
                System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
                // 将提交的字符串数据转换成字节数组           
                byte[] postData = System.Text.Encoding.ASCII.GetBytes("data=" + System.Web.HttpUtility.UrlEncode(str) + "&respond=json&charset=utf8&ignore=yes&duality=no&traditional=no&multi=0");

                // 设置提交的相关参数
                System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://www.ftphp.com/scws/api.php") as System.Net.HttpWebRequest;
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cookieContainer;
                request.ContentLength = postData.Length;

                // 提交请求数据
                System.IO.Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                // 接收返回的页面
                System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                System.IO.Stream responseStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                string val = reader.ReadToEnd();

                JavaScriptSerializer jss = new JavaScriptSerializer();
                SCWSModel model = null;
                try
                {
                    model = jss.Deserialize<SCWSModel>(val);
                }
                catch (Exception)
                {
                    return null;
                }

                if (model.status.ToLower() != "ok" || model.words == null || model.words.Length <= 0) return null;

                List<string> list = new List<string>();
                foreach (var item in model.words)
                {
                    if (item.idf <= 0) continue;
                    list.Add(item.word);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
