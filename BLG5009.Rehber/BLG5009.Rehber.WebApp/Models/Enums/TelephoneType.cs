using System.ComponentModel;

namespace BLG5009.Rehber.WebApp.Models.Enums
{
    public enum TelephoneType
    {
        [Description("Ev")]
        Home,
        [Description("İş")]
        Business,
        [Description("Cep")]
        Mobile,
        [Description("Diğer")]
        Other
    }
}
