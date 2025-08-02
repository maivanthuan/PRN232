using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTOs;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using Service.IServices;

namespace Service.Services
{

    public class BookingTimeService : IBookingTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingTimeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<BookingTimeDTO>> GetAvailableSlotsAsync(string pitchId, DateOnly selectedDate)
        {
            var allTimeSlots = await _unitOfWork.BookingTime.GetAllAsync();

            var bookedSlotIds = await _unitOfWork.TotalInvoicePitch
                .Get(t => t.BookTime == selectedDate) 
                .SelectMany(t => t.InvoicePitches)  
                .Where(ip => ip.PitchId == pitchId)  
                .Select(ip => ip.BookingTimeId)    
                .Distinct()
                .ToListAsync();

            var availableSlots = allTimeSlots
                .Where(slot => !bookedSlotIds.Contains(slot.BookingTimeId))
                .ToList();

            return _mapper.Map<List<BookingTimeDTO>>(availableSlots);
        }
    }
}
