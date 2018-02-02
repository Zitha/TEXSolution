using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEXWebservice.Models
{
    public class Item
    {

        public int Id { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ItemType ItemType { get; set; }

        public decimal Price { get; set; }

        public int ItemInStock { get; set; }
    }
}