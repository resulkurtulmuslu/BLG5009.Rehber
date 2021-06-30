using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

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
