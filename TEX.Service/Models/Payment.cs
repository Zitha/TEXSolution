using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TEX.Model;

namespace TEX.Service.Controllers
{
    public class Payment
    {
        //public static bool ProcessPayment(Order order)
        public static bool ProcessPayment()
        {
            try
            {
                // Create the order in your DB and get the ID
                //decimal amount = order.TotalPrice;
                //string orderReference = order.Reference;
                //string name = "TeamExpansion, Order #" + orderReference;
                //string description = "Online purchase payment";

                //string site = "";
                //string merchant_id = "";
                //string merchant_key = "";


                //string paymentMode = System.Configuration.ConfigurationManager.AppSettings["PaymentMode"];
                //if (paymentMode == "test")
                //{
                //    site = "https://sandbox.payfast.co.za/eng/process?";
                //    merchant_id = "10000100";
                //    merchant_key = "46f0cd694581a";
                //}
                //else if (paymentMode == "live")
                //{
                //    site = "https://www.payfast.co.za/eng/process?";
                //    merchant_id = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantID"];
                //    merchant_key = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantKey"];
                //}

                //// Build the query string for payment site

                //StringBuilder str = new StringBuilder();
                //str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
                //str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
                //str.Append("&return_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_ReturnURL"]));
                //str.Append("&cancel_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_CancelURL"]));
                //str.Append("&notify_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_NotifyURL"]));

                //str.Append("&m_payment_id=" + HttpUtility.UrlEncode(orderReference));
                //str.Append("&amount=" + HttpUtility.UrlEncode(string.Format("{0}", amount)));
                //str.Append("&item_name=" + HttpUtility.UrlEncode(name));
                //str.Append("&item_description=" + HttpUtility.UrlEncode(description));

                // Redirect to PayFast

                string url = "https://localhost:44305/Templates/ReportPage.html";

                System.Uri uri = new System.Uri(url);
                //Redirect(uri); HttpResponse.
                System.Web.HttpContext.Current.Response.Redirect("http://www.abcmvc.com");

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool ProcessSIDPayment()
        {

            return true;
        }
    }
}