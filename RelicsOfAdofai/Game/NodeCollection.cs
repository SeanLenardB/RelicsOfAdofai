using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Game
{
    public class NodeCollection
    {
        // @enhance: we might want different startup hand sets,
        // such as different character/player gives different skillsets.
        public static List<HexNode> StartingCollection()
        {
            // @cleanup: we might want a "nodeStack" style impl like minecraft does for items.
            // but I don't really see the benefit being that much,
            // so for now, this is very shitty and we'll spam nodes.
            return
            [
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = HexNode.NodeType.Connector_Opposite },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = HexNode.NodeType.Connector_Opposite },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = HexNode.NodeType.Connector_Opposite },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = HexNode.NodeType.Connector_Interval },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = HexNode.NodeType.Connector_Interval },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = HexNode.NodeType.Connector_Adjacent },
                new() {
                    Name = "Normal Play",
                    Description = "Connects the track in the specified direction.",
                    Type = HexNode.NodeType.Connector_Adjacent },
            ];
        }
    }

    public class HexNode
    {
        public string Name = "";
        public string Description = "";

        // @todo: change this to an icon.
        public Color Color = Color.DarkBlue;

        public double ConnectorEfficiency = 0.9;
        public NodeType Type = NodeType.Connector_Opposite;
        public enum NodeType
        {
            Connector_Opposite,
            Connector_Interval,
            Connector_Adjacent,
        }
    }
}
