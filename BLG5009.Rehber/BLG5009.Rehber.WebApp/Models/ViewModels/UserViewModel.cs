using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLG5009.Rehber.WebApp.Models.ViewModels
{
    public class UserViewModel : User
    {
        public IFormFile File { get; set; }
    }
}
