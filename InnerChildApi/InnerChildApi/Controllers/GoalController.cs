using Contract.Common.Constant;
using Contract.Dtos.Requests.Goal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InnerChildApi.Controllers
{
    [Route("innerchild/goal")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly ILogger<GoalController> _logger;
        public GoalController(IGoalService goalService, ILogger<GoalController> logger)
        {
            _goalService = goalService;
            _logger = logger;
        }
        [Authorize]
        [HttpGet("get-own-goals")]
        public async Task<IActionResult> GetAllOwnGoals()
        {
            var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
            if (string.IsNullOrEmpty(profileId))
            {
                return NotFound();
            }
            var ownGoals = await _goalService.GetAllOwnGoalsAsync(profileId);
            return Ok(ownGoals);
        }
        [Authorize]
        [HttpGet("detail/{goalId}")]
        public async Task<IActionResult> GetGoalById(string goalId)
        {
            var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
            if (string.IsNullOrEmpty(profileId))
            {
                return NotFound();
            }
            var goal = await _goalService.GetGoalByIdAsync(goalId, profileId);
            return Ok(goal);
        }
        [Authorize]
        [HttpPost("create-own-goal")]
        public async Task<IActionResult> CreateOwnGoal([FromBody] GoalCreateRequest request)
        {

            var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
            if (string.IsNullOrEmpty(profileId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var goal = new Goal()
            {
                GoalId = Guid.NewGuid().ToString(),
                GoalCreatedAt = DateTime.UtcNow,
                GoalUpdatedAt = DateTime.UtcNow,
                GoalTitle = request.GoalTitle,
                GoalDescription = request.GoalDescription,
                GoalType = request.GoalType,
                GoalStartDate = request.GoalStartDate,
                GoalEndDate = request.GoalEndDate,
                GoalTargetCount = request.GoalTargetCount,
                GoalPeriodDays = request.GoalPeriodDays,
                GoalStatus = request.GoalStatus,
                GoalCompletedAt = request.GoalCompletedAt,
                ProfileId = profileId,
            };
            try
            {
                await _goalService.CreateGoalAsync(goal);
                return Created("", new { message = "Goal created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating goal");
                return StatusCode(500);
            }
        }
        [Authorize]
        [HttpPut("update-own-goal/{goalId}")]
        public async Task<IActionResult> UpdateOwnGoal(string goalId, [FromBody] GoalUpdateRequest request)
        {
            var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
            if (string.IsNullOrEmpty(profileId))
            {
                return NotFound();
            }


            try
            {
                var existingGoal = await _goalService.GetGoalByIdAsync(goalId, profileId);
                if (existingGoal == null)
                {
                    return NotFound("Goal not found");
                }
                existingGoal.GoalTitle = request.GoalTitle ?? existingGoal.GoalTitle;
                existingGoal.GoalDescription = request.GoalDescription ?? existingGoal.GoalDescription;
                existingGoal.GoalType = request.GoalType ?? existingGoal.GoalType;
                existingGoal.GoalStartDate = request.GoalStartDate ?? existingGoal.GoalStartDate;
                existingGoal.GoalEndDate = request.GoalEndDate ?? existingGoal.GoalEndDate;
                existingGoal.GoalTargetCount = request.GoalTargetCount ?? existingGoal.GoalTargetCount;
                existingGoal.GoalPeriodDays = request.GoalPeriodDays ?? existingGoal.GoalPeriodDays;
                existingGoal.GoalStatus = request.GoalStatus ?? existingGoal.GoalStatus;
                existingGoal.GoalUpdatedAt = DateTime.UtcNow;
                existingGoal.GoalCompletedAt = request.GoalCompletedAt ?? existingGoal.GoalCompletedAt;
                await _goalService.UpdateGoalAsync(existingGoal);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating goal");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpDelete("delete-own-goal/{goalId}")]
        public async Task<IActionResult> DeleteOwnGoal(string goalId)
        {
            var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
            if (string.IsNullOrEmpty(profileId))
            {
                return NotFound();
            }
            try
            {
                var goal = await _goalService.GetGoalByIdAsync(goalId, profileId);
                if (goal == null)
                {
                    return NotFound("Goal not found");
                }
                await _goalService.DeleteGoalAsync(goal);
                return Ok($"Goal with id {goalId} deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating goal");
                return StatusCode(500);
            }
        }
    }
}

