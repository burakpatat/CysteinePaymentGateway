using System;

namespace CysteinePaymentGateway.VPOS.Iyzico
{
    internal interface RequestStringConvertible
    {
        String ToPKIRequestString();
    }
}
