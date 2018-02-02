using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEX.Model;
using TEX.PDFGenerator;
using Text.Data;

namespace TEX.Service.Controllers
{
    public class OrderController : System.Web.Http.ApiController
    {
        InvoiceGenerator invoice = new InvoiceGenerator();
        [HttpPost]
        public OrderReturn CreateOrder(Order order)
        {
            try
            {
                using (ApplicationUnit unit = new ApplicationUnit())
                {
                    EmailService emailService = new EmailService();

                    User loginUser = unit.Users.GetAll().Include(a => a.Address)
                        .FirstOrDefault(user => user.Email.ToLower() == order.User.Email.ToLower());

                    if (loginUser.Address != null)
                    {
                        order.Address = loginUser.Address;
                    }
                    else
                    {
                        loginUser.Address = order.Address;
                        unit.Users.Update(loginUser);
                        unit.SaveChanges();
                    }
                    //Update order counts
                    foreach (var orderItem in order.OrderItems)
                    {
                        unit.Items.Update(orderItem.Item);
                        unit.SaveChanges();
                    }
                    //Add Order
                    order.OrderDate = DateTime.Now;
                    order.User = loginUser;
                    order.Status = "Awaiting Delivery";
                    order.Reference = string.Format("TEX-{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));

                    //Generate Invoice
                    string invoicePath = invoice.GenerateInvoice(order);

                    order.InvoiceLocation = invoicePath;
                    unit.Orders.Add(order);
                    unit.SaveChanges();

                    //Send Email with Invoice
                    string emailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailTemplates/OrderPlacedEmail.html"));
                    Task sendEmailTask = new Task(() => emailService.SendOrderPurchaseEmail(order.User.Email, order.Reference, invoicePath, loginUser.FirstName, loginUser.LastName, emailBody));
                    // Start the task.
                    sendEmailTask.Start();
                }
            }
            catch (Exception ex)
            {
                var outputLines = new List<string>();
                outputLines.Add(ex.Message);
                File.AppendAllLines(@"c:\errors.txt", outputLines);
                throw;
            }

            return new OrderReturn { Reference = order.Reference, };
        }

        [HttpGet]
        public List<Order> GetCutomerOrders(int customerId)
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                return unit.Orders.GetAll().Where(or => or.User.Id == customerId).ToList();
            }
        }

        //[HttpGet]
        //public byte[] GetOrderInvoice(string reference)
        //{
        //    using (ApplicationUnit unit = new ApplicationUnit())
        //    {
        //        var invoice = unit.Orders.GetAll().Where(or => or.Reference == reference).FirstOrDefault();


        //        return File.ReadAllBytes(invoice.InvoiceLocation); ;
        //    }
        //}

        [HttpGet]
        public List<OrdersReturn> GetOrdersByStatus(string status)
        {
            List<OrdersReturn> orderReturn = new List<OrdersReturn>();
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                try
                {
                    // var pendingOrders = unit.Orders.GetAll().Include(ad => ad.Address)
                    //.Include(us => us.User).Include(d => d.OrderItems)
                    //.Where(or => or.Status == status).ToList();

                    var pendingOrders = unit.Orders.GetAll().Include(ad => ad.Address)
                   .Include(us => us.User).Include(d => d.OrderItems)
                          .Include(co => co.OrderItems.Select(emp => emp.Item))
                   .Where(or => or.Status == status).ToList();
                    foreach (var order in pendingOrders)
                    {
                        orderReturn.Add(new OrdersReturn
                        {
                            Id=order.Id,
                            OrderDate = order.OrderDate,
                            OrderItems = order.OrderItems,
                            Reference = order.Reference,
                            Status = order.Status,
                            TotalPrice = order.TotalPrice,
                            User = new User
                            {
                                Id = order.User.Id,
                                Email = order.User.Email,
                                FirstName = order.User.FirstName,
                                LastName = order.User.LastName
                            },
                            Address = new Address
                            {
                                Id = order.Address.Id,
                                BuildingName = order.Address.BuildingName,
                                CityTown = order.Address.CityTown,
                                ContactNumber = order.Address.ContactNumber,
                                PostalCode = order.Address.PostalCode,
                                StreetAddress = order.Address.StreetAddress,
                                Surburb = order.Address.Surburb
                            }
                        });
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                return orderReturn;
            }
        }

        [HttpPost]
        public bool UpdateOrderStatus(Order order)
        {
            using (ApplicationUnit unit = new ApplicationUnit())
            {
                Order pendingOrder = unit.Orders.GetAll()
                    .Where(or => or.Id == order.Id).FirstOrDefault();

                pendingOrder.Status = order.Status;

                unit.Orders.Update(pendingOrder);
                unit.SaveChanges();
                return true;
            }
        }

        //[HttpGet]
        //public string GetConsistentKey(string reference, decimal amount)
        //{
        //    BuyButtonConsistentField key = new BuyButtonConsistentField();

        //    return key.GetConsistentKey(reference, amount);
        //}
    }

    public class OrderReturn
    {
        public string Reference { get; set; }
    }


    public class OrdersReturn
    {
        public string Reference { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public User User { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Address Address { get; set; }
        public int Id { get; internal set; }
    }

}