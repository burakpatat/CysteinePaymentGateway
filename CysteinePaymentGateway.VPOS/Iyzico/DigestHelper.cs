using System;
using System.Text;

namespace CysteinePaymentGateway.VPOS.Iyzico
{
    internal sealed class DigestHelper
    {
        private DigestHelper()
        {
        }

        public static String DecodeString(String content)
        {
            return (!String.IsNullOrEmpty(content)) ? Encoding.UTF8.GetString(Convert.FromBase64String(content)) : null;
        }
    }
}
