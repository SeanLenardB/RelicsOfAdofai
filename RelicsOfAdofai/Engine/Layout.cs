using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Engine
{
    public class Layout
    {
        public class LayoutData
        {
            public enum AnchorSide
            {
                XLeft     = 0b000001,
                XCenter   = 0b000010,
                XRight    = 0b000100,
                YTop      = 0b001000,
                YCenter   = 0b010000,
                YBottom   = 0b100000,
            }
            public AnchorSide Side = AnchorSide.XLeft | AnchorSide.YTop;
            public double Width = 0; public double Height = 0;
            public double LeftX = 0; public double TopY = 0;
            public Rectangle Rect()
            {
                return new((int)this.LeftX, (int)this.TopY, (int)this.Width, (int)this.Height);
            }
            public Rectangle RectCenter()
            {
                return new((int)(this.LeftX + (this.Width / 2)), (int)(this.TopY + (this.Height / 2)), (int)this.Width, (int)this.Height);
            }
            public Vector2 Vect()
            {
                return new((int)this.LeftX, (int)this.TopY);
            }
            public Vector2 VectCenter()
            {
                return new((int)(this.LeftX + (this.Width / 2)), (int)(this.TopY + (this.Height / 2)));
            }

            public LayoutData Wvw(double percentage)
            {
                this.Width = percentage / 100.0 * Style.WindowWidth;
                return this;
            }
            public LayoutData Hvh(double percentage)
            {
                this.Height = percentage / 100.0 * Style.WindowHeight;
                return this;
            }
            public LayoutData Wpx(double px)
            {
                this.Width = px;
                return this;
            }
            public LayoutData Hpx(double px)
            {
                this.Height = px;
                return this;
            }
            public LayoutData Wmin(double px)
            {
                Debug.Assert(this.Width > 0, "You need to first set vw or px, and then call min-max methods!");
                if (this.Width < px) this.Width = px;
                return this;
            }
            public LayoutData Wmax(double px)
            {
                Debug.Assert(this.Width > 0, "You need to first set vw or px, and then call min-max methods!");
                if (this.Width > px) this.Width = px;
                return this;
            }
            public LayoutData Hmin(double px)
            {
                Debug.Assert(this.Height > 0, "You need to first set vh or px, and then call min-max methods!");
                if (this.Height < px) this.Height = px;
                return this;
            }
            public LayoutData Hmax(double px)
            {
                Debug.Assert(this.Height > 0, "You need to first set vh or px, and then call min-max methods!");
                if (this.Height > px) this.Height = px;
                return this;
            }

            public LayoutData Xpx(double px)
            {
                Debug.Assert(this.Width > 0, "You need to first set w, and then set x!");

                if ((this.Side & AnchorSide.XLeft) != 0) this.LeftX = px;
                else if ((this.Side & AnchorSide.XCenter) != 0) this.LeftX = px - (this.Width / 2.0);
                else this.LeftX = px - this.Width;

                return this;
            }
            public LayoutData Ypx(double px)
            {
                Debug.Assert(this.Height > 0, "You need to first set h, and then set y!");

                if ((this.Side & AnchorSide.YTop) != 0) this.TopY = px;
                else if ((this.Side & AnchorSide.YCenter) != 0) this.TopY = px - (this.Height / 2.0);
                else this.TopY = px - this.Height;

                return this;
            }
            public LayoutData Xvw(double percentage) => this.Xpx((double)(percentage / 100.0 * Style.WindowWidth));
            public LayoutData YVh(double percentage) => this.Ypx((double)(percentage / 100.0 * Style.WindowHeight));

            public LayoutData DXpx(double px)
            {
                Debug.Assert(this.LeftX > 0, "This is an offset function, you need to first set x!");

                this.LeftX += px;
                return this;
            }
            public LayoutData DYpx(double px)
            {
                Debug.Assert(this.TopY > 0, "This is an offset function, you need to first set y!");

                this.TopY += px;
                return this;
            }
        }

        public static LayoutData LeftTop() => new() { Side = LayoutData.AnchorSide.XLeft | LayoutData.AnchorSide.YTop };
        public static LayoutData LeftCenter() => new() { Side = LayoutData.AnchorSide.XLeft | LayoutData.AnchorSide.YCenter };
        public static LayoutData LeftBottom() => new() { Side = LayoutData.AnchorSide.XLeft | LayoutData.AnchorSide.YBottom };
        public static LayoutData CenterTop() => new() { Side = LayoutData.AnchorSide.XCenter | LayoutData.AnchorSide.YTop };
        public static LayoutData CenterCenter() => new() { Side = LayoutData.AnchorSide.XCenter | LayoutData.AnchorSide.YCenter };
        public static LayoutData CenterBottom() => new() { Side = LayoutData.AnchorSide.XCenter | LayoutData.AnchorSide.YBottom };
        public static LayoutData RightTop() => new() { Side = LayoutData.AnchorSide.XRight | LayoutData.AnchorSide.YTop };
        public static LayoutData RightCenter() => new() { Side = LayoutData.AnchorSide.XRight | LayoutData.AnchorSide.YCenter };
        public static LayoutData RightBottom() => new() { Side = LayoutData.AnchorSide.XRight | LayoutData.AnchorSide.YBottom };
    }
}
