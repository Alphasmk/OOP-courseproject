using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.Model
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GamePlatform> GamePlatforms { get; set; }
    }
}
