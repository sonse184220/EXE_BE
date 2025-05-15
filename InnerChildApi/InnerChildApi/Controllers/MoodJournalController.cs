using Contract.Common.Constant;
using Contract.Dtos.Requests.MoodJournal;
using Contract.Dtos.Responses.MoodJournal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Interfaces;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/moodjournal")]

    [ApiController]
    public class MoodJournalController : ControllerBase
    {
        private readonly IMoodJournalService _moodJournalService;
        public MoodJournalController(IMoodJournalService moodJournalService)
        {
            _moodJournalService = moodJournalService;
        }

        [HttpPost("create-type")]
        public async Task<IActionResult> CreateMoodJournalType([FromBody] MoodJournalTypeCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var moodJournalType = new MoodJournalType()
                {
                    MoodJournalTypeId = Guid.NewGuid().ToString(),
                    MoodJournalTypeName = request.MoodJournalTypeName,
                };
                await _moodJournalService.CreateMoodJournalTypeAsync(moodJournalType);
                return Created("", new { message = "Mood jornal type created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong " + ex.Message);
            }

        }

        [HttpGet("all-types")]
        public async Task<IActionResult> GetAllMoodJournalType()
        {
            var moodJournalTypes = await _moodJournalService.GetAllMoodJournalTypeAsync();
            return Ok(moodJournalTypes);
        }
        [HttpPut("update-type/{id}")]
        public async Task<IActionResult> UpdateMoodJournalType(string id, [FromBody] MoodJournalTypeCreateRequest updatedMoodJournalType)
        {
            try
            {
                var existingMoodJournalType = await _moodJournalService.GetMoodJournalTypeByIdAsync(id);
                if (existingMoodJournalType == null)
                {
                    return NotFound("Mood journal type not found");
                }
            ;
                existingMoodJournalType.MoodJournalTypeName = updatedMoodJournalType.MoodJournalTypeName;
                await _moodJournalService.UpdateMoodJournalTypeAsync(existingMoodJournalType);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GetAllMoodJournal()
        {
            var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
            if (profileId == null)
            {
                return NotFound();
            }
            var moodLogHistory = await _moodJournalService.GetAllMoodJournalByProfileIdAsync(profileId);

            var result = moodLogHistory.Select(x => new MoodJournalResponse()
            {
                MoodJournalId = x.MoodJournalId,
                MoodJournalTitle = x.MoodJournalTitle,
                MoodJournalEmotion = x.MoodJournalEmotion,
                MoodJournalDescription = x.MoodJournalDescription,
                MoodJournalCreatedAt = x.MoodJournalCreatedAt,
                MoodJournalTypeId = x.MoodJournalTypeId,
                ProfileId = x.ProfileId,
                MoodJournalTypeName = x.MoodJournalType?.MoodJournalTypeName
            });
            return Ok(result);
        }
        [Authorize]
        [HttpPost("add-log")]
        public async Task<IActionResult> CreateMoodJournalLog([FromBody] MoodJournalCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
                if (profileId == null)
                {
                    return NotFound();
                }
                var moodJournalType = await _moodJournalService.GetMoodJournalTypeByIdAsync(request.MoodJournalTypeId);
                if (moodJournalType == null)
                {
                    return NotFound("Mood journal type not found");
                }
                var moodJournal = new MoodJournal()
                {
                    MoodJournalId = Guid.NewGuid().ToString(),
                    MoodJournalTitle = request.MoodJournalTitle,
                    MoodJournalEmotion = request.MoodJournalEmotion.ToString(),
                    MoodJournalDescription = request.MoodJournalDescription,
                    MoodJournalCreatedAt = DateTime.UtcNow,
                    MoodJournalTypeId = request.MoodJournalTypeId,
                    ProfileId = profileId,
                };
                await _moodJournalService.CreateMoodJournalAsync(moodJournal);
                return Created("", new { message = "Mood journal log created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMoodJournal(string id, [FromBody] MoodJournalUpdateRequest updatedMoodJournal)
        {
            try
            {
                var existingMoodJournal = await _moodJournalService.GetMoodJournalByIdAsync(id);
                if (existingMoodJournal == null)
                {
                    return NotFound("Mood journal not found");
                }
                existingMoodJournal.MoodJournalTitle = updatedMoodJournal.MoodJournalTitle ?? existingMoodJournal.MoodJournalTitle;
                if (updatedMoodJournal.MoodJournalEmotion != null)
                {
                    existingMoodJournal.MoodJournalEmotion = updatedMoodJournal.MoodJournalEmotion.ToString() ?? existingMoodJournal.MoodJournalEmotion;
                }
                existingMoodJournal.MoodJournalDescription = updatedMoodJournal.MoodJournalDescription ?? existingMoodJournal.MoodJournalDescription;
                if (updatedMoodJournal.MoodJournalTypeId != null)
                {
                    var moodJournalType = await _moodJournalService.GetMoodJournalTypeByIdAsync(updatedMoodJournal.MoodJournalTypeId);
                    if (moodJournalType == null)
                    {
                        return NotFound("Mood journal type not found");
                    }
                    existingMoodJournal.MoodJournalTypeId = updatedMoodJournal.MoodJournalTypeId ?? existingMoodJournal.MoodJournalTypeId;
                }
                await _moodJournalService.UpdateMoodJournalAsync(existingMoodJournal);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
