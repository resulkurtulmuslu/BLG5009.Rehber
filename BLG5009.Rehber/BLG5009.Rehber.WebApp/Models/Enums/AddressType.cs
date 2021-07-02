using System.ComponentModel;

namespace BLG5009.Rehber.WebApp.Models.Enums
{
    public enum AddressType
    {
        [Description("Ev")]
        Home,
        [Description("İş")]
        Business,
        [Description("Diğer")]
        Other
    }
}
