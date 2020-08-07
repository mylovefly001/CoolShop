using System;
using FreeSql.DataAnnotations;

namespace CoolShop.Model
{
    [Table(Name = "sys_menu")]
    public class SysMenuModel
    {
        [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 父级菜单ID
        /// </summary>
        [Column(Name = "pid")]
        public int Pid { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Column(Name = "name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 菜单图标
        /// </summary>
        [Column(Name = "icon")]
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 菜单排序,倒序
        /// </summary>
        [Column(Name = "sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 菜单类型：1=目录|2=菜单
        /// </summary>
        [Column(Name = "type")]
        public int Type { get; set; }

        [Column(Name = "mctr")] public string Mctr { get; set; } = string.Empty;

        [Column(Name = "mact")] public string Mact { get; set; } = string.Empty;

        /// <summary>
        /// 菜单KEY
        /// </summary>
        [Column(Name = "mkey")]
        public string Mkey { get; set; } = string.Empty;

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