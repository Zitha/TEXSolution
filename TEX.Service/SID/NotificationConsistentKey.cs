using System.Security.Cryptography;
using System.Text;

namespace TEX.Service.Controllers
{
    public class NotificationConsistentKey
    {
        /// <summary>
        /// A Method used to Create a SID Notification Consistent Key - .NET and Classic ASP
        /// </summary>
        /// <param name="STATUS">The Status of the Transaction (COMPLETED, CANCELLED)</param>
        /// <param name="MERCHANT">Your merchant code</param>
        /// <param name="CURRENCY">ZAR</param>
        /// <param name="COUNTRY">ZA</param>
        /// <param name="REFERENCE">Your reference number</param>
        /// <param name="AMOUNT">The amount</param>
        /// <param name="BANK">The Aquirer Name (E.g.: ABSA)</param>
        /// <param name="DATE">The date the transaction was created</param>
        /// <param name="RECEIPT">The receipt number of only COMPLETED transactions</param>
        /// <param name="TNXID">The SID transaction ID</param>
        /// <param name="CUSTOM1">1st Custom Field - If users only use some of the fields, empty strings or null can be passed</param>
        /// <param name="CUSTOM2">2nd Custom Field</param>
        /// <param name="CUSTOM3">3rd Custom Field</param>
        /// <param name="CUSTOM4">4th Custom Field</param>
        /// <param name="CUSTOM5">5th Custom Field</param>
        /// <param name="PRIVATE_KEY">Your merchants private key</param>
        /// <returns></returns>
        public string ReturnURLConsistent(string STATUS, string MERCHANT, string CURRENCY, string COUNTRY, string REFERENCE, string AMOUNT, string BANK, string DATE, string RECEIPT, string TNXID, string CUSTOM1, string CUSTOM2, string CUSTOM3, string CUSTOM4, string CUSTOM5, string PRIVATE_KEY)
        {
            SHA512 SHA512HashCreator = SHA512.Create();

            #region Custom Field Null Checking

            if (CUSTOM1 == null)
                CUSTOM1 = string.Empty;
            if (CUSTOM2 == null)
                CUSTOM2 = string.Empty;
            if (CUSTOM3 == null)
                CUSTOM3 = string.Empty;
            if (CUSTOM4 == null)
                CUSTOM4 = string.Empty;
            if (CUSTOM5 == null)
                CUSTOM5 = string.Empty;

            #endregion

            StringBuilder concatenatedString = new StringBuilder();

            concatenatedString.Append(STATUS);
            concatenatedString.Append(MERCHANT);
            concatenatedString.Append(COUNTRY);
            concatenatedString.Append(CURRENCY);
            concatenatedString.Append(REFERENCE);
            concatenatedString.Append(AMOUNT);
            concatenatedString.Append(BANK);
            concatenatedString.Append(DATE);
            concatenatedString.Append(RECEIPT);
            concatenatedString.Append(TNXID);
            concatenatedString.Append(CUSTOM1);
            concatenatedString.Append(CUSTOM2);
            concatenatedString.Append(CUSTOM3);
            concatenatedString.Append(CUSTOM4);
            concatenatedString.Append(CUSTOM5);
            concatenatedString.Append(PRIVATE_KEY);

            byte[] EncryptedData = SHA512HashCreator.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString.ToString()));

            StringBuilder CONSISTENT_KEY = new StringBuilder();

            for (int i = 0; i < EncryptedData.Length; i++)
            {
                CONSISTENT_KEY.Append(EncryptedData[i].ToString("X2"));
            }

            return CONSISTENT_KEY.ToString().ToUpper();
        }
    }
}