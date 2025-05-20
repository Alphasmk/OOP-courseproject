using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.Model
{
    public class UserGameGroupGame
    {
        public int UserGameGroupId { get; set; }
        public int GameId { get; set; }

        public UserGameGroup UserGameGroup { get; set; }
        public Game Game { get; set; }
    }
}
