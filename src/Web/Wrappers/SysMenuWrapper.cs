using System;

namespace CoolShop.Web.Wrappers
{
    public class SysMenuWrapper
    {
        public int Id { get; set; }
        public string Cmd { get; set; } = string.Empty;
        public string MenuName { get; set; } = string.Empty;
        public int MenuGroupId { get; set; }
        public int MenuType { get; set; }
        public string MenuCtr { get; set; } = string.Empty;
        public string MenuAct { get; set; } = string.Empty;
        public int MenuSort { get; set; }
        public string MenuIcon { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}