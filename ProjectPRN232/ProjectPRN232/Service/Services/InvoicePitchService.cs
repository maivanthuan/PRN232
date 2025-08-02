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

namespace Service.Services
{
    public class InvoicePitchService : IInvoicePitchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoicePitchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InvoicePitchDTO> CreateInvoicePitchAsync(InvoicePitchCreateDTO createDto)
        {
            // 1. Tìm PricePitchId mới nhất dựa trên PitchId từ DTO
            var latestPricePitch = await _unitOfWork.PricePitch
          .Get(
              filter: p => p.PitchId == createDto.PitchId,
              orderBy: q => q.OrderByDescending(p => p.PricePitchId)
           )
          .FirstOrDefaultAsync();

            if (latestPricePitch == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy giá nào hợp lệ cho sân có ID = {createDto.PitchId}.");
            }
            var invoicePitch = _mapper.Map<InvoicePitch>(createDto);

            invoicePitch.PricePitchId = latestPricePitch.PricePitchId;

            await _unitOfWork.InvoicePitch.AddAsync(invoicePitch);
            await _unitOfWork.SaveChangesAsync();


            var resultDto = _mapper.Map<InvoicePitchDTO>(invoicePitch);

            resultDto.Price = latestPricePitch.Price;
            resultDto.Time = (await _unitOfWork.BookingTime.GetByIdAsync(invoicePitch.BookingTimeId))?.Time;

            return resultDto;
        }

        public async Task<bool> DeleteInvoicePitchAsync(int id)
        {
            var invoicePitch = await _unitOfWork.InvoicePitch.GetByIdAsync(id);
            if (invoicePitch == null) return false;

            _unitOfWork.InvoicePitch.Delete(invoicePitch);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public IQueryable<InvoicePitchDTO> GetInvoicePitchesAsQueryable()
        {
            var invoicePitches = _unitOfWork.InvoicePitch.Get(includeProperties: "PricePitch,BookingTime");
            return invoicePitches.ProjectTo<InvoicePitchDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<InvoicePitchDTO?> GetInvoicePitchByIdAsync(int id)
        {
            var invoicePitch = await _unitOfWork.InvoicePitch
                .Get(i => i.InvoicePitchId == id, includeProperties: "PricePitch,BookingTime")
                .FirstOrDefaultAsync();

            return _mapper.Map<InvoicePitchDTO>(invoicePitch);
        }

        public async Task<InvoicePitchDTO?> UpdateInvoicePitchAsync(int id, InvoicePitchUpdateDTO updateDto)
        {
            var existingInvoice = await _unitOfWork.InvoicePitch.GetByIdAsync(id);
            if (existingInvoice == null) return null;

            _mapper.Map(updateDto, existingInvoice);
            _unitOfWork.InvoicePitch.Update(existingInvoice);
            await _unitOfWork.SaveChangesAsync();

            return await GetInvoicePitchByIdAsync(existingInvoice.InvoicePitchId);
        }
    }

}
