using Raylib_cs;
using RelicsOfAdofai.Engine;

namespace RelicsOfAdofai.Engine.Gui
{
    public class InputBox
    {
        public string Text = "";
        public float TextSize = Style.SizeNormal;
        public TextAlign Align = TextAlign.Left;

        public GuiState BelongingState = GuiState.Splashscreen;
        public Rectangle CollisionBox;
        public bool IsActive = false;
        public bool IsHover = false;
        public Predicate<string> LegalText = _ => true;

        public enum TextAlign
        {
            Left,
            Center,
            Right
        }
    }
}
