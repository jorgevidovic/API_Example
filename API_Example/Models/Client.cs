using Microsoft.AspNetCore.Mvc;

namespace API_Example.Models
{
    public class Client
    {
        public string? id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
