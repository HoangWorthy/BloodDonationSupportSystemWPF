using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Services
{
    public interface IBlogService
    {
        List<Blog> GetBlogs();
        void AddBlog(Blog blog);
        void RemoveBlog(Blog blog);
        void UpdateBlog(Blog blog);
    }
} 