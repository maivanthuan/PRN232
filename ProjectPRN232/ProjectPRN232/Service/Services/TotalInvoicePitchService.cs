using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.DTOs;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using Service.IServices;
using static BusinessObject.DTOs.RevenueDTO;

namespace Service.Services
{
    public class TotalInvoicePitchService : ITotalInvoicePitchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TotalInvoicePitchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TotalInvoicePitchDTO> CreateTotalInvoiceAsync(TotalInvoicePitchCreateDTO createDto)
        {
            var totalInvoice = _mapper.Map<TotalInvoicePitch>(createDto);

            await _unitOfWork.TotalInvoicePitch.AddAsync(totalInvoice);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TotalInvoicePitchDTO>(totalInvoice);
        }

        public async Task<bool> DeleteTotalInvoiceAsync(int id)
        {
            var totalInvoice = await _unitOfWork.TotalInvoicePitch.GetByIdAsync(id);
            if (totalInvoice == null) return false;

            _unitOfWork.TotalInvoicePitch.Delete(totalInvoice);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public IQueryable<TotalInvoicePitchDTO> GetTotalInvoicesAsQueryable()
        {
            var totalInvoices = _unitOfWork.TotalInvoicePitch.Get();
            return totalInvoices.ProjectTo<TotalInvoicePitchDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<TotalInvoiceWithDetailsDTO?> GetTotalInvoiceByIdAsync(int id)
        {
            
            var totalInvoice = await _unitOfWork.TotalInvoicePitch
                .Get(
                    t => t.TotalInvoiceId == id,
                    includeProperties: "InvoicePitches.PricePitch,InvoicePitches.BookingTime"
                )
                .FirstOrDefaultAsync();

            return _mapper.Map<TotalInvoiceWithDetailsDTO>(totalInvoice);
        }

        public async Task<TotalInvoicePitchDTO?> UpdateTotalInvoiceAsync(int id, TotalInvoicePitchUpdateDTO updateDto)
        {
            var existingInvoice = await _unitOfWork.TotalInvoicePitch.GetByIdAsync(id);
            if (existingInvoice == null)
            {
                return null;
            }

            _mapper.Map(updateDto, existingInvoice);

            _unitOfWork.TotalInvoicePitch.Update(existingInvoice);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TotalInvoicePitchDTO>(existingInvoice);
        }
        public async Task<int?> GetTotalRevenueAsync()
        {
            var totalRevenue = await _unitOfWork.InvoicePitch
                .Get()
                .Select(ip => ip.PricePitch.Price)
                .SumAsync();

            return totalRevenue;
        }

        public async Task<List<YearlyRevenueDTO>> GetRevenueByYearAndMonthAsync()
        {
            var revenueData = await _unitOfWork.InvoicePitch.Get()
                // Đảm bảo có ngày và giá để tính toán
                .Where(ip => ip.TotalInvoice.BookTime.HasValue && ip.PricePitch.Price.HasValue)
                // Nhóm tất cả hóa đơn theo năm
                .GroupBy(ip => ip.TotalInvoice.BookTime.Value.Year)
                // Tạo đối tượng DTO cho mỗi năm
                .Select(yearGroup => new YearlyRevenueDTO
                {
                    Year = yearGroup.Key,
                    TotalYearlyRevenue = yearGroup.Sum(ip => ip.PricePitch.Price),
                    // Với mỗi năm, tiếp tục nhóm theo tháng
                    MonthlyBreakdown = yearGroup
                        .GroupBy(ipInYear => ipInYear.TotalInvoice.BookTime.Value.Month)
                        .Select(monthGroup => new MonthlyRevenueItemDTO
                        {
                            Month = monthGroup.Key,
                            TotalMonthlyRevenue = monthGroup.Sum(ipInMonth => ipInMonth.PricePitch.Price)
                        })
                        .OrderBy(m => m.Month)
                        .ToList()
                })
                .OrderBy(y => y.Year) // Sắp xếp kết quả theo năm
                .ToListAsync();

            return revenueData;
        }


        public async Task<List<TotalInvoiceWithDetailsDTO>> GetAllInvoicesWithDetailsAsync()
        {
            var totalInvoices = await _unitOfWork.TotalInvoicePitch
                .Get(
                    // Không có điều kiện lọc, lấy tất cả
                    filter: null,
                    // Sắp xếp theo ngày đặt gần nhất lên đầu
                    orderBy: q => q.OrderByDescending(t => t.BookTime),
                    // Quan trọng: Lấy kèm tất cả các chi tiết hóa đơn liên quan
                    includeProperties: "InvoicePitches.PricePitch,InvoicePitches.BookingTime,User"
                )
                .ToListAsync();

            return _mapper.Map<List<TotalInvoiceWithDetailsDTO>>(totalInvoices);
        }
        public async Task<List<TotalInvoiceWithDetailsDTO>> GetInvoicesByUserIdAsync(int userId)
        {
            var totalInvoices = await _unitOfWork.TotalInvoicePitch
                .Get(
                    // Điều kiện lọc: lấy các hóa đơn có UserId trùng khớp
                    filter: t => t.UserId == userId,
                    // Sắp xếp theo ngày đặt gần nhất lên đầu
                    orderBy: q => q.OrderByDescending(t => t.BookTime),
                    // Lấy kèm tất cả các chi tiết hóa đơn liên quan
                    includeProperties: "InvoicePitches.PricePitch,InvoicePitches.BookingTime,User"
                )
                .ToListAsync();

            return _mapper.Map<List<TotalInvoiceWithDetailsDTO>>(totalInvoices);
        }
    }
}
