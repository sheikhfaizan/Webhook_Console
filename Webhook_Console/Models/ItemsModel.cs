using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webhook_Console.Models
{
    internal class ItemsModel
    {
        public class Board
        {
            public ItemsPage Items_Page { get; set; }
        }

        public List<Board> Boards { get; set; }

        public class Item
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class ItemsPage
        {
            public object Cursor { get; set; }
            public List<Item> Items { get; set; }
        }

    }
}
