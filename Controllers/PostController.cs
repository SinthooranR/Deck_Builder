using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YTCG_Deck_Builder_API.Data;
using YTCG_Deck_Builder_API.Models.Dto;
using YTCG_Deck_Builder_API.Models.Entitities;

namespace YTCG_Deck_Builder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PostController> _logger;
        public PostController(DataContext dataContext, ILogger<PostController> logger, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult getAllPosts()
        {
            var posts = _dataContext.Posts.ToList();
            return Ok(posts);


        }

        [HttpPost]
        public async Task<IActionResult> createPost(PostCreateDto postCreateDto)
        {
            var user = await _userManager.FindByIdAsync(postCreateDto.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var post = new Post()
            {
                Name = postCreateDto.Name,
                CreatedAt = DateTime.UtcNow,
                PostRatings = [],
                Description = postCreateDto.Description,
                Replies = [],
                UserId = user.Id,
            };

            _dataContext.Posts.Add(post);
            _dataContext.SaveChanges();

            return Ok(post);
        }

        [HttpPut]
        public async Task<IActionResult> updatePost(PostUpdateDto postUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(postUpdateDto.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var postExists = _dataContext.Posts.Where(p => p.Id == postUpdateDto.PostId).FirstOrDefault();

            if (postExists == null)
            {
                return NotFound();
            }

            postExists.Name = postUpdateDto.Name;
            postExists.UpdatedAt = DateTime.UtcNow;
            postExists.Description = postUpdateDto.Description;
            postExists.UserId = user.Id;


            _dataContext.Posts.Update(postExists);
            _dataContext.SaveChanges();

            return Ok(postExists);
        }



        [HttpDelete("{postId}")]
        public async Task<IActionResult> deletePost(string userId, int postId)
        {
            var user = _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var post = _dataContext.Posts.Include(p => p.Replies).Include(p => p.PostRatings).Where(p => p.Id == postId && p.UserId.Equals(userId)).FirstOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            if (post.Replies != null)
            {
                _dataContext.Replies.RemoveRange(post.Replies);
            }

            if (post.PostRatings != null)
            {
                _dataContext.PostRatings.RemoveRange(post.PostRatings);
            }

            _dataContext.Remove(post);
            _dataContext.SaveChanges();

            return Ok("Post Deleted Successfully");
        }

    }
}
