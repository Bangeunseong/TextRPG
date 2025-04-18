using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class GameData
    {
        public int GroundLevel { get; set; }
        public int Quota { get; set; }

        public GameState GameState { get; set; }
        public GameTime GameTime { get; set; }

        public List<Consumables> Exposables { get; set; } = new();
    }
}
