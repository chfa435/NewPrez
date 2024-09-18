using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewTiceAI.Models.Enums
{

    public enum OpportunityType
    {
        [Display(Name="New Business")]
        NewBusiness,
        [Display(Name = "Expansion")]
        Expansion,
        [Display(Name = "Renewal")]
        Renewal,
        [Display(Name = "Partner Deal")]
        PartnerDeal
    }
}
