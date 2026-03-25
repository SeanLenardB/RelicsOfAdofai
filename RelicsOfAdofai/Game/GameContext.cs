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

        public bool DebugMode = true;
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
            this.CurrentChart.FinalEnergy = 0;
            ChartCell? startingCell = null;
            foreach (var cell in this.CurrentChart.Cells)
            {
                cell.FluxIn = 0; cell.FluxOut = 0;
                if (cell.Type == ChartCell.CellType.Start) startingCell = cell;
            }
            Debug.Assert(startingCell is not null, "Every chart should have (at least, not impl yet) 1 starting cell!");

            // @todo: this 1 is hardcoded. It should depend on the chart.
            if (startingCell.FilledNode is not null)
                this.PropagateChartCell(this.CurrentChart, startingCell, new(startingCell, 1));
        }
        public void PropagateChartCell(Chart chart, ChartCell cell, CellPropagationPacket packet)
        {
            if (this.DebugMode) Console.WriteLine($"{packet.From.Coords} -[{packet.Energy}]-> {cell.Coords}");
            Debug.Assert(chart == this.CurrentChart, "You're trying to propagate a cell that is not in the current chart!");
            var node = cell.FilledNode;
            Debug.Assert(node is not null, "A packet is propagated to a cell that has no node inside it! Prune it in the first place!");
            cell.FluxIn += packet.Energy;
            switch (node.Type)
            {
                case SkillNode.NodeType.Extractor_Single:
                    {
                        var outOffsetAngle = 0;
                        if (node.IsFlipped) outOffsetAngle = 180 - outOffsetAngle;
                        outOffsetAngle += node.Rotation;

                        if (packet.From.Type != ChartCell.CellType.Start) break;

                        var outCell = chart.Cells.FirstOrDefault(c => c.Coords.CoordsEqual(cell.Coords + HexCoords.RotationUnit(outOffsetAngle)));
                        if (outCell is null || outCell.FilledNode is null) break;

                        var outEnergy = packet.Energy * node.ConnectorEfficiency;
                        if (outEnergy < CellPropagationPacket.MinEnergyThreshold) break;
                        cell.FluxOut += outEnergy; this.PropagateChartCell(chart, outCell, new(cell, outEnergy));
                        break;
                    }

                case SkillNode.NodeType.Connector_Opposite:
                    {
                        var inOffsetAngle = 180; var outOffsetAngle = 0;
                        if (node.IsFlipped) { inOffsetAngle = 180 - inOffsetAngle; outOffsetAngle = 180 - outOffsetAngle; }
                        inOffsetAngle += node.Rotation; outOffsetAngle += node.Rotation;

                        if (!packet.From.Coords.CoordsEqual(cell.Coords + HexCoords.RotationUnit(inOffsetAngle))) break;

                        var outCell = chart.Cells.FirstOrDefault(c => c.Coords.CoordsEqual(cell.Coords + HexCoords.RotationUnit(outOffsetAngle)));
                        if (outCell is null || outCell.FilledNode is null) break;

                        var outEnergy = packet.Energy * node.ConnectorEfficiency;
                        if (outEnergy < CellPropagationPacket.MinEnergyThreshold) break;
                        cell.FluxOut += outEnergy; this.PropagateChartCell(chart, outCell, new(cell, outEnergy));
                        break;
                    }

                case SkillNode.NodeType.Connector_Interval:
                    {
                        var inOffsetAngle = 180; var outOffsetAngle = 60;
                        if (node.IsFlipped) { inOffsetAngle = 180 - inOffsetAngle; outOffsetAngle = 180 - outOffsetAngle; }
                        inOffsetAngle += node.Rotation; outOffsetAngle += node.Rotation;

                        if (!packet.From.Coords.CoordsEqual(cell.Coords + HexCoords.RotationUnit(inOffsetAngle))) break;

                        var outCell = chart.Cells.FirstOrDefault(c => c.Coords.CoordsEqual(cell.Coords + HexCoords.RotationUnit(outOffsetAngle)));
                        if (outCell is null || outCell.FilledNode is null) break;

                        var outEnergy = packet.Energy * node.ConnectorEfficiency;
                        if (outEnergy < CellPropagationPacket.MinEnergyThreshold) break;
                        cell.FluxOut += outEnergy; this.PropagateChartCell(chart, outCell, new(cell, outEnergy));
                        break;
                    }

                case SkillNode.NodeType.Connector_Adjacent:
                    {
                        var inOffsetAngle = 180; var outOffsetAngle = 120;
                        if (node.IsFlipped) { inOffsetAngle = 180 - inOffsetAngle; outOffsetAngle = 180 - outOffsetAngle; }
                        inOffsetAngle += node.Rotation; outOffsetAngle += node.Rotation;

                        if (!packet.From.Coords.CoordsEqual(cell.Coords + HexCoords.RotationUnit(inOffsetAngle))) break;

                        var outCell = chart.Cells.FirstOrDefault(c => c.Coords.CoordsEqual(cell.Coords + HexCoords.RotationUnit(outOffsetAngle)));
                        if (outCell is null || outCell.FilledNode is null) break;

                        var outEnergy = packet.Energy * node.ConnectorEfficiency;
                        if (outEnergy < CellPropagationPacket.MinEnergyThreshold) break;
                        cell.FluxOut += outEnergy; this.PropagateChartCell(chart, outCell, new(cell, outEnergy));
                        break;
                    }

                case SkillNode.NodeType.Receiver_Neighbor:
                    {
                        if (!HexCoords.Directions.Any(d => (cell.Coords + d).CoordsEqual(packet.From.Coords))) break;
                        chart.FinalEnergy += packet.Energy;
                        break;
                    }

                default: Debug.Assert(false, "Discriminated union!"); break;
            }
        }
        
        public class CellPropagationPacket
        {
            public static readonly double MinEnergyThreshold = 0.001;
            public static readonly double MaxEnergyThreshold = 10;

            public CellPropagationPacket(ChartCell from, double energy)
            {
                if (energy > MaxEnergyThreshold) energy = MaxEnergyThreshold;
                Debug.Assert(energy >= MinEnergyThreshold, "There is a packet with too little energy! You should prune it in the first place!");
                this.Energy = energy;
                this.From = from;
            }
            public double Energy;
            public ChartCell From;
        }
    }
}
