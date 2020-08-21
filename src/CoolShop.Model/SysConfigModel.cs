using System;
using FreeSql.DataAnnotations;

namespace CoolShop.Model
{
    [Table(Name = "sys_config")]
    public class SysConfigModel
    {
        [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 上传方案ID
        /// </summary>
        [Column(Name = "upload_id")]
        public int UploadId { get; set; }

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