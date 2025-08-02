using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;

namespace Service.IServices
{
    public interface IBookingTimeService
    {
        Task<List<BookingTimeDTO>> GetAvailableSlotsAsync(string pitchId, DateOnly selectedDate);
    }
}
