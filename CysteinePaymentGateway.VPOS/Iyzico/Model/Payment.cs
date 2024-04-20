using CysteinePaymentGateway.VPOS.Iyzico.Request;

namespace CysteinePaymentGateway.VPOS.Iyzico.Model
{
    internal class Payment : PaymentResource
    {
        public static Payment Create(CreatePaymentRequest request, Options options)
        {
            return RestHttpClient.Create().Post<Payment>(options.BaseUrl + "/payment/auth", GetHttpHeaders(request, options), request);
        }
    }
}
