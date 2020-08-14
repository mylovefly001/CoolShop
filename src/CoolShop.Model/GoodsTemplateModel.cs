using System;
using FreeSql.DataAnnotations;

namespace CoolShop.Model
{
    [Table(Name = "goods_template")]
    public class GoodsTemplateModel
    {
        [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(Name = "name")]
        public string Name { get; set; } = string.Empty;


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