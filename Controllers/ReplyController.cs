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
    public class ReplyController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ReplyController> _logger;
        public ReplyController(DataContext dataContext, ILogger<ReplyController> logger, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _logger = logger;
            _userManager = userManager;
        }


        [HttpGet("{postId}")]
        public IActionResult GetAllRepliesByPost(int postId)
        {
            var replies = _dataContext.Replies.Where(r => r.PostId == postId).Select(r => new Reply() { CreatedAt = r.CreatedAt, Id = r.Id, PostId = r.PostId, ReplyRatings = r.ReplyRatings, Text = r.Text, UpdatedAt = r.UpdatedAt }).ToList();
            return Ok(replies);
        }

        [HttpGet("/User/{userId}")]
        public IActionResult GetAllRepliesByUser(string userId)
        {
            var replies = _dataContext.Replies.Where(r => r.UserId == userId).ToList();
            return Ok(replies);
        }

        [HttpPost]
        public async Task<IActionResult> createReply(ReplyCreateDto replyCreateDto)
        {
            var user = await _userManager.FindByIdAsync(replyCreateDto.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var postExists = _dataContext.Posts.Where(p => p.Id == replyCreateDto.PostId).FirstOrDefault();
            if (postExists == null)
            {
                return NotFound();
            }

            var reply = new Reply()
            {
                Text = replyCreateDto.Text,
                User = user,
                CreatedAt = DateTime.Now,
                UserId = replyCreateDto.UserId,
                ReplyRatings = [],
                PostId = replyCreateDto.PostId,
                Replies = []
            };

            _dataContext.Replies.Add(reply);
            _dataContext.SaveChanges();

            return Ok(reply);
        }

        [HttpPut]
        public async Task<IActionResult> updateReply(ReplyUpdateDto replyUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(replyUpdateDto.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var postExists = _dataContext.Posts.Where(p => p.Id == replyUpdateDto.PostId).FirstOrDefault();
            if (postExists == null)
            {
                return NotFound();
            }

            var reply = _dataContext.Replies.Where(r => r.Id == replyUpdateDto.ReplyId).FirstOrDefault();
            if (reply == null)
            {
                return NotFound();
            }


            reply.Text = replyUpdateDto.Text;
            reply.UpdatedAt = DateTime.Now;
            reply.UserId = replyUpdateDto.UserId;
            reply.PostId = replyUpdateDto.PostId;


            _dataContext.Replies.Update(reply);
            _dataContext.SaveChanges();

            return Ok(reply);
        }

        [HttpDelete("{replyId}")]
        public async Task<IActionResult> deleteReply(string userId, int replyId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var reply = _dataContext.Replies.Include(r => r.ReplyRatings).Where(r => r.Id == replyId).FirstOrDefault();
            if (reply == null)
            {
                return NotFound();
            }

            if (reply.ReplyRatings != null)
            {
                _dataContext.ReplyRatings.RemoveRange(reply.ReplyRatings);
            }

            _dataContext.Remove(reply);
            _dataContext.SaveChanges();

            return Ok("Reply Deleted Successfully");
        }

    }
}
