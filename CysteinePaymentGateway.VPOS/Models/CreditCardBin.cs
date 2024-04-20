﻿using CysteinePaymentGateway.VPOS.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CysteinePaymentGateway.VPOS.Models
{
    public class CreditCardBinQueryRequest : ModelValidation
    {
        [Required(ErrorMessage = "binNumber alanı zorunludur")]
        [StringLength(maximumLength: 8, MinimumLength = 6, ErrorMessage = "binNumber alanı kart numarasının ilk 6 veya 8 hanesi olması gerekmektedir")]
        [RegularExpression(pattern: "[0-9]+", ErrorMessage = "binNumber alanı sadece rakamlardan oluşmalıdır")]
        public string binNumber { get; set; }
    }

    public class CreditCardBinQueryResponse
    {
        /// <summary>
        /// Kredi kartı 6 haneli bin numarası
        /// </summary>
        public string binNumber { get; set; }
        /// <summary>
        /// Banka kodu
        /// </summary>
        public string bankCode { get; set; }
        /// <summary>
        /// Kart tipi
        /// </summary>
        public CreditCardType cardType { get; set; }
        /// <summary>
        /// Kart markası
        /// </summary>
        public CreditCardBrand cardBrand { get; set; }
        /// <summary>
        /// Ticari kart
        /// </summary>
        public bool commercialCard { get; set; }
        /// <summary>
        /// Kart program
        /// </summary>
        public CreditCardProgram cardProgram { get; set; }
        /// <summary>
        /// Taksit yapabilen bankalar
        /// </summary>
        public List<string> banksWithInstallments { get; set; }
    }
}

