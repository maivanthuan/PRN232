using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Account { get; }
        IBookingTimeRepository BookingTime { get; }
        IFeedbackPitchRepository FeedbackPitch { get; }
        IInvoicePitchRepository InvoicePitch { get; }
        IPitchRepository Pitch { get; }
        ITotalInvoicePitchRepository TotalInvoicePitch { get; }
        IPricePitchRepository PricePitch { get; }
        Task<int> SaveChangesAsync();
    }
}
