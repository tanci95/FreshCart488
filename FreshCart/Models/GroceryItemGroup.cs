using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Models
{
    public class GroceryItemGroup : ObservableCollection<GroceryItem>
    {
        public string Category { get; set; }

        public GroceryItemGroup(string category, IEnumerable<GroceryItem> items) : base(items)
        {
            Category = category;
        }

    }
}
