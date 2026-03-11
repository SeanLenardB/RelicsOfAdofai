using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Engine.Gui;

namespace RelicsOfAdofai.Engine
{
    public class GameRender
    {
        public static void SplashScreen()
        {
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



            var titleExtent = Raylib.MeasureTextEx(Style.TitleFont, "Relics of Adofai", Style.SizeTitle, 0);
            Raylib.DrawRectangleRounded(
                Layout.CenterTop().Hpx((int)titleExtent.Y * 3).YVh(15).Wvw(60).Wmax(1600).Wmin((int)titleExtent.X).Xvw(50).Rect(),
                0.1f,
                8,
                Style.ColorBgDark);
            Raylib.DrawTextEx(
                Style.TitleFont,
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

            var rngSeedPromptExtent = Raylib.MeasureTextEx(Style.GenericFont, "随机数种子", Style.SizeNormal, 0);
            Raylib.DrawTextEx(
                Style.GenericFont,
                "随机数种子",
                Layout.RightBottom().Hpx(72).YVh(95).DYpx(-360).Wpx((int)rngSeedPromptExtent.X).Xvw(50).DXpx(-24).Vect(),
                Style.SizeNormal,
                0,
                Style.ColorTextGeneral);
        }

        public static void RenderGui()
        {
            foreach (var inputBox in GuiContext.InputBoxes.Values)
            {
                Color underbarColor;
                if (inputBox.IsActive) underbarColor = Style.ColorBorderLight;
                else if (inputBox.IsHover) underbarColor = Style.ColorBorderMedium;
                else underbarColor = Style.ColorBorderDark;

                Raylib.DrawLineEx(
                    new(inputBox.CollisionBox.X, inputBox.CollisionBox.Y + inputBox.CollisionBox.Height),
                    new(inputBox.CollisionBox.X + inputBox.CollisionBox.Width, inputBox.CollisionBox.Y + inputBox.CollisionBox.Height),
                    4.0f,
                    underbarColor);

                var textExtent = Raylib.MeasureTextEx(Style.GenericFont, inputBox.Text, inputBox.TextSize, 0);

                var yPadding = (inputBox.CollisionBox.Height - textExtent.Y) / 2.0f;
                if (yPadding < 0) yPadding = 0;

                var xPadding = 16.0f;  // @note: this should be dependent on text?
                if (inputBox.Align == InputBox.TextAlign.Center) xPadding = (inputBox.CollisionBox.Width - textExtent.X) / 2.0f;
                else if (inputBox.Align == InputBox.TextAlign.Right) xPadding = inputBox.CollisionBox.Width - textExtent.X - xPadding;
                if (xPadding < 0) xPadding = 0;

                Raylib.DrawTextEx(
                    Style.GenericFont,
                    inputBox.Text,
                    new(inputBox.CollisionBox.X + xPadding, inputBox.CollisionBox.Y + yPadding),
                    inputBox.TextSize,
                    0,
                    Style.ColorTextGeneral);
            }
            foreach (var button in GuiContext.Buttons.Values)
            {
                Color underbarColor;
                if (button.IsPressed) underbarColor = Style.ColorBorderDark;
                else if (button.IsHover) underbarColor = Style.ColorBorderLight;
                else underbarColor = Style.ColorBorderMedium;

                Raylib.DrawLineEx(
                    new(button.CollisionBox.X, button.CollisionBox.Y),
                    new(button.CollisionBox.X + button.CollisionBox.Width, button.CollisionBox.Y),
                    4.0f,
                    underbarColor);
                Raylib.DrawLineEx(
                    new(button.CollisionBox.X, button.CollisionBox.Y + button.CollisionBox.Height),
                    new(button.CollisionBox.X + button.CollisionBox.Width, button.CollisionBox.Y + button.CollisionBox.Height),
                    4.0f,
                    underbarColor);

                var textExtent = Raylib.MeasureTextEx(Style.GenericFont, button.Text, button.TextSize, 0);

                var yPadding = (button.CollisionBox.Height - textExtent.Y) / 2.0f;
                if (yPadding < 0) yPadding = 0;

                var xPadding = 16.0f;  // @note: this should be dependent on text?
                if (button.Align == Button.TextAlign.Center) xPadding = (button.CollisionBox.Width - textExtent.X) / 2.0f;
                else if (button.Align == Button.TextAlign.Right) xPadding = button.CollisionBox.Width - textExtent.X - xPadding;
                if (xPadding < 0) xPadding = 0;

                Raylib.DrawTextEx(
                    Style.GenericFont,
                    button.Text,
                    new(button.CollisionBox.X + xPadding, button.CollisionBox.Y + yPadding),
                    button.TextSize,
                    0,
                    Style.ColorTextGeneral);
            }
        }
    }
}
