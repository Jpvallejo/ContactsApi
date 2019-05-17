using System.IO;

namespace WebApi.Models
{
    public class ContactViewModel
    {
        public string Name { get; set; }

        public string Company { get; set; }

        public string Email { get; set; }

        public string WorkPhone { get; set; }

        public string PersonalPhone { get; set; }
        
        public string Address { get; set; }
        public string Birthday { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
