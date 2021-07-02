using System.ComponentModel;

namespace BLG5009.Rehber.WebApp.Models.Enums
{
    public enum EmailType
    {
        [Description("Kişisel")]
        Personal,
        [Description("İş")]
        Business,
        [Description("Diğer")]
        Other
    }
}
