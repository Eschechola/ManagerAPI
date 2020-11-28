using System.Collections.Generic;
using UserManager.Domain.Entities;
using UserManager.Domain.Validators;

namespace UserManager.Services.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
 