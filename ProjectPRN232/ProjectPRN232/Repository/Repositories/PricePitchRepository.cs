using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class PricePitchRepository : GenericRepository<PricePitch>, IPricePitchRepository
    {
        public PricePitchRepository(FootballFieldManagerContext context) : base(context)
        {
        }
    }
}
