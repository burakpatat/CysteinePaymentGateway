﻿using CysteinePaymentGateway.VPOS.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CysteinePaymentGateway.VPOS.Models
{
    public class CancelRequest : ModelValidation
    {
        /// <summary>
        /// Ödeme işlemi yapan client IP adresi
        /// </summary>
        [Required(ErrorMessage = "customerIPAddress alanı zorunludur")]
        public string customerIPAddress { get; set; }

        /// <summary>
        /// Sipariş numarası
        /// </summary>
        [Required(ErrorMessage = "orderNumber alanı zorunludur")]
        public string orderNumber { get; set; }

        /// <summary>
        /// Banka tarafındaki işlem numarası
        /// </summary>
        [Required(ErrorMessage = "transactionId alanı zorunludur")]
        public string transactionId { get; set; }
  
        /// <summary>
        /// İptal edilecek tutar para birimi, default = TRY
        /// </summary>
        public Currency? currency { get; set; }
    }

    public class CancelResponse
    {
        /// <summary>
        /// İşlem sonucu
        /// </summary>
        public ResponseStatu statu { get; set; }

        /// <summary>
        /// İşlem sonucu mesajı
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// İade edilen tutar
        /// </summary>
        public decimal? refundAmount { get; set; }
        /// <summary>
        /// Bankalardan verilen cevapların ham datası
        /// </summary>
        public Dictionary<string, object> privateResponse { get; set; }
    }

}
