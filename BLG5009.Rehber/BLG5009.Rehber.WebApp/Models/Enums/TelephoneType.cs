using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

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
