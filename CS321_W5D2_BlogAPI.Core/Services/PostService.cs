using System;
using System.Collections.Generic;
using CS321_W5D2_BlogAPI.Core.Models;

namespace CS321_W5D2_BlogAPI.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository, IBlogRepository blogRepository, IUserService userService)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _userService = userService;
        }

        public Post Add(Post newPost)
        {
            var currentBlog = _blogRepository.Get(newPost.BlogId);

            if (currentBlog == null)
                throw new Exception($"blog {newPost.BlogId} not found");

            if (currentBlog.UserId != _userService.CurrentUserId)
                throw new Exception($"unable to add to blog {newPost.BlogId} for user {_userService.CurrentUserId}");

            newPost.DatePublished = DateTime.Now;
            return _postRepository.Add(newPost);
        }

        public Post Get(int id)
        {
            return _postRepository.Get(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }
        
        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            return _postRepository.GetBlogPosts(blogId);
        }

        public void Remove(int id)
        {
            var post = this.Get(id);

            if (post.Blog.UserId != _userService.CurrentUserId)
                throw new Exception($"unable to remove post {id} for user {_userService.CurrentUserId}");

            _postRepository.Remove(id);
        }

        public Post Update(Post updatedPost)
        {
            var current = Get(updatedPost.Id);

            if (current.Blog.User.Id != _userService.CurrentUserId)
                throw new Exception(
                    $"unable to update post {updatedPost.Id} for user {_userService.CurrentUserId}");

            return _postRepository.Update(updatedPost);
        }

    }
}
