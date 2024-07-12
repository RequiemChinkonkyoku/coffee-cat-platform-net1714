using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Repositories.Impl
{
    public class MomoRepository : IMomoRepository
    {
        private readonly IOptions<MomoOptionModel> _options;
        private readonly HttpClient _httpClient;

        public MomoRepository(IOptions<MomoOptionModel> options, HttpClient httpClient)
        {
            _options = options;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_options.Value.MomoApiUrl);
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model, int reservationID)
        {
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfo = "Customer: " + model.FullName + ". Content: " + model.OrderInfo + ". ReservationID: " + reservationID;
            var rawData =
                $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={model.OrderId}&amount={model.Amount}&orderId={model.OrderId}&orderInfo={model.OrderInfo}&returnUrl={_options.Value.ReturnUrl}&notifyUrl={_options.Value.NotifyUrl}&extraData=";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = "captureMoMoWallet",
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = model.Amount.ToString(),
                orderInfo = model.OrderInfo,
                requestId = model.OrderId,
                extraData = "",
                signature = signature
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(responseContent);
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;
            var errorCode = collection.First(s => s.Key == "errorCode").Value;

            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo,
                ErrorCode = errorCode
            };
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

        public async Task<MomoCreateRefundResponseModel> CreateRefundAsync(OrderInfoModel model, int reservationID)
        {
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfo = "Refund for reservation: " + reservationID + ", Amount: " + model.Amount;

            var rawData =
                $"partnerCode={_options.Value.PartnerCode}" +
                $"&merchantRefId=151971741046" +
                $"&momoTransId={model.OrderId}" +
                $"&amount={model.Amount}" +
                $"&description=" +
                $"&publicKey={_options.Value.AccessKey}";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var requestData = new
            {
                partnerCode = _options.Value.PartnerCode,
                requestId = model.OrderId,
                amount = model.Amount,
                version = "2.0",
                signature = signature
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://test-payment.momo.vn/pay/refund", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MomoCreateRefundResponseModel>(responseContent);
        }
    }
}
