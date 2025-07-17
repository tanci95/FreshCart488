using System.Collections.ObjectModel;
using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FreshCart.Models
{
    public class GroceryItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Category { get; set; }
    }
}


