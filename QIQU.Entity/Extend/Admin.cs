using System;

namespace QIQU.Entity.Extend
{
    public class Admin
    {
        public AdminLoginStatus LoginStatus { get; set; }
        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public string RealName { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public enum AdminLoginStatus
    {
        Success = 0,
        NoExists = 1,
        PasswordError = 2,
        Other = 3,
    }
}
