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
    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var tags = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<IEnumerable<TagDto>> GetUnusedAsync()
        {
            var tags = await _repo.GetUnusedTagsAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<TagDto?> GetByIdAsync(int id)
        {
            var tag = await _repo.GetByIdAsync(id);
            return tag == null ? null : _mapper.Map<TagDto>(tag);
        }

        public async Task AddAsync(TagDto dto)
        {
            var tag = _mapper.Map<Tag>(dto);
            await _repo.AddAsync(tag);
        }

        public async Task<bool> UpdateAsync(int id, TagDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.TagName = dto.TagName;
            _repo.Update(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await _repo.GetByIdAsync(id);
            if (tag == null || tag.NewsArticles.Any()) return false;

            _repo.Delete(tag);
            return true;
        }
    }
}
