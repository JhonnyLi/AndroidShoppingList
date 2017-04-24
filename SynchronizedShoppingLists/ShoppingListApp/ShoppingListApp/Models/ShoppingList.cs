using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListApp.Models
{
    public class ShoppingList
    {
        public Guid ShoppingListId { get; set; }
        public string Name { get; set; }
        public virtual List<Item> Items { get; set; } = new List<Item>();
    }
}
