using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Engine;

namespace RelicsOfAdofai.Game
{
    public class GameContext
    {
        public int Seed = 0;
        public Random Random = new();
        public double TotalTime = 0;

        public bool DebugMode = true;
        public double DeltaTime = 0.0;
        public Chart? CurrentChart = null;
        public List<Chart> Charts = [];
        public List<SkillNode> HandNodes = [];

        public SkillNode? CurrentSelectedNode = null;

        public void RefreshSeed() => this.Random = new(this.Seed);

        public void StartGame()
        {
            this.Random = new(this.Seed);

            this.Charts = ChartCollection.ChartPool();
            this.Charts.ForEach(c => c.AdjustGridToCenter());
            this.HandNodes = NodeCollection.StartingCollection();
            this.CurrentChart = this.Charts[0];
        }

        public void RecalculateCurrentChart(GuiContext guiContext)  // @cleanup: can we not pass this in because we only need to update a button???
        {
            Debug.Assert(this.CurrentChart is not null, "Cannot recalculate a null chart!");
            this.CurrentChart.FinalEnergy = 0;
            List<ChartCell> sourceCells = [];
            foreach (var cell in this.CurrentChart.Cells)
            {
                cell.FluxIn = 0; cell.FluxOut = 0;
                if (cell.Type == ChartCell.CellType.Source) sourceCells.Add(cell);
            }
            Debug.Assert(sourceCells is not null, "Every chart should have at least 1 source cell!");

            foreach (var sourceCell in sourceCells)
            {
                if (sourceCell.FilledNode is not null)
                    this.PropagateChartCell(this.CurrentChart, sourceCell, new(sourceCell, sourceCell.SourceEnergy));
            }

            if (this.CurrentChart.FinalEnergy > 0 && !guiContext.Buttons["attempt"].Enabled)
                guiContext.Buttons["attempt"].Enabled = true;
        }

        public void AttemptChart(GuiContext guiContext, Chart chart)
        {
            Debug.Assert(chart.FinalEnergy > 0, "Cannot spam hours! (is the predicate wrong?)");
            double clearChance = Math.Pow(
                                    Math.Exp(11.45 * Math.Pow(chart.FinalEnergy - chart.OptimalEnergy, 2)), 
                                    -4);  // Formula is stolen from Moni Labs, Sculk Vat probability impl.
            double roll = this.Random.NextDouble();
            if (roll <= clearChance)
            {
                this.TotalTime += 0.5;

                var success = $"Success with accuracy {96.0 + (4.0 * clearChance):0.00%}";
                var successTextExtent = Raylib.MeasureTextEx(Style.FontGeneral, success, Style.SizeSmall, 0);
                guiContext.FloatingMessages.Enqueue(new()
                {
                    Text = success,
                    Position =
                        Layout.RightBottom().Hpx(successTextExtent.Y).YVh(100).DYpx(-Style.HandHeight * 2)
                            .Wpx(successTextExtent.X).Xvw(100).DXpx(-successTextExtent.Y).Vect()
                });
            }
            else
            {
                var failProgress = (1.0 - roll) / (1.0 - clearChance);
                this.TotalTime += 0.5 * failProgress;

                var failText = $"Failed at {Math.Floor(failProgress)}";
                var failTextExtent = Raylib.MeasureTextEx(Style.FontGeneral, failText, Style.SizeSmall, 0);
                guiContext.FloatingMessages.Enqueue(new()
                {
                    Text = failText,
                    Position =
                        Layout.RightBottom().Hpx(failTextExtent.Y).YVh(100).DYpx(-Style.HandHeight * 2)
                            .Wpx(failTextExtent.X).Xvw(100).DXpx(-failTextExtent.Y).Vect()
                });
            }
        }

        public void PropagateChartCell(Chart chart, ChartCell cell, CellPropagationPacket packet)
        {
            if (this.DebugMode) Console.WriteLine($"{packet.From.Coords} -[{packet.Energy}]-> {cell.Coords}");
            Debug.Assert(chart == this.CurrentChart, "You're trying to propagate a cell that is not in the current chart!");
            var node = cell.FilledNode;
            Debug.Assert(node is not null, "A packet is propagated to a cell that has no node inside it! Prune it in the first place!");
            cell.FluxIn += packet.Energy;
            /// <see cref="GameRender.DrawFluxHint(GameContext, ChartCell)"/>
            switch (node.Type)  // @copypasta
            {
                case SkillNode.NodeType.Extractor_Single:
                    {
                        var outOffsetAngle = 0;
                        if (node.IsFlipped) outOffsetAngle = 180 - outOffsetAngle;
                        outOffsetAngle += node.Rotation;

                        if (packet.From.Type != ChartCell.CellType.Source) break;

                        var outCell = chart.Cells.FirstOrDefault(c => c.Coords.CoordsEqual(cell.Coords + HexCoords.RotationUnit(outOffsetAngle)));
                        if (outCell is null || outCell.FilledNode is null) break;

                        var outEnergy = packet.Energy * node.ExtractorMultiplier;
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

                        var outEnergy = packet.Energy * node.ConnectorMultiplier;
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

                        var outEnergy = packet.Energy * node.ConnectorMultiplier;
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

                        var outEnergy = packet.Energy * node.ConnectorMultiplier;
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
            public static readonly double MinEnergyThreshold = 0.01;
            public static readonly double MaxEnergyThreshold = 1000;

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
