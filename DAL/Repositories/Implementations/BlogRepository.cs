using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DAL.Repositories.Implementations
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BloodDonationSupportSystemContext _context;

        public BlogRepository()
        {
            _context = new BloodDonationSupportSystemContext();
        }

        public List<Blog> GetBlogs()
        {
            return _context.Blogs.Include(b => b.Author).ToList();
        }

        public void AddBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            _context.SaveChanges();
        }

        public void UpdateBlog(Blog blog)
        {
            _context.Blogs.Update(blog);
            _context.SaveChanges();
        }

        public void RemoveBlog(Blog blog)
        {
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
        }
    }
} 