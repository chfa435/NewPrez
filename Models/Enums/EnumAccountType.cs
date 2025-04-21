using System.ComponentModel;

namespace NewTiceAI.Models.Enums
{
    public enum EnumAccountType
    {
        [Description("Customer")]
        Customer,
        
        [Description("Competitor")]
        Competitor,
        
        [Description("Former Customer")]
        FormerCustomer,
        
        [Description("Partner")]
        Partner,
        
        [Description("Prospect")]
        Prospect,
        
        [Description("Other")]
        Other
    }
} 