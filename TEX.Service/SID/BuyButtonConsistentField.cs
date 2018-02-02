using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace TEX.Service.Controllers
{
    public class BuyButtonConsistentField
    {
        public string GetConsistentKey(string reference, decimal amount)
        {
            string MERCHANT = ConfigurationManager.AppSettings["SIDMerchantcode"]
                 , CURRENCY = "ZAR"
                 , COUNTRY = "ZA"
                 , REFERENCE = reference
                 , AMOUNT = string.Format("{0}", amount)
                 , PRIVATEKEY = ConfigurationManager.AppSettings["SIDPrivateKey"];

            SHA512 SHA512HashCreator = SHA512.Create();

            MD5 mD5 = MD5.Create();

            StringBuilder concatenatedString = new StringBuilder();
            concatenatedString.Append(MERCHANT);
            concatenatedString.Append(CURRENCY);
            concatenatedString.Append(COUNTRY);
            concatenatedString.Append(REFERENCE);
            concatenatedString.Append(AMOUNT);
            concatenatedString.Append(PRIVATEKEY);

            byte[] EncryptedData = SHA512HashCreator.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString.ToString()));

            StringBuilder CONSISTENT_KEY = new StringBuilder();

            for (int i = 0; i < EncryptedData.Length; i++)
            {
                CONSISTENT_KEY.Append(EncryptedData[i].ToString("X2"));
            }
            var FINAL_CONSISTENT_KEY = CONSISTENT_KEY.ToString().ToUpper();

            return FINAL_CONSISTENT_KEY;
        }
    }
}