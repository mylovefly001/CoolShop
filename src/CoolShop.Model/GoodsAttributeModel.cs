using System;
using FreeSql.DataAnnotations;

namespace CoolShop.Model
{
    [Table(Name = "goods_attribute")]
    public class GoodsAttributeModel
    {
        [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [Column(Name = "description")] public string Description { get; set; } = string.Empty;

        [Column(Name = "tid")] public int Tid { get; set; }

        [Column(Name = "pid")] public int Pid { get; set; }

        [Column(Name = "icon")] public string Icon { get; set; } = string.Empty;

        [Column(Name = "sort")] public int Sort { get; set; }

        /// <summary>
        /// 输入方式：1=手填|2=单选|3=多选
        /// </summary>
        [Column(Name = "input_mode")]
        public int InputMode { get; set; }

        [Column(Name = "input_val")] public string InputVal { get; set; } = string.Empty;

        /// <summary>
        /// 状态：0=禁用|1=启用
        /// </summary>
        [Column(Name = "status")]
        public int Status { get; set; }

        [Column(Name = "created_time", CanInsert = false, CanUpdate = false)]
        public DateTime CreatedTime { get; set; } = DateTime.Now.ToLocalTime();


        [Column(Name = "updated_time", CanInsert = false, CanUpdate = false)]
        public DateTime UpdatedTime { get; set; } = DateTime.Now.ToLocalTime();
    }
}