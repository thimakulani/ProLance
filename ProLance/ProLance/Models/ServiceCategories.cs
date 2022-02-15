using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProLance.Models
{
    public class ServiceCategories
    {
        [Id]
        public string Id { get; set; }
        [MapTo("name")]
        public string Name { get; set; }
        [MapTo("description")]
        public string Description { get; set; }
        public string ImgUrl { get; set; }
    }
}
