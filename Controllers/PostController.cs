using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using YTCG_Deck_Builder_API.Data;
using YTCG_Deck_Builder_API.Hubs;
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
        private readonly IHubContext<PostRatingHub> _hubContext;

        public PostController(DataContext dataContext, ILogger<PostController> logger, UserManager<User> userManager, IHubContext<PostRatingHub> hubContext)
        {
            _dataContext = dataContext;
            _logger = logger;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult getAllPosts()
        {
            var posts = _dataContext.Posts.ToList();
            var updatedPosts = posts.Select(p =>
            {

                var postDto = new Post()
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    PostRatings = _dataContext.PostRatings.Where(pr => pr.PostId == p.Id).Select(pr => new PostRating() { Id = pr.Id, PostId = pr.PostId, UserId = pr.UserId, IsThumbsUp = pr.IsThumbsUp }).ToList(),
                    Description = p.Description,
                    Replies = _dataContext.Replies.Where(r => r.Id == p.Id).Select(r => new Reply()
                    {
                        PostId = r.PostId,
                        Text = r.Text,
                        UserId = r.UserId,
                    }).ToList(),
                    UserId = p.UserId,
                };

                return postDto;

            }
            );

            return Ok(updatedPosts);


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

        [HttpPut("{postId}")]
        public async Task<IActionResult> updatePostRating(PostRatingUpdateDto postRatingUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(postRatingUpdateDto.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var post = _dataContext.Posts.Include(p => p.PostRatings).Where(post => post.Id == postRatingUpdateDto.PostId).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            var existingRating = post.PostRatings?.Where(rating => rating.UserId == postRatingUpdateDto.UserId).FirstOrDefault();
            //if a new rating is being made since there was none found
            if (existingRating == null)
            {

                var newRating = new PostRating()
                {
                    PostId = postRatingUpdateDto.PostId,
                    UserId = postRatingUpdateDto.UserId,
                    IsThumbsUp = postRatingUpdateDto.IsThumbsUp,
                };

                _dataContext.PostRatings.Add(newRating);
            }

            else
            {
                //updating the existing rating
                existingRating.IsThumbsUp = postRatingUpdateDto.IsThumbsUp == existingRating.IsThumbsUp ? null : postRatingUpdateDto.IsThumbsUp;
                _dataContext.PostRatings.Update(existingRating);
            }

            await _dataContext.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveRatingUpdate", postRatingUpdateDto);



            return Ok("Rating has been updated");
        }

    }
}
