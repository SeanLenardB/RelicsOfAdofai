using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Engine.Gui;
using RelicsOfAdofai.Game;

namespace RelicsOfAdofai.Engine
{
    public class GuiContext
    {
        public static GameContext GameContext = new();
        public static GuiState GuiState = GuiState.Splashscreen;
        public static Dictionary<string, InputBox> InputBoxes = [];
        public static Dictionary<string, Button> Buttons = [];

        public static void GuiInit()
        {
            InputBoxes["rngseed"] = new() { LegalText = text => int.TryParse(text, out var _) };
            Buttons["startgame"] = new() { Align = Button.TextAlign.Center, Text = "开始" };
        }
        public static void RecalculateUIPosition()
        {
            InputBoxes["rngseed"].CollisionBox = 
                Layout.LeftBottom().Hpx(72).YVh(95).DYpx(-360).Wpx(480).Xvw(50).DXpx(24).Rect();
            Buttons["startgame"].CollisionBox =
                Layout.CenterBottom().Hpx(72).YVh(95).DYpx(-80).Wpx(240).Xvw(50).Rect();
        }
    }

    public enum GuiState
    {
        Splashscreen
    }
}
