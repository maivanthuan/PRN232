using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using static BusinessObject.DTOs.RevenueDTO;

namespace Service.IServices
{
    public interface ITotalInvoicePitchService
    {
        IQueryable<TotalInvoicePitchDTO> GetTotalInvoicesAsQueryable();
    
        Task<TotalInvoiceWithDetailsDTO?> GetTotalInvoiceByIdAsync(int id);
        Task<TotalInvoicePitchDTO> CreateTotalInvoiceAsync(TotalInvoicePitchCreateDTO createDto);
        Task<TotalInvoicePitchDTO?> UpdateTotalInvoiceAsync(int id, TotalInvoicePitchUpdateDTO updateDto);
        Task<bool> DeleteTotalInvoiceAsync(int id);
        Task<int?> GetTotalRevenueAsync();
        Task<List<YearlyRevenueDTO>> GetRevenueByYearAndMonthAsync();
        Task<List<TotalInvoiceWithDetailsDTO>> GetAllInvoicesWithDetailsAsync();
        Task<List<TotalInvoiceWithDetailsDTO>> GetInvoicesByUserIdAsync(int userId);
    }
}
