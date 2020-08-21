using System;
using FreeSql.DataAnnotations;

namespace CoolShop.Model
{
    [Table(Name = "sys_config_upload")]
    public class SysConfigUploadModel
    {
        [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 方案名称
        /// </summary>
        [Column(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [Column(Name = "detail")] public string Detail { get; set; } = string.Empty;


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