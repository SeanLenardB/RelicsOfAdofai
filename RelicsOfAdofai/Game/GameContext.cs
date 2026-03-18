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
        public List<SkillNode> HandNodes = [];

        public SkillNode? CurrentSelectedNode = null;

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
            ChartCell? startingCell = null;
            foreach (var cell in this.CurrentChart.Cells)
            {
                cell.FluxIn = 0; cell.FluxOut = 0;
                if (cell.Type == ChartCell.CellType.Start) startingCell = cell;
            }
            Debug.Assert(startingCell is not null, "Every chart should have (at least, not impl yet) 1 starting cell!");

            // @todo: this 1 is hardcoded. It should depend on the chart.
            if (startingCell.FilledNode is not null) this.PropagateChartCell(this.CurrentChart, startingCell, new(1));
        }
        public void PropagateChartCell(Chart chart, ChartCell cell, CellPropagationPacket packet)
        {
            Debug.Assert(chart == this.CurrentChart, "You're trying to propagate a cell that is not in the current chart!");
            var node = cell.FilledNode;
            Debug.Assert(node is not null, "A packet is propagated to a cell that has no node inside it! Prune it in the first place!");
            switch (node.Type)
            {
                case SkillNode.NodeType.Connector_Opposite:
                    var outOffsetAngle = 0; if (node.IsFlipped) outOffsetAngle = 180 - outOffsetAngle;
                    outOffsetAngle += node.Rotation;
                    var outCell = chart.Cells.FirstOrDefault(c => c.Coords.IsEqual(cell.Coords + HexCoords.RotationUnit(outOffsetAngle)));
                    if (outCell is null) break;
                    var outEnergy = packet.Energy * node.ConnectorEfficiency;
                    if (outEnergy < CellPropagationPacket.MinEnergyThreshold) break;
                    this.PropagateChartCell(chart, outCell, new(outEnergy));
                    break;

                default: return;
            }
        }
        
        public class CellPropagationPacket
        {
            public static readonly double MinEnergyThreshold = 0.001;
            public static readonly double MaxEnergyThreshold = 10;
            public double Energy = 0;

            public CellPropagationPacket(double energy)
            {
                if (energy > MaxEnergyThreshold) energy = MaxEnergyThreshold;
                Debug.Assert(energy >= MinEnergyThreshold, "There is a packet with too little energy! You should prune it in the first place!");
                this.Energy = energy;
            }
        }
    }
}
