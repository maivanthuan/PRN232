﻿using BusinessObject.Models;
using DataAccess;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly TagDAO _tagDAO;
        public TagRepository(TagDAO tagDAO) 
        {
            _tagDAO = tagDAO;
        }

        public void Add(Tag tag) => _tagDAO.Add(tag);

        public void Delete(int id) => _tagDAO.Delete(id);

        public IQueryable<Tag> GetAll() => _tagDAO.GetAll();

        public Tag? GetById(int id) => _tagDAO.GetById(id);

        public void Update(Tag tag) => _tagDAO.Update(tag);
    }
}
