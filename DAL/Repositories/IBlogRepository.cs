using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface IBlogRepository
    {
        List<Blog> GetBlogs();
        void AddBlog(Blog blog);
        void RemoveBlog(Blog blog);
        void UpdateBlog(Blog blog);
    }
} 