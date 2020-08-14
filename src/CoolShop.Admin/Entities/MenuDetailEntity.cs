using System.Collections.Generic;

namespace CoolShop.Admin.Entities
{
    public class MenuDetailEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; } = string.Empty;

        /// <summary>
        /// 菜单Ctr
        /// </summary>
        public string MenuCtr { get; set; } = string.Empty;

        /// <summary>
        /// 菜单Act
        /// </summary>
        public string MenuAct { get; set; } = string.Empty;

        public int MenuPid { get; set; }

        public string MenuKey { get; set; } = string.Empty;

        /// <summary>
        /// 菜单类型
        /// </summary>
        public int MenuType { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; } = string.Empty;

        public List<MenuDetailEntity> MenuDetail { get; set; } = new List<MenuDetailEntity>();
    }
}