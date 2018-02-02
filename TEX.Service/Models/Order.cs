using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEXWebservice.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public List<Item> Items { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal ItemTotal { get; set; }

        public string Status { get; set; }

        public string Reference { get; set; }

        public decimal ShippingCost { get; set; }

        public Address Address { get; set; }

        public User User { get; set; }
    }
}