﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLG5009.Rehber.WebApp.Models
{
    public class Address : BaseModel
    {
        public string AddressText { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
