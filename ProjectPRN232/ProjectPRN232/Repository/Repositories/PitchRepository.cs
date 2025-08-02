using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class PitchRepository : GenericRepository<Pitch>, IPitchRepository
    {
        public PitchRepository(FootballFieldManagerContext context) : base(context)
        {
        }

       
    }
}
