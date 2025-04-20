using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.Model
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GameGroup> GameGroups { get; set; }
    }
}
