using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Engine;

namespace RelicsOfAdofai.Game
{
    public class ChartCollection
    {
        public static List<Chart> ChartPool()
        {
            return
            [
                new() {
                    SongInfo = "The Final Descent of Quartrond",
                    Creator = "SeanLB",
                    IconColor = Color.DarkGray,
                    Cells = [
                        new(0, 0, ChartCell.CellType.Start),
                        new(1, 0), new(2, -1), new(1, -1), new(3, -2),
                        new(3, -1, ChartCell.CellType.End)]},
            ];
        }
    }

    public class Chart
    {
        public string SongInfo = "";
        public string Creator = "";
        public Color IconColor = Color.SkyBlue;  // @todo: Change this to an actual thumbnail or difficulty icon or something

        public List<ChartCell> Cells = [];
        public Vector2 HexOrigin = Layout.CenterCenter().Hpx(1).Wpx(1).YVh(50).Xvw(50).Vect();  // @todo: auto adjust the grid to the center.
    }

    public class ChartCell(double q, double r, ChartCell.CellType type = ChartCell.CellType.Normal)
    {
        public HexCoords Coords = new(q, r);
        public CellType Type = type;
        public enum CellType
        {
            Normal,
            Start,
            End,
        }

        public bool IsHover = false;
    }
    public struct HexCoords(double q, double r)
    {
        public double Q = q;
        public double R = r;
        public readonly double S => -this.Q - this.R;
        public readonly Vector2[] BoundingBox =
        [
            new((float)(Style.HexCellDrawRadius * Style.ConstSqrtThreeOverTwo), -(float)(0.5 * Style.HexCellDrawRadius)),
            new((float)(Style.HexCellDrawRadius * Style.ConstSqrtThreeOverTwo), (float)(0.5 * Style.HexCellDrawRadius)),
            new(0, Style.HexCellDrawRadius),
            new(-(float)(Style.HexCellDrawRadius * Style.ConstSqrtThreeOverTwo), (float)(0.5 * Style.HexCellDrawRadius)),
            new(-(float)(Style.HexCellDrawRadius * Style.ConstSqrtThreeOverTwo), -(float)(0.5 * Style.HexCellDrawRadius)),
            new(0, -Style.HexCellDrawRadius),
        ];

        public Vector2 Cartesian()
        {
            // @hack: There should be a negative sign on the y coord, but since y+ is down, we don't add that.
            return new((float)(this.Q + (this.R * 0.5)), (float)(Style.ConstSqrtThreeOverTwo * this.R));
        }

        public static readonly HexCoords DirectionRight = new(1, 0);
        public static readonly HexCoords DirectionLeft = new(-1, 0);
        public static readonly HexCoords DirectionRightUp = new(1, -1);
        public static readonly HexCoords DirectionLeftUp = new(0, -1);
        public static readonly HexCoords DirectionLeftDown = new(-1, 1);
        public static readonly HexCoords DirectionRightDown = new(0, 1);
    }
}
