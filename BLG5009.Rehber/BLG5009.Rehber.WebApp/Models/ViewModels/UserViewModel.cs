using Microsoft.AspNetCore.Http;

namespace BLG5009.Rehber.WebApp.Models.ViewModels
{
    public class UserViewModel : User
    {
        public IFormFile File { get; set; }
    }
}
