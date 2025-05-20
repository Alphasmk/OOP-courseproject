using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.Model
{
    public class UserGameGroup
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public User User { get; set; }
        public List<UserGameGroupGame> UserGameGroupGames { get; set; }
    }
}
