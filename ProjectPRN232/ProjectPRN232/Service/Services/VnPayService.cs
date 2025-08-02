using BusinessObject.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPayConfig _vnpayConfig;

        public VnPayService(IOptions<VnPayConfig> vnpayConfig)
        {
            _vnpayConfig = vnpayConfig.Value;
        }

        public string CreatePaymentUrl(PaymentRequestDTO model, HttpContext context)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _vnpayConfig.TmnCode);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString()); // Số tiền * 100
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", model.OrderId); // Mã giao dịch của hệ thống bạn
            vnpay.AddRequestData("vnp_OrderInfo", model.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", "other"); // Loại hàng hóa (có thể thay đổi)
            vnpay.AddRequestData("vnp_Locale", "vn"); // Ngôn ngữ (vn/en)
            vnpay.AddRequestData("vnp_ReturnUrl", _vnpayConfig.ReturnUrl);
            vnpay.AddRequestData("vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            // Nếu muốn thêm ngân hàng cụ thể
            if (!string.IsNullOrEmpty(model.BankCode))
            {
                vnpay.AddRequestData("vnp_BankCode", model.BankCode);
            }

            var paymentUrl = vnpay.CreateRequestUrl(_vnpayConfig.BaseUrl, _vnpayConfig.HashSecret);
            return paymentUrl;
        }

        public PaymentResponseDTO VnPayReturn(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (string key in collections.Keys)
            {
                // Lấy giá trị từ query string
                var value = collections[key];
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_SecureHash = collections["vnp_SecureHash"];
            var orderId = collections["vnp_TxnRef"];
            var amount = Convert.ToInt64(collections["vnp_Amount"]) / 100; // Chia lại cho 100
            var vnp_ResponseCode = collections["vnp_ResponseCode"];
            var vnp_TransactionStatus = collections["vnp_TransactionStatus"];
            var vnp_OrderInfo = collections["vnp_OrderInfo"];

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _vnpayConfig.HashSecret);

            if (!checkSignature)
            {
                return new PaymentResponseDTO
                {
                    Success = false,
                    Message = "Sai chữ ký điện tử (invalid signature)",
                    OrderId = orderId,
                    Amount = (int)amount,
                    VnPayResponseCode = vnp_ResponseCode
                };
            }

            // Kiểm tra trạng thái giao dịch
            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                // Giao dịch thành công
                // Cần kiểm tra thêm:
                // 1. Kiểm tra mã giao dịch vnp_TxnRef có hợp lệ và chưa được xử lý không? (Chống replay attack)
                // 2. Kiểm tra số tiền vnp_Amount nhận được có đúng với số tiền trong đơn hàng không?
                return new PaymentResponseDTO
                {
                    Success = true,
                    Message = "Giao dịch thành công",
                    OrderId = orderId,
                    Amount = (int)amount,
                    VnPayResponseCode = vnp_ResponseCode
                };
            }
            else
            {
                // Giao dịch thất bại hoặc lỗi
                return new PaymentResponseDTO
                {
                    Success = false,
                    Message = $"Giao dịch thất bại. Mã lỗi VNPay: {vnp_ResponseCode}, Trạng thái: {vnp_TransactionStatus}",
                    OrderId = orderId,
                    Amount = (int)amount,
                    VnPayResponseCode = vnp_ResponseCode
                };
            }
        }
    }

    // Class PaymentRequestDTO để truyền thông tin thanh toán cho VNPayService
    public class PaymentRequestDTO
    {
        public string OrderId { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string OrderDescription { get; set; } = string.Empty;
        public string? BankCode { get; set; } // Mã ngân hàng (nếu muốn chỉ định)
        // Thêm các thông tin khác cần thiết cho việc tạo hóa đơn backend
        public string PitchId { get; set; } = string.Empty;
        public DateOnly SelectedDate { get; set; }
        public List<int> SelectedSlots { get; set; } = new List<int>();
        public List<string> SelectedSlotTimes { get; set; } = new List<string>();
        public int UserId { get; set; } // Cần lấy từ Claim
    }

    // Class PaymentResponseDTO để trả về kết quả từ VNPay
    public class PaymentResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string VnPayResponseCode { get; set; } = string.Empty;
    }

    // Thư viện VNPay (Được cung cấp bởi VNPay, bạn có thể tìm trên mạng hoặc từ tài liệu của họ)
    // Đây là một phiên bản rút gọn/ví dụ
    public class VnPayLibrary
    {
        public static string VERSION = "2.1.0"; // Phiên bản API VNPay

        private SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string Get
            (string key)
        {
            return _responseData.ContainsKey(key) ? _responseData[key] : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string rawData = data.ToString();

            string hashData = string.Empty;
            if (_requestData.ContainsKey("vnp_SecureHashType"))
            {
                _requestData.Remove("vnp_SecureHashType");
            }

            hashData = string.Join("&", _requestData.Select(kv => WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value)));

            string secureHash = HmacSHA512(hashSecret, hashData); // Sử dụng SHA512

            return baseUrl + "?" + rawData + "vnp_SecureHash=" + secureHash;
        }

        public bool ValidateSignature(string secureHash, string hashSecret)
        {
            // Remove vnp_SecureHash and vnp_SecureHashType
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            string hashData = string.Join("&", _responseData.Select(kv => WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value)));

            string mySecureHash = HmacSHA512(hashSecret, hashData);

            return mySecureHash.Equals(secureHash);
        }

        private string HmacSHA512(string key, string input)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using (var hmac = new System.Security.Cryptography.HMACSHA512(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                foreach (byte b in hashBytes)
                {
                    hash.Append(b.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnp_x = x.IndexOf("vnp_");
            var vnp_y = y.IndexOf("vnp_");
            if (vnp_x == 0 && vnp_x == vnp_y)
            {
                var vnp_xn = x.Substring(4, x.Length - 4);
                var vnp_yn = y.Substring(4, y.Length - 4);
                return string.CompareOrdinal(vnp_xn, vnp_yn);
            }
            else
            {
                return string.CompareOrdinal(x, y);
            }
        }
    }
}
