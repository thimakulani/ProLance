using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProLance.Models
{
    public class User
    {
        [Id]
        public string Id { get; set; }
        [MapTo("FirstName")]
        public string FirstName { get; set; }
        [MapTo("LastName")]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string Role { get; set; }
    }
}
