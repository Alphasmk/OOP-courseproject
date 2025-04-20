using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.Model
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double? SizeGB { get; set; }
        public string CoverImagePath { get; set; }
        public bool IsActive { get; set; }
        public ICollection<GameGenre> GameGenres { get; set; }
        public ICollection<GamePlatform> GamePlatforms { get; set; }
        public ICollection<GameGroup> GameGroups { get; set; }
    }
}
