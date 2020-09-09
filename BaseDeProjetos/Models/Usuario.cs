using Microsoft.AspNetCore.Identity;

namespace BaseDeProjetos.Models
{
    public class Usuario : IdentityUser
    {
        public Casa Casa { get; set; }
    }
}
