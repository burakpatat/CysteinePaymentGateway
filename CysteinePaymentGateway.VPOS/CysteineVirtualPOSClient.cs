using CysteinePaymentGateway.VPOS.Helpers;
using CysteinePaymentGateway.VPOS.Interfaces;
using CysteinePaymentGateway.VPOS.Models;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;


namespace CysteinePaymentGateway.VPOS
{
    public class CysteineVirtualPOSClient : ICysteineVirtualPOSService
    {
        /// <summary>
        /// Karttan çekim yapmak için kullanılır. 3D çekim yapmak için "SaleRequest.payment3D.confirm = true" olarak gönderilmelidir.
        /// </summary>
        /// <param name="request">Kart çekim işlemine ait işlem bilgileri</param>
        /// <param name="auth">Banka API bilgileri</param>
        /// <returns></returns>
        public SaleResponse Sale(SaleRequest request, VirtualPOSAuth auth)
        {
            request.Validate();
            request.saleInfo.Validate();
            request.invoiceInfo.Validate();
            request.shippingInfo.Validate();
            auth.Validate();

            if (!FoundationHelper.IsCardNumberValid(request.saleInfo.cardNumber))
                throw new ValidationException("Geçersiz kart numarası. Lütfen kart numaranızı kontrol ediniz.");

            #region Adres Max Length
            int city_town_length = 25;

            request.invoiceInfo.cityName = request.invoiceInfo.cityName.clearString().getMaxLength(city_town_length);
            request.invoiceInfo.townName = request.invoiceInfo.townName.clearString().getMaxLength(city_town_length);
            request.invoiceInfo.addressDesc = request.invoiceInfo.addressDesc.clearString().getMaxLength(200);
            request.invoiceInfo.postCode = request.invoiceInfo.postCode.clearString().getMaxLength(10);
            request.invoiceInfo.emailAddress = request.invoiceInfo.emailAddress.clearString().getMaxLength(100);
            request.invoiceInfo.phoneNumber = request.invoiceInfo.phoneNumber.clearString().getMaxLength(20);
            request.invoiceInfo.name = request.invoiceInfo.name.clearString().getMaxLength(50);
            request.invoiceInfo.surname = request.invoiceInfo.surname.clearString().getMaxLength(50);
            request.invoiceInfo.taxNumber = request.invoiceInfo.taxNumber.clearString().getMaxLength(20);
            request.invoiceInfo.taxOffice = request.invoiceInfo.taxOffice.clearString().getMaxLength(50);

            request.shippingInfo.cityName = request.shippingInfo.cityName.clearString().getMaxLength(city_town_length);
            request.shippingInfo.townName = request.shippingInfo.townName.clearString().getMaxLength(city_town_length);
            request.shippingInfo.addressDesc = request.shippingInfo.addressDesc.clearString().getMaxLength(200);
            request.shippingInfo.postCode = request.shippingInfo.postCode.clearString().getMaxLength(10);
            request.shippingInfo.emailAddress = request.shippingInfo.emailAddress.clearString().getMaxLength(100);
            request.shippingInfo.phoneNumber = request.shippingInfo.phoneNumber.clearString().getMaxLength(20);
            request.shippingInfo.name = request.shippingInfo.name.clearString().getMaxLength(50);
            request.shippingInfo.surname = request.shippingInfo.surname.clearString().getMaxLength(50);
            request.shippingInfo.taxNumber = request.shippingInfo.taxNumber.clearString().getMaxLength(20);
            request.shippingInfo.taxOffice = request.shippingInfo.taxOffice.clearString().getMaxLength(50);
            #endregion

            request.saleInfo.cardNameSurname = request.saleInfo.cardNameSurname.clearString();

            ICysteineVirtualPOSService vPOSService = GetVirtualPOSService(auth.bankCode);

            return vPOSService.Sale(request, auth);
        }

        /// <summary>
        /// 3D yapılan çekim işlemi sonucunu döner
        /// </summary>
        /// <param name="request"></param>
        public SaleResponse Sale3DResponse(Sale3DResponseRequest request, VirtualPOSAuth auth)
        {
            request.Validate();
            auth.Validate();

            if (auth.bankCode == "0067") // YapıKredi Bankası
            {
                if (request.currency == null)
                    throw new ValidationException("currency alanı Yapı Kredi bankası için zorunludur");
            }

            try
            {
                Dictionary<string, object> _responseArray = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(Newtonsoft.Json.JsonConvert.SerializeObject(request.responseArray));

                foreach (KeyValuePair<string, object> item in _responseArray)
                {
                    if (item.Value != null && item.Value.GetType().Name == "JArray")
                    {
                        JArray jArray = item.Value as JArray;

                        _responseArray[item.Key] = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(Newtonsoft.Json.JsonConvert.SerializeObject(jArray.First));
                    }
                }


                request.responseArray = _responseArray;
            }
            catch
            {

            }

            ICysteineVirtualPOSService vPOSService = GetVirtualPOSService(auth.bankCode);

            return vPOSService.Sale3DResponse(request, auth);
        }

        /// <summary>
        /// Karta yapılabilecek taksit sayısını döner
        /// </summary>
        /// <param name="request">Kart bilgisi</param>
        /// <param name="auth">Banka API bilgileri</param>
        /// <returns></returns>
        public BINInstallmentQueryResponse BINInstallmentQuery(BINInstallmentQueryRequest request, VirtualPOSAuth auth)
        {
            request.Validate();
            auth.Validate();

            ICysteineVirtualPOSService vPOSService = GetVirtualPOSService(auth.bankCode);

            return vPOSService.BINInstallmentQuery(request, auth);
        }

        /// <summary>
        /// Tutar ile taksit sayısını döner
        /// </summary>
        /// <param name="request"></param>
        /// <param name="auth">Banka API bilgileri</param>
        /// <returns></returns>
        public AllInstallmentQueryResponse AllInstallmentQuery(AllInstallmentQueryRequest request, VirtualPOSAuth auth)
        {
            request.Validate();
            auth.Validate();

            ICysteineVirtualPOSService vPOSService = GetVirtualPOSService(auth.bankCode);

            return vPOSService.AllInstallmentQuery(request, auth);
        }

        /// <summary>
        /// Satış yapılabilecek ek taksit kampanyalarını döner
        /// </summary>
        /// <param name="request"></param>
        /// <param name="auth">Banka API bilgileri</param>
        /// <returns></returns>
        public AdditionalInstallmentQueryResponse AdditionalInstallmentQuery(AdditionalInstallmentQueryRequest request, VirtualPOSAuth auth)
        {
            request.Validate();
            request.saleInfo.Validate();
            auth.Validate();

            ICysteineVirtualPOSService vPOSService = GetVirtualPOSService(auth.bankCode);

            return vPOSService.AdditionalInstallmentQuery(request, auth);
        }

        /// <summary>
        ///  Ödeme iptal etme. Aynı gün yapılan ödemeler için kullanılabilir. Çekilen tutarın tamamı iptal edilir ve müşteri ekstresine hiçbir işlem yansımaz.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="auth">Banka API bilgileri</param>
        /// <returns></returns>
        public CancelResponse Cancel(CancelRequest request, VirtualPOSAuth auth)
        {
            request.Validate();
            auth.Validate();

            ICysteineVirtualPOSService vPOSService = GetVirtualPOSService(auth.bankCode);

            return vPOSService.Cancel(request, auth);
        }

        /// <summary>
        /// Ödeme iade etme. Belirtilen tutar kadar kısmi iade işlemi yapılır
        /// </summary>
        /// <param name="request"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public RefundResponse Refund(RefundRequest request, VirtualPOSAuth auth)
        {
            request.Validate();
            auth.Validate();

            ICysteineVirtualPOSService vPOSService = GetVirtualPOSService(auth.bankCode);

            return vPOSService.Refund(request, auth);
        }

        /// <summary>
        /// Tüm banka listesi
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Bank> AllBankList(Func<Bank, bool> where)
        {
            var banks = CysteinePaymentGateway.VPOS.Services.BankService.AllBanks
                .Where(where)
                .Select(s => new Bank
                {
                    BankCode = s.BankCode,
                    BankName = s.BankName,
                    CollectiveVPOS = s.CollectiveVPOS,
                    CommissionAutoAdd = s.CommissionAutoAdd,
                    InstallmentAPI = s.InstallmentAPI,
                }).ToList();

            return banks;
        }

        /// <summary>
        /// Kredi Kartı BIN sorgulama
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //public CreditCardBinQueryResponse CreditCardBinQuery(CreditCardBinQueryRequest request)
        //{
            //request.Validate();

            //List<CreditCardBinQueryResponse> binList = BinService.GetBinList();

            //CreditCardBinQueryResponse _binDetail = binList.FirstOrDefault(s => s.binNumber == request.binNumber.Substring(0, 6));

            //if (_binDetail == null && request.binNumber.Length > 6)
            //    _binDetail = binList.FirstOrDefault(s => s.binNumber == request.binNumber);

            //CreditCardBinQueryResponse binDetail = null;

            //if (_binDetail != null)
            //{
            //    binDetail = _binDetail.DeepClone();

            //    if ((int)binDetail.cardProgram >= 0 && binDetail.cardType == Enums.CreditCardType.Credit)
            //        binDetail.banksWithInstallments = binList.Where(s => s.cardProgram == binDetail.cardProgram).GroupBy(s => s.bankCode).OrderByDescending(s => s.Count()).Select(s => s.Key).ToList();

            //    binDetail.banksWithInstallments = binDetail.banksWithInstallments ?? new List<string>();

            //    if (!binDetail.banksWithInstallments.Any(s => s == binDetail.bankCode))
            //        binDetail.banksWithInstallments.Add(binDetail.bankCode);


            //    if (binDetail.banksWithInstallments.IndexOf(binDetail.bankCode) != 0)
            //    {
            //        binDetail.banksWithInstallments.Remove(binDetail.bankCode);
            //        binDetail.banksWithInstallments.Insert(0, binDetail.bankCode);
            //    }
            //}

            //return binDetail;
        //}

        /// <summary>
        /// Tüm Kart Bilgileri Listesi
        /// </summary>
        //public List<CreditCardBinQueryResponse> AllCreditCardBinList()
        //{
            //List<CreditCardBinQueryResponse> binList = BinService.GetBinList().DeepClone();

            //foreach (CreditCardBinQueryResponse binDetail in binList)
            //{
            //    if ((int)binDetail.cardProgram >= 0 && binDetail.cardType == Enums.CreditCardType.Credit)
            //        binDetail.banksWithInstallments = binList.Where(s => s.cardProgram == binDetail.cardProgram).GroupBy(s => s.bankCode).OrderByDescending(s => s.Count()).Select(s => s.Key).ToList();

            //    binDetail.banksWithInstallments = binDetail.banksWithInstallments ?? new List<string>();

            //    if (!binDetail.banksWithInstallments.Any(s => s == binDetail.bankCode))
            //        binDetail.banksWithInstallments.Add(binDetail.bankCode);


            //    if (binDetail.banksWithInstallments.IndexOf(binDetail.bankCode) != 0)
            //    {
            //        binDetail.banksWithInstallments.Remove(binDetail.bankCode);
            //        binDetail.banksWithInstallments.Insert(0, binDetail.bankCode);
            //    }
            //}


           //return binList;
        //}

        private ICysteineVirtualPOSService GetVirtualPOSService(string bankCode)
        {
            ICysteineVirtualPOSService virtualPOSService = null;

            var searchBank = Services.BankService.AllBanks.FirstOrDefault(s => s.BankCode == bankCode);

            if (searchBank?.BankService != null)
                virtualPOSService = (ICysteineVirtualPOSService)Activator.CreateInstance(searchBank.BankService);
            else
                throw new Exception("Banka entegrasyonu bulunamadı.");

            return virtualPOSService;
        }
    }
}
