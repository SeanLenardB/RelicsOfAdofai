using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
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
                        new(0, 0, ChartCell.CellType.Source),
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

        public double FinalEnergy = 0.0;
        public double OptimalEnergy = 1.0;  // @todo: determine a good way to generate this value to make game fun

        public List<ChartCell> Cells = [];
        public Vector2 HexOrigin = Layout.CenterCenter().Hpx(1).Wpx(1).YVh(50).Xvw(50).Vect();
        public void AdjustGridToCenter()
        {
            HexCoords centerOfMass = this.Cells.Aggregate(new HexCoords(), (coords, cell) => coords + cell.Coords) / this.Cells.Count;
            this.HexOrigin -= centerOfMass.Cartesian() * Style.HexCellSpaceRadius;
        }
    }

    public class ChartCell(double q, double r, ChartCell.CellType type = ChartCell.CellType.Normal)
    {
        public HexCoords Coords = new(q, r);
        public CellType Type = type;
        public SkillNode? FilledNode = null;
        public enum CellType
        {
            Normal,
            Source,
            End,
        }

        public double FluxIn = 0;
        public double FluxOut = 0;

        public double SourceEnergy = 1;

        public bool IsHover = false;
    }
    public struct HexCoords(double q, double r)
    {
        /*
         *    0 ---- Q+
         *     \
         *      \
         *       R+
         *
         *   Rotation +direction: CCW
         */
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

        public static HexCoords FromCartesian(Vector2 cart)
        {
            return new(cart.X - (cart.Y * Style.ConstOneOverSqrtThree), cart.Y * Style.ConstTwoOverSqrtThree);
        }

        public static readonly HexCoords DirectionRight = new(1, 0);
        public static readonly HexCoords DirectionLeft = new(-1, 0);
        public static readonly HexCoords DirectionRightUp = new(1, -1);
        public static readonly HexCoords DirectionLeftUp = new(0, -1);
        public static readonly HexCoords DirectionLeftDown = new(-1, 1);
        public static readonly HexCoords DirectionRightDown = new(0, 1);
        public static readonly HexCoords[] Directions = 
            [DirectionRight, DirectionRightUp, DirectionLeftUp, DirectionLeft, DirectionLeftDown, DirectionRightDown];
        public static HexCoords RotationUnit(int rotation)
        {
            Debug.Assert(rotation % 60 == 0, "Rotation should be multiples of 60!");
            rotation %= 360; if (rotation < 0) rotation += 360;

            return rotation switch
            {
                0 => DirectionRight,
                60 => DirectionRightUp,
                120 => DirectionLeftUp,
                180 => DirectionLeft,
                240 => DirectionLeftDown,
                300 => DirectionRightDown,
                _ => DirectionRight,
            };
        }
        public static HexCoords operator +(HexCoords left, HexCoords right) { return new(left.Q + right.Q, left.R + right.R); }
        public static HexCoords operator -(HexCoords left, HexCoords right) { return new(left.Q - right.Q, left.R - right.R); }
        public static HexCoords operator *(HexCoords left, double right) { return new(left.Q * right, left.R * right); }
        public static HexCoords operator /(HexCoords left, double right) { return new(left.Q / right, left.R / right); }

        public bool CoordsEqual(HexCoords hex) { return this.Q == hex.Q && this.R == hex.R; }
        public override string ToString() => $"{{HexCoords <Q={this.Q}, R={this.R}>}}";
    }
}
