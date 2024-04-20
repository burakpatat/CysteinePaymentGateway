﻿using CysteinePaymentGateway.VPOS.Iyzico.Request;
using System.Collections.Generic;

namespace CysteinePaymentGateway.VPOS.Iyzico.Model
{
    internal class InstallmentInfo : IyzipayResource
    {
        public List<InstallmentDetail> InstallmentDetails { get; set; }

        public static InstallmentInfo Retrieve(RetrieveInstallmentInfoRequest request, Options options)
        {
            return RestHttpClient.Create().Post<InstallmentInfo>(options.BaseUrl + "/payment/iyzipos/installment", GetHttpHeaders(request, options), request);
        }
    }
}
