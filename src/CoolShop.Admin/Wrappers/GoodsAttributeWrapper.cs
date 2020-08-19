namespace CoolShop.Admin.Wrappers
{
    public class GoodsAttributeWrapper
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Tid { get; set; }

        public int Pid { get; set; }

        public string Icon { get; set; } = string.Empty;

        public int Sort { get; set; }

        public int InputMode { get; set; }

        public string InputVal { get; set; } = string.Empty;

        public int Status { get; set; }
    }
}