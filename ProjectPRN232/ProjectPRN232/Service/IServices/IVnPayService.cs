using BusinessObject.DTOs;
using Microsoft.AspNetCore.Http;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentRequestDTO model, HttpContext context);
        PaymentResponseDTO VnPayReturn(IQueryCollection collections);
    }
}
