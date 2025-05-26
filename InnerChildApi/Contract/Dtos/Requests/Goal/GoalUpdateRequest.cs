namespace Contract.Dtos.Requests.Goal
{
    public class GoalUpdateRequest
    {
        public string? GoalTitle { get; set; }
        public string? GoalDescription { get; set; }

        public string? GoalType { get; set; }

        public DateTime? GoalStartDate { get; set; }

        public DateTime? GoalEndDate { get; set; }

        public int? GoalTargetCount { get; set; }

        public int? GoalPeriodDays { get; set; }

        public string? GoalStatus { get; set; }

        public DateTime? GoalCompletedAt { get; set; }
    }
}
