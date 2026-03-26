using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
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
        public Queue<FloatingMessage> FloatingMessages = [];

        public void GuiInit(GameContext gameContext)
        {
            this.InputBoxes["rngseed"] =
                new() { LegalText = text => int.TryParse(text, out gameContext.Seed) };
            this.Buttons["startgame"] =
                new()
                {
                    Align = Button.TextAlign.Center,
                    Text = "开始",
                    PressAction = () =>
                    {
                        this.SwitchState(GuiState.Game);
                        gameContext.RefreshSeed();
                        gameContext.StartGame();
                    }
                };

            this.Buttons["attempt"] =
                new()
                {
                    BelongingState = GuiState.Game,
                    Align = Button.TextAlign.Center,
                    Text = "尝试",
                    DisabledHint = "需要至少一条通路才能尝试",
                    TextSize = Style.SizeSmall,
                    PressAction = () =>
                    {
                        Debug.Assert(gameContext.CurrentChart is not null, "Cannot attempt a null chart!");
                        Debug.Assert(gameContext.CurrentChart.FinalEnergy > 0, "Cannot spam hours! (is the predicate wrong?)");
                        gameContext.AttemptChart(this, gameContext.CurrentChart);
                    },
                    Enabled = false
                };
        }
        public void RecalculateUIPosition()
        {
            this.InputBoxes["rngseed"].CollisionBox = 
                Layout.LeftCenter().Hpx((int)(Style.SizeNormal * 1.5)).YVh(95).DYpx(-360).Wpx(480).Xvw(50).DXpx(24).Rect();
            this.Buttons["startgame"].CollisionBox =
                Layout.CenterBottom().Hpx(72).YVh(95).DYpx(-80).Wpx(240).Xvw(50).Rect();
            this.Buttons["attempt"].CollisionBox =
                Layout.RightBottom().Hpx((int)(Style.SizeSmall * 1.5)).YVh(100).DYpx(-Style.HandHeight).DYpx(-Style.SizeNormal)
                    .Wpx(Style.SizeSmall * 4).Xvw(100).DXpx(-Style.SizeNormal).Rect();
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

    public class FloatingMessage
    {
        public string Text = "";
        public Vector2 Position;
        public double RemainingTime = 1;

        public Vector2 Velocity = new(0, -2);
    }
}
