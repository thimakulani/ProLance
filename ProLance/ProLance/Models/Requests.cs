using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProLance.Models
{
    public class Requests
    {
        [Id]
        public string Id { get; set; }
        public string Name { get; set; }
        [MapTo("S_ID")]
        public string SiD { get; set; }
        public string Uid { get; set; }
        public string Dates { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        
    }
}
