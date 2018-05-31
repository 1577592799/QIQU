using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QIQU.Tools
{
    public class EmailAction
    {
        const string systemEmail = "mail@umould.net";
        const string systemEmailPwd = "um123456";
        const string systemName = "优摩管家";

        public int smtpPort = 25;
        public string smtpHost = "smtp.exmail.qq.com";
        public bool isSSL = false;

        public string errorInfo;

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="title">邮件主题</param>
        /// <param name="content">发送的内容</param>
        /// <param name="receiveEmail">接收邮件</param>
        /// <returns></returns>
        public bool SendEmail(string title, string content, string receiveEmail)
        {
            MailAddress from = new MailAddress(systemEmail, systemName);
            MailAddress to = new MailAddress(receiveEmail);

            MailMessage msg = new MailMessage(from, to);
            msg.BodyEncoding = Encoding.UTF8;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Subject = title;
            msg.Body = content;
            msg.Priority = MailPriority.Normal;

            SmtpClient client = new SmtpClient(smtpHost);
            if (smtpPort != 0)
            {
                client.Port = smtpPort;
            }
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(systemEmail, systemEmailPwd);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = isSSL;

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
            return true;
        }
    }
}
