using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class InvoicePitchRepository : GenericRepository<InvoicePitch>, IInvoicePitchRepository
    {
       

        public InvoicePitchRepository(FootballFieldManagerContext context) : base(context)
        {
        }
    }
}
