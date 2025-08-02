using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;

namespace Service.IServices
{
    public interface IInvoicePitchService
    {
        IQueryable<InvoicePitchDTO> GetInvoicePitchesAsQueryable();
        Task<InvoicePitchDTO?> GetInvoicePitchByIdAsync(int id);
        Task<InvoicePitchDTO> CreateInvoicePitchAsync(InvoicePitchCreateDTO createDto);
        Task<InvoicePitchDTO?> UpdateInvoicePitchAsync(int id, InvoicePitchUpdateDTO updateDto);
        Task<bool> DeleteInvoicePitchAsync(int id);
    }
}
