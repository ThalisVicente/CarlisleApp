using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Classes
{
    public class Item
    {
        public Item()
        {
            Attributes = new List<Attribute>();
        }
        public int Id { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string AttributesText => string.Join(", ", Attributes.Select(a => a.Title).ToList());
    }
}
