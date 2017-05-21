using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListApp.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Comment { get; set; }
        public bool Deleted { get; set; }
        public bool Editable { get; set; } = true;

    }
}
