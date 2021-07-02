using BLG5009.Rehber.WebApp.Models.Enums;

namespace BLG5009.Rehber.WebApp.Models
{
    public class Email : BaseModel
    {
        public string EmailAddress { get; set; }
        public EmailType Type { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
