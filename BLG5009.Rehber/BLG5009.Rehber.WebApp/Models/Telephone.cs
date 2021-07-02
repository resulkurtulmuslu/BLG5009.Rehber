using BLG5009.Rehber.WebApp.Models.Enums;

namespace BLG5009.Rehber.WebApp.Models
{
    public class Telephone : BaseModel
    {
        public string Number { get; set; }
        public TelephoneType Type { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
