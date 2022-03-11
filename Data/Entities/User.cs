using Microsoft.AspNetCore.Identity;

namespace TEST.Data.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Article> Articles { get; set; }
    }
}
