using System.Collections.Generic;

namespace CoolShop.Admin.Entities
{
    public class MenuTreeEntity
    {
        
        public int Id { get; set; }
        
        public int Pid { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public int Status { get; set; }

        public int Type { get; set; }

        public List<MenuTreeEntity> Child = new List<MenuTreeEntity>();
    }
}