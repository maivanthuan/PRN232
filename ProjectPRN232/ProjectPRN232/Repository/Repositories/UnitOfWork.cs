using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FootballFieldManagerContext _context;

        public IAccountRepository Account { get; private set; }

        public IBookingTimeRepository BookingTime { get; private set; }

        public IFeedbackPitchRepository FeedbackPitch { get; private set; }

        public IInvoicePitchRepository InvoicePitch { get; private set; }

        public IPitchRepository Pitch { get; private set; }

        public ITotalInvoicePitchRepository TotalInvoicePitch { get; private set; }
        public IPricePitchRepository PricePitch { get; private set; }



        public UnitOfWork(FootballFieldManagerContext context)
        {
            _context = context;
 
            Account = new AccountRepository(_context);
            BookingTime = new BookingTimeRepository(_context);
            FeedbackPitch = new FeedbackRepository(_context);
            InvoicePitch = new InvoicePitchRepository(_context);
            TotalInvoicePitch = new TotalInvoicePitchRepository(_context);
            Pitch   = new PitchRepository(_context);
            PricePitch = new PricePitchRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
