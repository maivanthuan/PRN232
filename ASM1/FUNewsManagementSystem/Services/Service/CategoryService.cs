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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;
		private readonly FunewsManagementContext _context;
		public CategoryService(ICategoryRepository repo, IMapper mapper, FunewsManagementContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(short id)
        {
            var cat = await _repo.GetByIdAsync(id);
            return cat == null ? null : _mapper.Map<CategoryDto>(cat);
        }

        public async Task AddAsync(CategoryDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            await _repo.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

        public async Task UpdateAsync(CategoryDto dto)
        {
            var entity = await _repo.GetByIdAsync(dto.CategoryId);
            if (entity == null) return;
            entity.CategoryName = dto.CategoryName;
            entity.CategoryDesciption = dto.CategoryDesciption;
            entity.ParentCategoryId = dto.ParentCategoryId;
            entity.IsActive = dto.IsActive;
            _repo.Update(entity);
			await _context.SaveChangesAsync();
		}

        public async Task<bool> DeleteAsync(short id)
        {
            if (await _repo.HasNewsArticlesAsync(id)) return false;
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Delete(entity);
			await _context.SaveChangesAsync();
			return true;
        }
    }
}
