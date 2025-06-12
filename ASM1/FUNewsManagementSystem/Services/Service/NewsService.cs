using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.Models;
using DataAccessObjects.DTO;
using Repositories.IRepository;
using Services.IService;

namespace Services.Service
{
    public class NewsService : INewsService
    {
        private readonly INewsArticleRepository _newsRepo;
        private readonly ITagRepository _tagRepo;
        private readonly IMapper _mapper;
        private readonly FunewsManagementContext _context;
        public NewsService(INewsArticleRepository newsRepo, ITagRepository tagRepo, IMapper mapper, FunewsManagementContext context)
        {
            _newsRepo = newsRepo;
            _tagRepo = tagRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<NewsDto>> GetActiveNewsAsync()
        {
            var news = await _newsRepo.GetActiveAsync();
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<IEnumerable<NewsDto>> GetByAuthorAsync(short authorId)
        {
            var news = await _newsRepo.GetByAuthorAsync(authorId);
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<NewsDto?> GetByIdAsync(String id)
        {
            var news = await _newsRepo.GetByIdAsync(id);
            return news == null ? null : _mapper.Map<NewsDto>(news);
        }

        public async Task CreateNewsAsync(NewsCreateDto dto, short authorId)
        {
            var entity = _mapper.Map<NewsArticle>(dto);
           /* entity.NewsArticleId = Guid.NewGuid().ToString();*/
            entity.CreatedById = authorId;
            entity.CreatedDate = DateTime.Now;

            var tags = await _tagRepo.FindAsync(t => dto.TagIds.Contains(t.TagId));
            entity.Tags = tags.ToList();

            await _newsRepo.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

		public async Task<bool> UpdateNewsAsync(string id, NewsCreateDto dto, short authorId)
		{
			var entity = await _newsRepo.GetByIdAsync(id);
			if (entity == null || entity.CreatedById != authorId) return false;

			// Load Tags nếu chưa được include
			_context.Entry(entity).Collection(e => e.Tags).Load();

			// Xoá toàn bộ bản ghi trong bảng NewsTag
			entity.Tags.Clear();
			await _context.SaveChangesAsync(); // <- lưu trước khi thêm tag mới

			// Gán lại thông tin
			entity.NewsTitle = dto.NewsTitle;
			entity.NewsContent = dto.NewsContent;
			entity.CategoryId = dto.CategoryId;
			entity.Headline = dto.Headline;
			entity.ModifiedDate = DateTime.Now;

			// Lấy tag từ repo
			var tags = await _tagRepo.FindAsync(t => dto.TagIds.Contains(t.TagId));
			foreach (var tag in tags)
			{
				entity.Tags.Add(tag); // EF sẽ tạo lại NewsTag mới
			}

			_newsRepo.Update(entity);
			await _context.SaveChangesAsync();

			return true;
		}


		public async Task<bool> DeleteNewsAsync(String id, short requesterId)
        {
            var entity = await _newsRepo.GetByIdAsync(id);
            if (entity == null || entity.CreatedById != requesterId) return false;

            _newsRepo.Delete(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

