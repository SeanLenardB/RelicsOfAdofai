using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Game
{
    public class GameContext
    {
        public int Seed = 0;
        public Random Random = new();

        public Chart? CurrentChart = null;
        public List<Chart> Charts = [];
        public List<HexNode> HandNodes = [];

        public HexNode? CurrentlySelectedNode = null;

        public void RefreshSeed() => this.Random = new(this.Seed);

        public void StartGame()
        {
            this.Random = new(this.Seed);

            this.Charts = ChartCollection.ChartPool();
            this.HandNodes = NodeCollection.StartingCollection();
            this.CurrentChart = this.Charts[0];
        }

        public void RecalculateCurrentChart()
        {
            Debug.Assert(this.CurrentChart is not null, "Cannot recalculate a null chart!");
        }
    }
}
