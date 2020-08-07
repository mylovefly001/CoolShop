namespace CoolShop.Admin.Wrappers
{
    public class SysMenuWrapper
    {
        public int Id { get; set; }
        public string Cmd { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Pid { get; set; }
        public int Type { get; set; }
        public string Mctr { get; set; } = string.Empty;
        public string Mact { get; set; } = string.Empty;
        public int Sort { get; set; }
        public string Icon { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}