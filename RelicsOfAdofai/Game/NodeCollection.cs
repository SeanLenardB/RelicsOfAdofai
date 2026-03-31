using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Engine;
using static RelicsOfAdofai.Game.GameContext;

namespace RelicsOfAdofai.Game
{
    public class NodeCollection
    {
        // @enhance: we might want different startup hand sets,
        // such as different character/player gives different skillsets.
        public static List<SkillNode> StartingCollection()
        {
            // @cleanup: we might want a "nodeStack" style impl like minecraft does for items.
            // but I don't really see the benefit being that much,
            // so for now, this is very shitty and we'll spam nodes.
            return
            [
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Opposite,
                    PassOnMultiplier = 0.8, PassOnMultiplierMinimum = 0.85, PassOnMultiplierMaximum = 0.75, PassOnMultiplierTweakAmount = 0.05
                },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Opposite,
                    PassOnMultiplier = 0.8, PassOnMultiplierMinimum = 0.75, PassOnMultiplierMaximum = 0.85, PassOnMultiplierTweakAmount = 0.05
                },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Opposite,
                    PassOnMultiplier = 0.8, PassOnMultiplierMinimum = 0.75, PassOnMultiplierMaximum = 0.85, PassOnMultiplierTweakAmount = 0.05
                },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Interval,
                    PassOnMultiplier = 0.8, PassOnMultiplierMinimum = 0.75, PassOnMultiplierMaximum = 0.85, PassOnMultiplierTweakAmount = 0.05
                },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Interval,
                    PassOnMultiplier = 0.8, PassOnMultiplierMinimum = 0.75, PassOnMultiplierMaximum = 0.85, PassOnMultiplierTweakAmount = 0.05
                },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Adjacent,
                    PassOnMultiplier = 0.8, PassOnMultiplierMinimum = 0.75, PassOnMultiplierMaximum = 0.85, PassOnMultiplierTweakAmount = 0.05
                },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Adjacent,
                    PassOnMultiplier = 0.8, PassOnMultiplierMinimum = 0.75, PassOnMultiplierMaximum = 0.85, PassOnMultiplierTweakAmount = 0.05
                },
                new() {
                    Name = "Normal Starter",
                    Description = "Provide the circuit with the energy from the source.",
                    Type = SkillNode.NodeType.Extractor_Single,
                    PassOnMultiplier = 1.0, PassOnMultiplierMinimum = 1.0, PassOnMultiplierMaximum = 1.0
                },
                new() {
                    Name = "Normal Finish",
                    Description = "Accepts the energy from all sides and output to the circuit.",
                    Type = SkillNode.NodeType.Receiver_Neighbor },
            ];
        }
    }

    public class SkillNode
    {
        public string Name = "";
        public string Description = "";
        public int Rotation { get; set { field = value % 360; if (field < 0) field += 360; } } = 0;
        public bool IsFlipped = false;



        // @cleanup: The following parameters are part of the fat struct. We might want polymorphism later.
        public double PassOnMultiplier { get; set { field = Math.Clamp(value, this.PassOnMultiplierMinimum, this.PassOnMultiplierMaximum); } } = 0;
        public double PassOnMultiplierMinimum = CellPropagationPacket.MinEnergyThreshold;
        public double PassOnMultiplierMaximum = CellPropagationPacket.MaxEnergyThreshold;
        public double PassOnMultiplierTweakAmount = 0;



        public NodeType Type = NodeType.Connector_Opposite;
        public enum NodeType
        {
            Extractor_Single,

            Connector_Opposite,
            Connector_Interval,
            Connector_Adjacent,

            Receiver_Neighbor,
        }

        public string ResourceKey()
        {
            return this.Type switch
            {
                NodeType.Extractor_Single => "node-extractor-single",

                NodeType.Connector_Opposite => "node-connector-opposite",
                NodeType.Connector_Interval => "node-connector-interval",
                NodeType.Connector_Adjacent => "node-connector-adjacent",

                NodeType.Receiver_Neighbor => "node-receiver-neighbor",

                _ => "bg",  // probably unnecessary
            };
        }

        public static readonly Vector2[] BoundingBox =
        [
            new((float)(Style.NodeInHandRadius * Style.ConstSqrtThreeOverTwo), -(float)(0.5 * Style.NodeInHandRadius)),
            new((float)(Style.NodeInHandRadius * Style.ConstSqrtThreeOverTwo), (float)(0.5 * Style.NodeInHandRadius)),
            new(0, Style.NodeInHandRadius),
            new(-(float)(Style.NodeInHandRadius * Style.ConstSqrtThreeOverTwo), (float)(0.5 * Style.NodeInHandRadius)),
            new(-(float)(Style.NodeInHandRadius * Style.ConstSqrtThreeOverTwo), -(float)(0.5 * Style.NodeInHandRadius)),
            new(0, -Style.NodeInHandRadius),
        ];
        public bool IsHover = false;
        public bool IsUsed = false;
    }
}
