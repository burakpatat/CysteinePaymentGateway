using CysteinePaymentGateway.VPOS.Iyzico.Request;

namespace CysteinePaymentGateway.VPOS.Iyzico.Model
{
    internal class ThreedsPayment : PaymentResource
    {
        public static ThreedsPayment Create(CreateThreedsPaymentRequest request, Options options)
        {
            return RestHttpClient.Create().Post<ThreedsPayment>(options.BaseUrl + "/payment/3dsecure/auth", GetHttpHeaders(request, options), request);
        }
    }
}
