using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace QIQU.Tools
{
    /// <summary>
    /// 生成验证码，返回生成后的字节
    /// </summary>
    public class CodeImage
    {
        //产生四位的随机字符串
        private string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                code = (char)('0' + (char)(number % 10));
                checkCode += code.ToString();
            }
            //Session["CheckCode"] = checkCode;//用于客户端校验码比较

            return checkCode;
        }

        //将验证码生成图片显示
        public byte[] CreateCheckCodeImage(out string checkCode)
        {
            checkCode = this.GenerateCheckCode();
            Bitmap image = new Bitmap(55, 20);
            Graphics g = Graphics.FromImage(image);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            try
            {
                //生成随机生成器 
                Random random = new Random();

                //清空图片背景色 
                g.Clear(Color.White);

                //画图片的背景噪音线 
                for (int i = 0; i < 8; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255))
                        ), x1, y1, x2, y2);
                }

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                List<FontStyleExt> a = GetColorList;
                for (int i = 0; i < checkCode.Length; i++)
                {
                    FontStyleExt Ftyle = GetColor(a);
                    Font font = new Font("Verdana", Ftyle.FontSize, (System.Drawing.FontStyle.Bold));
                    SolidBrush brush = new SolidBrush(Ftyle.FontColor);
                    g.DrawString(checkCode.Substring(i, 1), font, brush, GetCodeRect(i), sf);
                }
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                //Response.ClearContent();
                //Response.ContentType = "image/Gif";
                //Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
            return ms.ToArray();
        }

        /// <summary>
        /// 从颜色列表中随机选取颜色
        /// </summary>
        /// <param name="Color_L"></param>
        /// <returns></returns>
        private FontStyleExt GetColor(List<FontStyleExt> Color_L)
        {
            Random rnd = new Random();
            int i = rnd.Next(0, Color_L.Count);
            FontStyleExt l = Color_L[i];
            Color_L.RemoveAt(i);
            return l;
        }

        /// <summary>
        /// 获取颜色列表
        /// </summary>
        private List<FontStyleExt> GetColorList
        {
            get
            {
                List<FontStyleExt> a = new List<FontStyleExt>(4);
                a.Add(new FontStyleExt(Color.Red, 12));
                a.Add(new FontStyleExt(Color.Green, 12));
                a.Add(new FontStyleExt(Color.Blue, 12));
                a.Add(new FontStyleExt(Color.Black, 12));
                return a;
            }
        }

        /// <summary>
        /// 获取单个字符的绘制区域
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Rectangle GetCodeRect(int index)
        {
            // 计算一个字符应该分配有多宽的绘制区域（等分为CodeLength份）
            int subWidth = 55 / 4;
            // 计算该字符左边的位置
            int subLeftPosition = subWidth * index;

            return new Rectangle(subLeftPosition + 1, 1, subWidth, 20);
        }
    }

    /// <summary>
    /// 字体类
    /// </summary>
    public class FontStyleExt
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FontColor">颜色</param>
        /// <param name="FontSize">字体大小</param>
        public FontStyleExt(Color FontColor, int FontSize)
        {
            _FontColor = FontColor;
            _FontSize = FontSize;
        }
        private Color _FontColor;
        private int _FontSize;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor
        {
            get
            {
                return _FontColor;
            }
            set
            {
                _FontColor = value;
            }
        }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize
        {
            get
            {
                return _FontSize;
            }
            set
            {
                _FontSize = value;
            }
        }
    }
}
