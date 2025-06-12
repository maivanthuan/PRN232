using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessObjects.DTO;
using Repositories.IRepository;
using Services.IService;

namespace Services.Service
{
    public class StatisticService : IStatisticService
    {
        private readonly INewsArticleRepository _repo;
        private readonly IMapper _mapper;

        public StatisticService(INewsArticleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsStatisticDto>> GetNewsByPeriodAsync(DateTime start, DateTime end)
        {
            var news = await _repo.FindAsync(n => n.CreatedDate >= start && n.CreatedDate <= end);
            return news.OrderByDescending(n => n.CreatedDate)
                       .Select(n => new NewsStatisticDto
                       {
                           NewsTitle = n.NewsTitle,
                           AuthorName = n.CreatedBy?.AccountName ?? "Unknown",
                           CreatedDate = n.CreatedDate ?? DateTime.MinValue
                       });
        }
    }
}
