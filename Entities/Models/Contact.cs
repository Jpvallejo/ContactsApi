using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string WorkPhone { get; set; }
        public string PersonalPhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ProfilePictureType { get; set; }
        public bool? Active { get; set; }
    }
}
