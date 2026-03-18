using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Engine;

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
                    Type = SkillNode.NodeType.Connector_Opposite },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Opposite },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Opposite },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Interval },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Interval },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Adjacent },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = SkillNode.NodeType.Connector_Adjacent },
            ];
        }
    }

    public class SkillNode
    {
        public string Name = "";
        public string Description = "";

        // @clanup: this is a fat struct. Might need some polymorphism?
        public int Rotation { get; set { field = value % 360; if (field < 0) field += 360; } } = 0;
        public bool IsFlipped = false;
        public double ConnectorEfficiency = 0.9;
        public NodeType Type = NodeType.Connector_Opposite;
        public enum NodeType
        {
            Connector_Opposite,
            Connector_Interval,
            Connector_Adjacent,
        }

        public string ResourceKey()
        {
            return this.Type switch
            {
                NodeType.Connector_Opposite => "node-connector-opposite",
                NodeType.Connector_Interval => "node-connector-interval",
                NodeType.Connector_Adjacent => "node-connector-adjacent",
                _ => "bg",  // probably unnecessary
            };
        }

        public readonly Vector2[] BoundingBox =
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
