using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models.Enums
{
    public enum OpportunityForecastCategory
    {
        [Display(Name ="Pipeline")]
        Pipeline,
        [Display(Name = "Best Case")]
        BestCase,
        [Display(Name = "COmmit")]
        Commit
    }
}
