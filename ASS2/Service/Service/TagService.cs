using BusinessObject.Models;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;
        public TagService(ITagRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        public void Add(Tag tag) => _repo.Add(tag);

        public void Delete(int id) => _repo.Delete(id);

        public IQueryable<Tag> GetAll() => _repo.GetAll();

        public Tag? GetById(int id) => _repo.GetById(id);

        public void Update(Tag tag)  => _repo.Update(tag);
    }
}
