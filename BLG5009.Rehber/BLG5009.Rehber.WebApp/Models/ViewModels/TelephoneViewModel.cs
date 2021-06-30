using BLG5009.Rehber.WebApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLG5009.Rehber.WebApp.Models.ViewModels
{
    public class TelephoneViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; }
        public TelephoneType Type { get; set; }
        public string TypeText { get; set; }

        public int UserId { get; set; }
    }
}
