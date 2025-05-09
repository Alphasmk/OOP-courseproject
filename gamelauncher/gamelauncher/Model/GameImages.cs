using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.Model
{
    public class GameImage
    {
        public int Id {  get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string ImagePath { get; set; }
    }
}
