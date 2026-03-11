using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Engine.Gui
{
    public class Button
    {
        public string Text = "";
        public float TextSize = Style.SizeNormal;
        public TextAlign Align = TextAlign.Left;

        public GuiState BelongingState = GuiState.Splashscreen;
        public Rectangle CollisionBox;
        public bool IsPressed = false;
        public bool IsHover = false;

        public enum TextAlign
        {
            Left,
            Center,
            Right
        }
    }
}
