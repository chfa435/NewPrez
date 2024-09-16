using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models.Enums
{
    public enum OpportunityStage
    {
        [Display(Name = "BU GTM")]
        Stage_BU_GTM,
        [Display(Name = "Target")]
        Stage_Target,
        [Display(Name = "Re-Engage")]
        Stage_Re_engage,
        [Display(Name = "Pre-Intro")]
        Stage_Pre_intro,
        [Display(Name = "Pre-Proposal")]
        Stage_Pre_Proposal,
        [Display(Name = "Pre-Contrct")]
        Stage_Pre_Contract,
        [Display(Name = "Pre-Signature")]
        Stage_Pre_Signature,
        [Display(Name = "Closed (Won)")]
        Stage_Closed_Won,
        [Display(Name = "Closed (Lost)")]
        Stage_Closed_Lost,
    }
}
