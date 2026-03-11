using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Game
{
    public class GameRender
    {
        public static void SplashScreen()
        {
            var bg = Style.Textures["bg"];
            var widthMultiplier = 1.0 * Style.WindowWidth / bg.Width;
            var heightMultiplier = 1.0 * Style.WindowHeight / bg.Height;
            var finalMultiplier = widthMultiplier > heightMultiplier ? widthMultiplier : heightMultiplier;

            var scaledWidth = (int)(bg.Width * finalMultiplier);
            var scaledHeight = (int)(bg.Height * finalMultiplier);

            Raylib.DrawTexturePro(
                bg,
                new(0, 0, bg.Width, bg.Height),
                new(Style.WindowWidth / 2, Style.WindowHeight / 2, scaledWidth, scaledHeight),
                new(scaledWidth / 2, scaledHeight / 2), 0,
                Color.White);
        }
    }
}
