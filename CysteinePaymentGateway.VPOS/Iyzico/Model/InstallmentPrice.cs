using Newtonsoft.Json;
using System;

namespace CysteinePaymentGateway.VPOS.Iyzico.Model
{
    internal class InstallmentPrice
    {
        [JsonProperty(PropertyName = "InstallmentPrice")]
        public String Price { get; set; }
        public String TotalPrice { get; set; }
        public int? InstallmentNumber { get; set; }
    }
}
