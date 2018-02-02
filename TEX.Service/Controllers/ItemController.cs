using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TEX.Model;
using Text.Data;

namespace TEX.Service.Controllers
{
    public class ItemController : ApiController
    {
        // GET api/values
        public IEnumerable<Item> GetItemsByType(string itemType)
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                List<Item> items = unit.Items.GetAll()
                    .Include(it => it.ItemType)
                    .Where(i => i.ItemType.Type == itemType).ToList();

                return items;
            }
        }

        public IEnumerable<Item> GetAllItems()
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                List<Item> items = unit.Items.GetAll()
                    .Include(it => it.ItemType)
                    .ToList();
                return items;
            }
        }

        [System.Web.Http.HttpPost]
        public Item AddItem(Item item)
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                unit.Items.Add(item);
                unit.SaveChanges();
                return item;
            }
        }
    }
}