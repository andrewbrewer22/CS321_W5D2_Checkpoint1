﻿using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _dbContext;

        public BlogRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Blog> GetAll()
        {
            return _dbContext.Blogs
                .Include(b => b.User)
                .ToList();
        }

        public Blog Get(int id)
        {
            return _dbContext.Blogs
                .Include(b => b.User)
                .FirstOrDefault(b => b.Id == id);
        }

        public Blog Add(Blog blog)
        {
            _dbContext.Blogs.Add(blog);
            _dbContext.SaveChanges();

            return blog;
        }

        public Blog Update(Blog updatedItem)
        {
            var current = Get(updatedItem.Id);

            if (current == null)
                return null;

            _dbContext.Entry(current)
                .CurrentValues
                .SetValues(updatedItem);

            _dbContext.Blogs.Update(current);
            _dbContext.SaveChanges();

            return current;
        }

        public void Remove(int id)
        {
            var current = Get(id);

            _dbContext.Remove(current);
            _dbContext.SaveChanges();
        }
    }
}
