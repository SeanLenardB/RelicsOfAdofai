using Raylib_cs;

namespace RelicsOfAdofai.Game.Gui
{
    public class GuiInputBox()
    {
        public string Text = "";
        public float TextSize = 32;

        public GuiState BelongingState = GuiState.Splashscreen;
        public Rectangle CollisionBox;
        public bool IsActive = false;
        public bool IsHover = false;
        public Predicate<string> LegalText = _ => true;

        public float TimeAfterPreviousDelete = 0.05f;
    }
}
