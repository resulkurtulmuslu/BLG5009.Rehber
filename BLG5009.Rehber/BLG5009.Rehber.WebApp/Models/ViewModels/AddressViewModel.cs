using BLG5009.Rehber.WebApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLG5009.Rehber.WebApp.Models.ViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        public string AddressText { get; set; }
        public AddressType Type { get; set; }
        public string TypeText { get; set; }

        public int UserId { get; set; }
    }
}
