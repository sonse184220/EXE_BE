using Contract.Common.Constant;
using Contract.Common.Enums;
using Contract.Dtos.Requests.Community;
using Contract.Dtos.Responses.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Services;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/community")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityService;
        private readonly IUserService _userService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<CommunityController> _logger;
        public CommunityController(ICommunityService communityService, IUserService userService, ICloudinaryService cloudinaryService, ILogger<CommunityController> logger)
        {
            _communityService = communityService;
            _userService = userService;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }
        [HttpPost("create-community")]
        public async Task<IActionResult> CreateCommunity([FromForm] CommunityCreateRequest request)
        {
            try
            {
                var community = new UserCommunity()
                {
                    CommunityGroupId = Guid.NewGuid().ToString(),
                    CommunityName = request.CommunityName,
                    CommunityDescription = request.CommunityDescription,
                    CommunityStatus = request.CommunityStatus.ToString(),
                    CommunityCreatedAt = DateTime.UtcNow,
                };
                await _communityService.CreateCommunityAsync(community);
                return Created("", new { message = "Community created successfully" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

        }
        [HttpGet("all-communities")]
        public async Task<IActionResult> GetAllCommunities()
        {
            var communities = await _communityService.GetAllCommunitiesAsync();
            return Ok(communities);

        }
        [HttpGet("community-detail/{id}")]
        public async Task<IActionResult> GetCommunityById(string id)
        {
            var community = await _communityService.GetCommunityByIdAsync(id);
            if (community == null)
                return NotFound("Community not found");
            var result = new CommunityDetailReponse()
            {
                CommunityGroupId = community.CommunityGroupId,
                CommunityName = community.CommunityName,
                CommunityDescription = community.CommunityDescription,
                CommunityCreatedAt = community.CommunityCreatedAt,
                CommunityStatus = community.CommunityStatus,
                CommunityMembersDetail = community.CommunityMembers.Select(m => new CommunityMemberDetail()
                {
                    CommunityMemberId = m.CommunityMemberId,
                    CommunityMemberStatus = m.CommunityMemberStatus,
                    CommunityGroupId = m.CommunityGroupId,
                    ProfileId = m.ProfileId,
                    UserId = m.Profile?.User?.UserId,
                    ProfilePicture = m.Profile?.User.ProfilePicture,
                    UserName = m.Profile?.User?.FullName,
                }),
                CommunityPostsDetail = community.CommunityPosts.Select(m => new CommunityPostDetail()
                {
                    CommunityPostId = m.CommunityPostId,
                    CommunityPostTitle = m.CommunityPostTitle,
                    CommunityPostContent = m.CommunityPostContent,
                    CommunityPostImageUrl = m.CommunityPostImageUrl,
                    CommunityPostStatus = m.CommunityPostStatus,
                    CommunityPostCreatedAt = m.CommunityPostCreatedAt,
                    CommunityGroupId = m.CommunityGroupId,
                    ProfileId = m.ProfileId,
                    UserId = m.Profile?.User?.UserId,
                    ProfilePicture = m.Profile?.User.ProfilePicture,
                    UserName = m.Profile?.User?.FullName,
                })
            };

            return Ok(result);
        }

        [HttpPut("update-community/{id}")]
        public async Task<IActionResult> UpdateCommunity(string id, [FromForm] CommunityUpdateRequest updatedCommunity)
        {

            try
            {
                var existingCommunity = await _communityService.GetCommunityByIdAsync(id);
                if (existingCommunity == null)
                {
                    return NotFound("Community not found");
                }
                existingCommunity.CommunityDescription = updatedCommunity.CommunityDescription ?? existingCommunity.CommunityDescription;
                existingCommunity.CommunityName = updatedCommunity.CommunityName ?? existingCommunity.CommunityName;
                if (updatedCommunity.CommunityStatus.HasValue)
                {
                    existingCommunity.CommunityStatus = updatedCommunity.CommunityStatus.ToString() ?? existingCommunity.CommunityStatus;
                }
                ;
                await _communityService.UpdateUserCommunityAsync(existingCommunity);
                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"An error occurred");
            }

        }
        [Authorize]
        [HttpPost("join-community")]
        public async Task<IActionResult> JoinCommunity([FromBody] CommunityMemberCreateRequest request)
        {
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;

                if (string.IsNullOrEmpty(profileId))
                {
                    return BadRequest("User not found");
                }
                var group = await _communityService.GetCommunityByIdAsync(request.CommunityGroupId);
                if (group == null)
                {
                    return NotFound("Community not found");
                }
                var alreadyJoined = await _communityService.GetCommunityMembersByProfileIdAndGroupIdAsync(profileId, request.CommunityGroupId);
                if (alreadyJoined != null)
                {
                    return BadRequest("User already joined this group");
                }
                var userCommunityMember = new CommunityMember()
                {
                    CommunityMemberId = profileId,
                    CommunityGroupId = group.CommunityGroupId,
                    ProfileId = profileId,
                };
                await _communityService.CreateCommunityMemberAsync(userCommunityMember);
                return Created("", new { message = "Community member joined successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

        }
        [Authorize]
        [HttpPost("leave-community/{communityId}")]
        public async Task<IActionResult> LeaveCommunity(string communityId)
        {
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;

                if (string.IsNullOrEmpty(profileId))
                {
                    return BadRequest("User not found");
                }
                var communityMember = await _communityService.GetCommunityMembersByProfileIdAndGroupIdAsync(profileId, communityId);
                if (communityMember == null)
                {
                    return NotFound("Community member not found in this group");
                }
                await _communityService.DeleteUserCommunityMemberAsync(communityMember);
                return Ok("Left community");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

        }
        [Authorize]
        [HttpPost("create-post")]
        public async Task<IActionResult> CreatePost([FromForm] CommunityPostCreateRequest request)
        {
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;

                if (string.IsNullOrEmpty(profileId))
                {
                    return BadRequest("User not found");
                }
                var group = await _communityService.GetCommunityByIdAsync(request.CommunityGroupId);
                if (group == null)
                {
                    return NotFound("Community not found");
                }
                string imageUrl = null;
                if (request.CommunityPostImageFile != null)
                {
                    var postParams = _cloudinaryService.CreateUploadParams(request.CommunityPostImageFile, CloudinaryFolderEnum.CommunityPostThumbnail.ToString());
                    imageUrl = await _cloudinaryService.UploadAsync(postParams, request.CommunityPostImageFile);
                }
                var userCommunityPost = new CommunityPost()
                {
                    CommunityPostId = Guid.NewGuid().ToString(),
                    CommunityPostTitle = request.CommunityPostTitle,
                    CommunityPostContent = request.CommunityPostContent,
                    CommunityPostImageUrl = imageUrl,
                    CommunityPostCreatedAt = DateTime.UtcNow,
                    CommunityGroupId = group.CommunityGroupId,
                    ProfileId = profileId,
                    
                };
                await _communityService.CreateCommunityPostAsync(userCommunityPost);
                return Created("", new { message = "Community post created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

        }
        [Authorize]
        [HttpPut("update-post/{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromForm] CommunityPostUpdateRequest updatedPost)
        {
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;

                if (string.IsNullOrEmpty(profileId))
                {
                    return BadRequest("User not found");
                }
                var group = await _communityService.GetCommunityByIdAsync(updatedPost.CommunityGroupId);
                if (group == null)
                {
                    return NotFound("Community not found");
                }
                string imageUrl = null;
                var existingPost = await _communityService.GetCommunityPostByIdAsync(id);

                if (existingPost == null)
                {
                    return NotFound("Community post not found");
                }
                if (updatedPost.CommunityPostImageFile != null)
                {
                    if (existingPost.CommunityPostImageUrl != null)
                    {
                        await _cloudinaryService.DeleteAsync(existingPost.CommunityPostImageUrl);
                    }
                    var postParams = _cloudinaryService.CreateUploadParams(updatedPost.CommunityPostImageFile, CloudinaryFolderEnum.CommunityPostThumbnail.ToString());
                    imageUrl = await _cloudinaryService.UploadAsync(postParams, updatedPost.CommunityPostImageFile);
                }
                existingPost.CommunityPostTitle = updatedPost.CommunityPostTitle ?? existingPost.CommunityPostTitle;
                existingPost.CommunityPostContent = updatedPost.CommunityPostContent ?? existingPost.CommunityPostContent;
                await _communityService.UpdateCommunityPostAsync(existingPost);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

        }

        [HttpGet("post-detail/{id}")]
        public async Task<IActionResult> GetPostDetail(string id)
        {
            var postDetail = await _communityService.GetCommunityPostByIdAsync(id);
            if (postDetail == null)
                return NotFound("Post not found");
            return Ok(new CommunityPostDetail()
            {
                CommunityGroupId = postDetail.CommunityGroupId,
                CommunityPostId = postDetail.CommunityPostId,
                CommunityPostTitle = postDetail.CommunityPostTitle,
                CommunityPostContent = postDetail.CommunityPostContent,
                CommunityPostImageUrl = postDetail.CommunityPostImageUrl,
                CommunityPostStatus = postDetail.CommunityPostStatus,
                CommunityPostCreatedAt = postDetail.CommunityPostCreatedAt,
                ProfileId = postDetail.ProfileId,
                UserName = postDetail?.Profile?.User?.FullName,
                ProfilePicture = postDetail?.Profile?.User?.ProfilePicture,
                UserId = postDetail.Profile?.UserId,
            });
        }


    }
}
