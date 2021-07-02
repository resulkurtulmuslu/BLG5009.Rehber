using System.Collections.Generic;

namespace BLG5009.Rehber.WebApp.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string NickName { get; set; }
        public string Image { get; set; }

        public bool Star { get; set; }

        public ICollection<Telephone> Telephones { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Email> Emails { get; set; }

        public User()
        {
            Telephones = new List<Telephone>();
            Addresses = new List<Address>();
            Emails = new List<Email>();
        }
    }
}
