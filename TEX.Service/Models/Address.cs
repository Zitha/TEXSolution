using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEXWebservice.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string ContactNumber { get; set; }
        public string BuildingName { get; set; }
        public string StreetAddress { get; set; }
        public string Surburb { get; set; }
        public string CityTown { get; set; }
        public string PostalCode { get; set; }
    }
}