using Contract.Common.Constant;
using Contract.Common.Enums;
using Contract.Dtos.Requests.Article;
using Contract.Dtos.Requests.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Interfaces;
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
        public CommunityController(ICommunityService communityService, IUserService userService, ICloudinaryService cloudinaryService)
        {
            _communityService = communityService;
            _userService = userService;
            _cloudinaryService = cloudinaryService;
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

            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
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
            return Ok(community);
        }
        
        [HttpPut("update-community/{id}")]
        public async Task<IActionResult> UpdateCommunity(string id, [FromForm] CommunityUpdateRequest updatedCommunity)
        {
         
            try
            {
                var existingCommunity = await _communityService.GetCommunityByIdAsync(id);
                if (existingCommunity == null) { 
                    return NotFound("Community not found");
                }
                existingCommunity.CommunityDescription = updatedCommunity.CommunityDescription ?? existingCommunity.CommunityDescription;
                existingCommunity.CommunityName = updatedCommunity.CommunityName ?? existingCommunity.CommunityName;
                if (updatedCommunity.CommunityStatus.HasValue)
                {
                    existingCommunity.CommunityStatus = updatedCommunity.CommunityStatus.ToString() ?? existingCommunity.CommunityStatus;
                };
                await _communityService.UpdateUserCommunityAsync(existingCommunity);
                return NoContent();
            }
           
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }
        [Authorize]
        [HttpPost("join-community")]
        public async Task<IActionResult> JoinCommunity([FromForm] CommunityMemberCreateRequest request)
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
                return StatusCode(500, ex.Message);
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
                    var postParams =  _cloudinaryService.CreateUploadParams(request.CommunityPostImageFile, CloudinaryFolderEnum.CommunityPostThumbnail.ToString());
                    imageUrl= await _cloudinaryService.UploadAsync(postParams, request.CommunityPostImageFile);
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
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
            }

        }

    }
}
