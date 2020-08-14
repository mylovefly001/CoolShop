namespace CoolShop.Admin.Wrappers
{
    public class SysAdminGroupWrapper
    {
        public int Id { get; set; }
        public string Cmd { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}