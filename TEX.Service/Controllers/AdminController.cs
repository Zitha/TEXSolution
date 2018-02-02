using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TEX.Model;
using Text.Data;

namespace TEX.Service.Controllers
{
    public class AdminController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Item UpdateItem(Item item)
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                Item updateItem = unit.Items.GetAll().FirstOrDefault(it => it.Id == item.Id);

                updateItem.Image = item.Image;
                updateItem.ItemType = item.ItemType;
                updateItem.Price = item.Price;
                updateItem.Name = item.Name;
                updateItem.Description = item.Description;
                updateItem.SmallItemInStock = item.SmallItemInStock;
                updateItem.MediumItemInStock = item.MediumItemInStock;
                updateItem.LargeItemInStock = item.LargeItemInStock;
                updateItem.XLardtemInStock = item.XLardtemInStock;
                updateItem.XXLargeItemInStock = item.XXLargeItemInStock;

                unit.Items.Update(updateItem);
                unit.SaveChanges();
                return updateItem;
            }
        }
    }
}
