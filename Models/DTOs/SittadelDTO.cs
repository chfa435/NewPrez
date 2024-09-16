using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using NewTiceAI.Models.Enums;
using System.Text.Json.Serialization;

namespace NewTiceAI.Models.DTOs
{
    public class SittadelDTO
    {
        [JsonPropertyName("avatar")]
        public string? Avatar {  get; set; }
        [JsonPropertyName("contact_id")]
        public int? ContactId { get; set; }
        [JsonPropertyName("contact")]
        public string? ContactName { get; set; }
        [JsonPropertyName("email")]
        public string? ContactEmail { get; set; }
        [JsonPropertyName("comments")]
        public ICollection<ActionItemComment> Comments { get; set; } = new HashSet<ActionItemComment>();
        [JsonPropertyName("account_id")]
        public int? AccountId { get; set; }
        [JsonPropertyName("account")]
        public string? AccountName { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; } = 0M;
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("notes")]
        [DisplayName("Notes")]
        public string? Notes { get; set; }
        [JsonPropertyName("closed_date")]
        [DataType(DataType.Date)]
        [DisplayName("Date Closed")]
        public string? ClosedDate { get; set; }
        [JsonPropertyName("opportunity_type")]
        [DisplayName("Type")]
        public string? OpportunityType { get; set; }
        [JsonPropertyName("opportunity_forecast")]
        [DisplayName("Forecast")]
        public string? OpportunityForecastCategory { get; set; }
        [JsonPropertyName("opportunity_stage")]
        [DisplayName("Stage")]
        public string? OpportunityStage { get; set; }

    }
}
