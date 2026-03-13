using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Game
{
    public class GameContext
    {
        public int Seed = 0;
        public Chart CurrentChart = new();
        public List<Chart> Charts = [];
        public Random Random = new();

        public void RefreshSeed() => this.Random = new(this.Seed);

        public void StartGame()
        {
            this.Random = new(this.Seed);

            this.Charts = ChartCollection.ChartPool();
            this.CurrentChart = this.Charts[0];
        }
    }

    public class HexNode
    {

    }
}
