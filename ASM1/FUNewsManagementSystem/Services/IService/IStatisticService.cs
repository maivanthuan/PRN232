using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DTO;

namespace Services.IService
{
    public interface IStatisticService
    {
        Task<IEnumerable<NewsStatisticDto>> GetNewsByPeriodAsync(DateTime startDate, DateTime endDate);
    }
}
