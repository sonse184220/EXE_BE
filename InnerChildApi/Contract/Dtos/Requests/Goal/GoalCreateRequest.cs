using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Goal
{
    public class GoalCreateRequest
    {
        [Required(ErrorMessage = "Goal title is required")]
        public string GoalTitle { get; set; }
        [Required(ErrorMessage = "Goal description is required")]
        public string GoalDescription { get; set; }

        public string? GoalType { get; set; }

        public DateTime? GoalStartDate { get; set; }

        public DateTime? GoalEndDate { get; set; }

        public int? GoalTargetCount { get; set; }

        public int? GoalPeriodDays { get; set; }

        public string? GoalStatus { get; set; }

        public DateTime? GoalCompletedAt { get; set; }
    }
}
