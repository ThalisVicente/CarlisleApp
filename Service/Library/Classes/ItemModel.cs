using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Library.Classes
{
    public class ItemModel
    {
        public ItemModel()
        {
            Attributes = new List<AttributeModel>();
        }
        public int Id { get; set; }
        public List<AttributeModel> Attributes { get; set; }
        public string AttributesText
        {
            get
            {
                if(Attributes.Any())
                    return string.Join(", ", Attributes.Select(a => a.Title).ToList()).ToUpper();
                else
                    return "NO ATTRIBUTE LINKED TO THIS ITEM";
            }
        }
        
    }
}
