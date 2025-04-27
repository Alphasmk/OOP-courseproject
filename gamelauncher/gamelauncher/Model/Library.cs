using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.Model
{
    public class Library
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
        public DateTime? DateOfPurchase { get; set; }
        public ICollection<GameGroup> GameGroups { get; set; }
    }
}
