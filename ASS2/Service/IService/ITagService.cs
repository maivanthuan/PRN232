using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ITagService
    {
        IQueryable<Tag> GetAll();
        Tag? GetById(int id);
        void Add(Tag tag);
        void Update(Tag tag);
        void Delete(int id);

    }
}
