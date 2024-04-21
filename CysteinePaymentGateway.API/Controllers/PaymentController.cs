using CysteinePaymentGateway.VPOS;
using CysteinePaymentGateway.VPOS.Interfaces;
using CysteinePaymentGateway.VPOS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CysteinePaymentGateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public ICysteineVirtualPOSService _cysteineVirtualPOSService;

        public PaymentController(ICysteineVirtualPOSService cysteineVirtualPOSService)
        {
            _cysteineVirtualPOSService = cysteineVirtualPOSService;
        }

        [HttpPost]
        public async Task<IActionResult> Payment()
        {
            VirtualPOSAuth iyzico = new VirtualPOSAuth
            {
                merchantID = "3397112",
                bankCode = CysteinePaymentGateway.VPOS.Services.BankService.Iyzico,
                merchantUser = "sandbox-eTCk7RCkvL5clWgy4Yd66hXeR3nHaiB7",
                merchantPassword = "sandbox-m13JCIzqQHCrWDsjqihA2Z2NDDixle1s",
                testPlatform = true
            };

            CustomerInfo customerInfo = new CustomerInfo
            {
                taxNumber = "1111111111",
                emailAddress = "test@test.com",
                name = "burak",
                surname = "patat",
                phoneNumber = "1111111111",
                addressDesc = "adres",
                cityName = "istanbul",
                country = CysteinePaymentGateway.VPOS.Enums.Country.TUR,
                postCode = "34000",
                taxOffice = "Umraniye",
                townName = "Umraniye"
            };

            SaleRequest saleRequest = new SaleRequest
            {
                invoiceInfo = customerInfo,
                shippingInfo = customerInfo,
                saleInfo = new SaleInfo
                {
                    cardNameSurname = "burak test",
                    cardNumber = "5890040000000016",
                    cardExpiryDateMonth = 12,
                    cardExpiryDateYear = 2030,
                    amount = (decimal)100.50,
                    cardCVV = "000",
                    currency = CysteinePaymentGateway.VPOS.Enums.Currency.TRY,
                    installment = 1,
                },
                payment3D = new Payment3D
                {
                    confirm = true,
                    returnURL = "https://localhost:7042/api/Payment/VirtualPOS3DResponse"
                },
                customerIPAddress = "1.1.1.1",
                orderNumber = Convert.ToInt32((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds).ToString("X")
            };


            var resp = _cysteineVirtualPOSService.Sale(saleRequest, iyzico);

            return Ok(resp.message);
        }

        [HttpPost("VirtualPOS3DResponse")]
        public async Task<IActionResult> VirtualPOS3DResponse([FromForm] IFormCollection collections)
        {
            Dictionary<string, object> form = collections.Keys.ToDictionary(k => k, v => (object)collections[v]);

            VirtualPOSAuth iyzico = new VirtualPOSAuth
            {
                merchantID = "3397112",
                bankCode = CysteinePaymentGateway.VPOS.Services.BankService.Iyzico,
                merchantUser = "sandbox-eTCk7RCkvL5clWgy4Yd66hXeR3nHaiB7",
                merchantPassword = "sandbox-m13JCIzqQHCrWDsjqihA2Z2NDDixle1s",
                testPlatform = true
            };

            SaleResponse response = _cysteineVirtualPOSService.Sale3DResponse(new Sale3DResponseRequest
            { 
                responseArray = form
            }, iyzico);

            return Ok(response.message);
        }
    }
}
