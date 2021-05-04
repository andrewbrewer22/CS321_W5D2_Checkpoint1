using System;
using CS321_W5D2_BlogAPI.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CS321_W5D2_BlogAPI.Core.Services;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CS321_W5D2_BlogAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        // TODO: inject PostService
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }
        [AllowAnonymous]
        [HttpGet("/api/blogs/{blogId}/posts")]
        public IActionResult Get(int blogId)
        {
            try
            {
                return Ok(_postService.GetBlogPosts(blogId).ToApiModels());
            }
            catch(Exception e)
            {
                ModelState.AddModelError("GET", e.Message);
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpGet("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Get(int blogId, int postId)
        {
            try
            {
                var posts = _postService.GetBlogPosts(blogId);

                if (posts == null)
                    return NotFound();

                return Ok(posts.FirstOrDefault(p => p.Id == postId).ToApiModel());
            }
            catch(Exception e)
            {
                ModelState.AddModelError("GET", e.Message);
                return BadRequest(ModelState);
            }
        }

        // TODO: add a new post to blog
        // POST /api/blogs/{blogId}/post
        [HttpPost("/api/blogs/{blogId}/posts")]
        public IActionResult Post(int blogId, [FromBody]PostModel postModel)
        {
            try
            {
                postModel.BlogId = blogId;
                return Ok(_postService.Add(postModel.ToDomainModel()).ToApiModel());
            }
            catch(Exception e)
            {
                ModelState.AddModelError("POST", e.Message);
                return BadRequest(ModelState);
            }
        }

        // PUT /api/blogs/{blogId}/posts/{postId}
        [HttpPut("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Put(int blogId, int postId, [FromBody]PostModel postModel)
        {
            try
            {
                var posts = _postService.GetBlogPosts(blogId);

                if (!posts.Any())
                    return NotFound();

                var post = _postService.Get(postId);

                if (post == null)
                    return NotFound();

                var updatedPost = _postService.Update(postModel.ToDomainModel());
                return Ok(updatedPost.ToApiModel());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdatePost", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // TODO: delete post by id
        // DELETE /api/blogs/{blogId}/posts/{postId}
        [HttpDelete("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Delete(int blogId, int postId)
        {
            try
            {
                var posts = _postService.GetBlogPosts(blogId);

                if (!posts.Any())
                    return NotFound();

                var post = _postService.Get(postId);

                if (post == null)
                    return NotFound();

                _postService.Remove(post.Id);

                return NoContent();
            }
            catch(Exception e)
            {
                ModelState.AddModelError("DeletePost", e.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
