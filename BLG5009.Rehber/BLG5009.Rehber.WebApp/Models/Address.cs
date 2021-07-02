using BLG5009.Rehber.WebApp.Models.Enums;

namespace BLG5009.Rehber.WebApp.Models
{
    public class Address : BaseModel
    {
        public string AddressText { get; set; }
        public AddressType Type { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
