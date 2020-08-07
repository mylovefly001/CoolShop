using System;
using FreeSql.DataAnnotations;

namespace CoolShop.Model
{
    [Table(Name = "sys_admin")]
    public class SysAdminModel
    {
        [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 管理组ID
        /// </summary>
        [Column(Name = "group_id")]
        public int GroupId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(Name = "username")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Column(Name = "password")]
        public string PassWord { get; set; } = string.Empty;

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Column(Name = "real_name")]
        public string RealName { get; set; } = string.Empty;

        [Column(Name = "salt")] public string Salt { get; set; } = string.Empty;

        /// <summary>
        /// 网站状态：0=禁用|1=启用
        /// </summary>
        [Column(Name = "status")]
        public int Status { get; set; }

        [Column(Name = "created_time", CanInsert = false, CanUpdate = false)]
        public DateTime CreatedTime { get; set; } = DateTime.Now.ToLocalTime();

        [Column(Name = "updated_time", CanInsert = false, CanUpdate = false)]
        public DateTime UpdatedTime { get; set; } = DateTime.Now.ToLocalTime();
    }
}