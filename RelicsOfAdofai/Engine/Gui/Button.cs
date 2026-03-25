using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Game;

namespace RelicsOfAdofai.Engine.Gui
{
    public class Button
    {
        public string Text = "";
        public float TextSize = Style.SizeNormal;
        public TextAlign Align = TextAlign.Left;
        public Predicate Enabled = () => true;

        public GuiState BelongingState = GuiState.Splashscreen;
        public Rectangle CollisionBox;
        public bool IsPressed = false;
        public bool IsHover = false;
        public Action PressAction = () => { };

        public enum TextAlign
        {
            Left,
            Center,
            Right
        }
    }
}
