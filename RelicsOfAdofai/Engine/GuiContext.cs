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
        public GuiState GuiState = GuiState.Splashscreen;
        public Dictionary<string, InputBox> InputBoxes = [];
        public Dictionary<string, Button> Buttons = [];

        public void GuiInit(GameContext gameContext)
        {
            this.InputBoxes["rngseed"] = 
                new() { LegalText = text => int.TryParse(text, out gameContext.Seed) };
            this.Buttons["startgame"] = 
                new() { Align = Button.TextAlign.Center, Text = "开始", PressAction = () =>
                {
                    this.SwitchState(GuiState.Game);
                    gameContext.RefreshSeed();
                    gameContext.StartGame();
                }};
        }
        public void RecalculateUIPosition()
        {
            this.InputBoxes["rngseed"].CollisionBox = 
                Layout.LeftCenter().Hpx((int)(Style.SizeNormal * 1.5)).YVh(95).DYpx(-360).Wpx(480).Xvw(50).DXpx(24).Rect();
            this.Buttons["startgame"].CollisionBox =
                Layout.CenterBottom().Hpx(72).YVh(95).DYpx(-80).Wpx(240).Xvw(50).Rect();
        }
        public void SwitchState(GuiState newState)
        {
            // @todo: impl animation?
            Debug.Assert(newState != this.GuiState, "Cannot change from and to the same state!");
            this.GuiState = newState;
        }
    }

    public enum GuiState
    {
        Splashscreen,
        Game,
    }
}
