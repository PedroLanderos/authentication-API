using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Entities
{
    //defines the main entity of the authentication api 
    public class UserEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? Adress { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    }
}
