using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Engine.Gui;
using RelicsOfAdofai.Game;

namespace RelicsOfAdofai.Engine
{
    public class GameRender
    {
        /* ----------- SPLASH ----------- */
        public void SplashScreen()
        {
            var bg = Style.Textures["bg"];

            var widthMultiplier = 1.0 * Style.WindowWidth / bg.Width;
            var heightMultiplier = 1.0 * Style.WindowHeight / bg.Height;
            var finalMultiplier = widthMultiplier > heightMultiplier ? widthMultiplier : heightMultiplier;

            finalMultiplier *= 1.15;  // Leaving room for the cursor effect
            var cursorPosition = Raylib.GetMousePosition();
            var xOffsetProportion = -(cursorPosition.X - (Style.WindowWidth / 2.0)) / Style.WindowWidth / 2.0;
            var yOffsetProportion = (cursorPosition.Y - (Style.WindowHeight / 2.0)) / Style.WindowHeight / 2.0;

            var scaledWidth = (int)(bg.Width * finalMultiplier);
            var scaledHeight = (int)(bg.Height * finalMultiplier);

            Raylib.DrawTexturePro(
                bg,
                new(0, 0, bg.Width, bg.Height),
                new(
                    (Style.WindowWidth / 2) + (int)(xOffsetProportion * bg.Width * 0.1),
                    (Style.WindowHeight / 2) + (int)(yOffsetProportion * bg.Height * 0.1),
                    scaledWidth, scaledHeight),
                new(scaledWidth / 2, scaledHeight / 2), 0,
                Color.Gray);



            var titleExtent = Raylib.MeasureTextEx(Style.FontTitle, "Relics of Adofai", Style.SizeTitle, 0);
            Raylib.DrawRectangleRounded(
                Layout.CenterTop().Hpx((int)titleExtent.Y * 3).YVh(15).Wvw(60).Wmax(1600).Wmin((int)titleExtent.X).Xvw(50).Rect(),
                0.1f,
                8,
                Style.ColorBgDark);
            Raylib.DrawTextEx(
                Style.FontTitle,
                "Relics of Adofai",
                Layout.CenterTop().Hpx((int)titleExtent.Y).YVh(15).Wpx((int)titleExtent.X).Xvw(50).DYpx((int)titleExtent.Y).Vect(),
                Style.SizeTitle,
                0,
                Style.ColorTextGeneral);

            Raylib.DrawRectangleRounded(
                Layout.CenterBottom().Hvh(45).YVh(95).Wvw(60).Wmax(1600).Wmin(720).Xvw(50).Rect(),
                0.1f,
                8,
                Style.ColorBgMedium);

            var rngExtent = Raylib.MeasureTextEx(Style.FontNormal, "随机数种子", Style.SizeNormal, 0);
            Raylib.DrawTextEx(
                Style.FontNormal,
                "随机数种子",
                Layout.RightCenter().Hpx((int)rngExtent.Y).YVh(95).DYpx(-360).Wpx((int)rngExtent.X).Xvw(50).DXpx(-24).Vect(),
                Style.SizeNormal,
                0,
                Style.ColorTextGeneral);
        }


        
        /* ----------- GAME ----------- */
        public void Game(GameContext gameContext)
        {
            /* ----- BACKGROUND ----- */
            // @cleanup: copypasta from SplashScreen()
            Debug.Assert(Style.Textures.ContainsKey("bg"), "Cannot find the background image bg!");
            var bg = Style.Textures["bg"];

            var widthMultiplier = 1.0 * Style.WindowWidth / bg.Width;
            var heightMultiplier = 1.0 * Style.WindowHeight / bg.Height;
            var finalMultiplier = widthMultiplier > heightMultiplier ? widthMultiplier : heightMultiplier;

            finalMultiplier *= 1.15;  // Leaving room for the cursor effect
            var cursorPosition = Raylib.GetMousePosition();
            var xOffsetProportion = -(cursorPosition.X - (Style.WindowWidth / 2.0)) / Style.WindowWidth / 2.0;
            var yOffsetProportion = (cursorPosition.Y - (Style.WindowHeight / 2.0)) / Style.WindowHeight / 2.0;

            var scaledWidth = (int)(bg.Width * finalMultiplier);
            var scaledHeight = (int)(bg.Height * finalMultiplier);

            Raylib.DrawTexturePro(
                bg,
                new(0, 0, bg.Width, bg.Height),
                new(
                    (Style.WindowWidth / 2) + (int)(xOffsetProportion * bg.Width * 0.1),
                    (Style.WindowHeight / 2) + (int)(yOffsetProportion * bg.Height * 0.1),
                    scaledWidth, scaledHeight),
                new(scaledWidth / 2, scaledHeight / 2), 0,
                Color.Gray);



            /* ----- GRID ----- */
            this.DrawChartGrid(gameContext);



            /* ----- HEADER ----- */
            var headerRect = Layout.CenterTop().Hpx(Style.HeaderHeight).Ypx(0).Wvw(100).Xvw(50).Rect();
            var headerRectLeftHalf = headerRect;
            headerRectLeftHalf.Width /= 2;
            var headerRectRightHalf = headerRectLeftHalf;
            headerRectRightHalf.X += headerRectRightHalf.Width;
            Raylib.DrawRectangle(
                (int)headerRect.X, (int)headerRect.Y, 
                (int)headerRect.Width, (int)headerRect.Height,
                Style.ColorBgLight);
            Raylib.DrawRectangleGradientEx(
                headerRectLeftHalf,
                Color.Blank, Color.Blank, Style.ColorBgInputGradientInactive, Color.Blank);
            Raylib.DrawRectangleGradientEx(
                headerRectRightHalf,
                Color.Blank, Style.ColorBgInputGradientInactive, Color.Blank, Color.Blank);
            Raylib.DrawRectangleGradientH(
                0, (int)headerRect.Height,
                (int)(headerRect.Width / 2), 6, Style.ColorBorderBlack, Style.ColorBorderLight);
            Raylib.DrawRectangleGradientH(
                (int)(headerRect.Width / 2), (int)headerRect.Height,
                (int)(headerRect.Width / 2), 6, Style.ColorBorderLight, Style.ColorBorderBlack);

            var titleExtent = Raylib.MeasureTextEx(Style.FontTitle, "Relics of Adofai", Style.SizeHeaderTitle, 0);
            var padding = (128 - titleExtent.Y) / 2;
            Raylib.DrawTextEx(
                Style.FontTitle,
                "Relics of Adofai",
                new(padding, padding),
                Style.SizeHeaderTitle,
                0,
                Style.ColorTextGeneral);
            /*
             * [  ]----[  ]----[  ]
             *    < 96 >
             * <40>
             */
            var chartListLineLength = 136 * (gameContext.Charts.Count - 1);
            Raylib.DrawLineEx(
                new((Style.WindowWidth - chartListLineLength) / 2, 64),
                new((Style.WindowWidth + chartListLineLength) / 2, 64),
                4.0f,
                Style.ColorBorderLight);
            var firstIconCenterX = (Style.WindowWidth - chartListLineLength ) / 2;
            for (int i = 0; i < gameContext.Charts.Count; i++)
            {
                var iconRect = Layout.CenterCenter().Hpx(40).Ypx(64).Wpx(40).Xpx(firstIconCenterX + (i * 136)).Rect();
                Raylib.DrawRectangle(
                    (int)iconRect.X, (int)iconRect.Y, (int)iconRect.Width, (int)iconRect.Height,
                    gameContext.Charts[i].IconColor);
                Raylib.DrawRectangleRoundedLinesEx(
                    iconRect,
                    0.1f,
                    8,
                    4f,
                    Style.ColorBorderLight);
            }



            /* ----- HAND ----- */
            var handRect = Layout.CenterBottom().Hpx(Style.HandHeight).YVh(100).Wvw(100).Xvw(50).Rect();
            Raylib.DrawRectangleGradientV(
                (int)handRect.X, (int)handRect.Y, (int)handRect.Width, (int)handRect.Height,
                Style.ColorBgInputGradientActive, Style.ColorBgInputGradientInactive);
            Raylib.DrawLineEx(
                new(handRect.X, handRect.Y),
                new(handRect.X + handRect.Width, handRect.Y),
                6,
                Style.ColorBorderLight);

            this.DrawHand(gameContext);
        }

        public void DrawChartGrid(GameContext gameContext)
        {
            foreach (var cell in gameContext.CurrentChart.Cells)
            {
                var cellCenter = (cell.Coords.Cartesian() * Style.HexCellSpaceRadius) + gameContext.CurrentChart.HexOrigin;
                if (cell.IsHover)
                    Raylib.DrawPoly(
                        cellCenter,
                        6,
                        Style.HexCellDrawRadius,
                        30,
                        Style.ColorBgMedium);
                else 
                    Raylib.DrawPoly(
                        cellCenter,
                        6,
                        Style.HexCellDrawRadius,
                        30,
                        Style.ColorBgDark);

                Raylib.DrawPolyLinesEx(
                    cellCenter,
                    6,
                    Style.HexCellDrawRadius,
                    30,
                    6,
                    Style.ColorBorderLight);

                var imageLocation = cellCenter;
                imageLocation.X -= 128; imageLocation.Y -= 128;  // The image is 256x256.
                if (cell.Type == ChartCell.CellType.Start)
                    Raylib.DrawTextureEx(
                        Style.Textures["nodeStart"],
                        imageLocation,
                        0,
                        1,
                        new(255, 255, 255, 128));
                else if (cell.Type == ChartCell.CellType.End)
                    Raylib.DrawTextureEx(
                        Style.Textures["nodeEnd"],
                        imageLocation,
                        0,
                        1,
                        new(255, 255, 255, 128));
            }
        }

        public void DrawHand(GameContext gameContext)
        {
            // @note:
            // The Layout **should** specify the center of the node as the variable name suggests,
            // but we are doing Layout.LeftTop() here because the Layout construction will give the left-top corner.
            // For perf we don't want to have a lot of calculation involved.
            //
            // Therefore we specify LeftTop() to let the return value be the center of the hexagon.
            var currentNodeCenter =
                Layout.LeftTop().Hpx(Style.NodeInHandRadius * 2).YVh(100).DYpx(-Style.HandHeight / 2).Wpx(Style.NodeInHandRadius * 2).Xpx(Style.HandHeight / 2).Vect();
            foreach (var node in gameContext.HandNodes)
            {
                Raylib.DrawPoly(
                    currentNodeCenter,
                    6,
                    Style.NodeInHandRadius,
                    30,
                    node.Color);  // @note: will get changed. Look at Node definition.

                Raylib.DrawPolyLinesEx(
                    currentNodeCenter,
                    6,
                    Style.NodeInHandRadius,
                    30,
                    6,
                    Style.ColorBorderLight);

                currentNodeCenter.X += Style.NodeInHandRadius * 2;
            }
        }



        /* ----------- GENERIC GUI ----------- */

        public void RenderGui(GuiContext guiContext)
        {
            foreach (var inputBox in guiContext.InputBoxes.Values)
            {
                if (inputBox.BelongingState != guiContext.GuiState) continue;

                Color underbarColor;
                if (inputBox.IsActive) underbarColor = Style.ColorBorderLight;
                else if (inputBox.IsHover) underbarColor = Style.ColorBorderMedium;
                else underbarColor = Style.ColorBorderDark;

                if (inputBox.IsActive)
                    Raylib.DrawRectangleGradientEx(
                        inputBox.CollisionBox,
                        Color.Blank, Style.ColorBgInputGradientActive, Style.ColorBgInputGradientActive, Color.Blank);
                else
                    Raylib.DrawRectangleGradientEx(
                        inputBox.CollisionBox,
                        Color.Blank, Style.ColorBgInputGradientInactive, Style.ColorBgInputGradientInactive, Color.Blank);

                Raylib.DrawLineEx(
                    new(inputBox.CollisionBox.X, inputBox.CollisionBox.Y + inputBox.CollisionBox.Height),
                    new(inputBox.CollisionBox.X + inputBox.CollisionBox.Width, inputBox.CollisionBox.Y + inputBox.CollisionBox.Height),
                    4.0f,
                    underbarColor);

                var textExtent = Raylib.MeasureTextEx(Style.FontNormal, inputBox.Text, inputBox.TextSize, 0);

                var yPadding = (inputBox.CollisionBox.Height - textExtent.Y) / 2.0f;
                if (yPadding < 0) yPadding = 0;

                var xPadding = 16.0f;  // @note: this should be dependent on text?
                if (inputBox.Align == InputBox.TextAlign.Center) xPadding = (inputBox.CollisionBox.Width - textExtent.X) / 2.0f;
                else if (inputBox.Align == InputBox.TextAlign.Right) xPadding = inputBox.CollisionBox.Width - textExtent.X - xPadding;
                if (xPadding < 0) xPadding = 0;

                Raylib.DrawTextEx(
                    Style.FontNormal,
                    inputBox.Text,
                    new(inputBox.CollisionBox.X + xPadding, inputBox.CollisionBox.Y + yPadding),
                    inputBox.TextSize,
                    0,
                    Style.ColorTextGeneral);
            }
            foreach (var button in guiContext.Buttons.Values)
            {
                if (button.BelongingState != guiContext.GuiState) continue;

                Color outlineColor;
                if (button.IsPressed) outlineColor = Style.ColorBorderDark;
                else if (button.IsHover) outlineColor = Style.ColorBorderLight;
                else outlineColor = Style.ColorBorderMedium;

                Raylib.DrawRectangleRoundedLinesEx(
                    button.CollisionBox,
                    0.1f,
                    8,
                    4.0f,
                    outlineColor);

                var textExtent = Raylib.MeasureTextEx(Style.FontNormal, button.Text, button.TextSize, 0);

                var yPadding = (button.CollisionBox.Height - textExtent.Y) / 2.0f;
                if (yPadding < 0) yPadding = 0;

                var xPadding = 16.0f;  // @note: this should be dependent on text?
                if (button.Align == Button.TextAlign.Center) xPadding = (button.CollisionBox.Width - textExtent.X) / 2.0f;
                else if (button.Align == Button.TextAlign.Right) xPadding = button.CollisionBox.Width - textExtent.X - xPadding;
                if (xPadding < 0) xPadding = 0;

                Raylib.DrawTextEx(
                    Style.FontNormal,
                    button.Text,
                    new(button.CollisionBox.X + xPadding, button.CollisionBox.Y + yPadding),
                    button.TextSize,
                    0,
                    Style.ColorTextGeneral);
            }
        }
    }
}
