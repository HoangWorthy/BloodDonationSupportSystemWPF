using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Implementations;
using System.Collections.Generic;

namespace BLL.Services.Implementations
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService()
        {
            _blogRepository = new BlogRepository();
        }

        public List<Blog> GetBlogs()
        {
            return _blogRepository.GetBlogs();
        }

        public void AddBlog(Blog blog)
        {
            _blogRepository.AddBlog(blog);
        }

        public void UpdateBlog(Blog blog)
        {
            _blogRepository.UpdateBlog(blog);
        }

        public void RemoveBlog(Blog blog)
        {
            _blogRepository.RemoveBlog(blog);
        }
    }
} 