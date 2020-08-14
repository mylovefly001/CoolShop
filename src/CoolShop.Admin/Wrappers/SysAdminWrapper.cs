namespace CoolShop.Admin.Wrappers
{
    public class SysAdminWrapper
    {
        public int Id { get; set; }

        public string Cmd { get; set; }

        /// <summary>
        /// 管理组ID
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; } = string.Empty;

        public string SurePass { get; set; } = string.Empty;

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; } = string.Empty;

        /// <summary>
        /// 网站状态：0=禁用|1=启用
        /// </summary>
        public int Status { get; set; }
    }
}