using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;
using uweb4Media.Application.Features.CQRS.Handlers.Like;
using uweb4Media.Application.Features.CQRS.Handlers.Comments;
using uweb4Media.Application.Features.CQRS.Handlers.Subscription;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Company;
using uweb4Media.Application.Features.CQRS.Handlers.User;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly GetVideoQueryHandler _getVideoQueryHandler;
        private readonly GetLikeQueryHandler _getLikeQueryHandler;
        private readonly GetCommentQueryHandler _getCommentQueryHandler;
        private readonly GetSubscriptionQueryHandler _getSubscriptionQueryHandler;
        private readonly GetCompanyQueryHandler _getCompanyQueryHandler;
        private readonly GetUserQueryHandler _getUserQueryHandler;

        public ContentController(
            GetVideoQueryHandler getVideoQueryHandler,
            GetLikeQueryHandler getLikeQueryHandler,
            GetCommentQueryHandler getCommentQueryHandler,
            GetSubscriptionQueryHandler getSubscriptionQueryHandler,
            GetCompanyQueryHandler getCompanyQueryHandler,
            GetUserQueryHandler getUserQueryHandler)
        {
            _getVideoQueryHandler = getVideoQueryHandler;
            _getLikeQueryHandler = getLikeQueryHandler;
            _getCommentQueryHandler = getCommentQueryHandler;
            _getSubscriptionQueryHandler = getSubscriptionQueryHandler;
            _getCompanyQueryHandler = getCompanyQueryHandler;
            _getUserQueryHandler = getUserQueryHandler;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllContent()
        {
            var videos = await _getVideoQueryHandler.Handle();
            var likes = await _getLikeQueryHandler.Handle();
            var comments = await _getCommentQueryHandler.Handle();
            var companies = await _getCompanyQueryHandler.Handle();
            var users = await _getUserQueryHandler.Handle();

            var result = videos.Select(video =>
            {
                string authorAvatar = null;
                string authorName = null;

                if (video.CompanyId != null)
                {
                    var company = companies.FirstOrDefault(c => c.Id == video.CompanyId);
                    authorAvatar = company?.Logo;
                    authorName = company?.Name;
                }
                else if (video.UserId != null)
                {
                    // DİKKAT: UserId'nin karşılığı AppUserID!
                    var user = users.FirstOrDefault(u => u.AppUserID == video.UserId);
                    authorAvatar = user?.AvatarUrl;
                    authorName = string.IsNullOrWhiteSpace(user?.Name)
                        ? user?.Username
                        : (user?.Name + (string.IsNullOrWhiteSpace(user?.Surname) ? "" : " " + user?.Surname));
                }
                else
                {
                    authorAvatar = "/images/defaultUserIcon.svg";
                    authorName = video.Responsible ?? "Unknown";
                }

                return new
                {
                    Video = video,
                    LikeCount = likes.Count(l => l.VideoId == video.Id),
                    CommentCount = comments.Count(c => c.VideoId == video.Id),
                    AuthorAvatar = authorAvatar,
                    AuthorName = authorName
                };
            });

            return Ok(result);
        }
    }
}