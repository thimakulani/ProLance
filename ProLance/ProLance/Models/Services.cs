using Plugin.CloudFirestore.Attributes;
namespace ProLance.Models
{
    public class Services
    {
        [Id]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public string Category { get; set; }    
    }
}
