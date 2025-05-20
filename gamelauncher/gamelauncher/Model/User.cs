using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal Balance { get; set; }
        public bool IsBlocked { get; set; }
        public int SnakeRecord { get; set; }

        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<Library> Library { get; set; }
        public ICollection<UserGameGroup> UserGameGroups { get; set; }
    }
}
