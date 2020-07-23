using System.Collections.Generic;

namespace CoolShop.Core.Entity
{
    public class MenuTreeEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public int Pid { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 菜单排序,倒序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 菜单类型：1=目录|2=菜单
        /// </summary>
        public int Type { get; set; }

        public string Mctr { get; set; } = string.Empty;

        public string Mact { get; set; } = string.Empty;

        /// <summary>
        /// 菜单KEY
        /// </summary>
        public string Mkey { get; set; } = string.Empty;

        /// <summary>
        /// 网站状态：0=禁用|1=启用
        /// </summary>
        public int Status { get; set; }

        public List<MenuTreeEntity> Child { get; set; } = new List<MenuTreeEntity>();
    }
}