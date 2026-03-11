using Raylib_cs;
using RelicsOfAdofai.Game;

public class Program
{
    private static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.SetTargetFPS(144);
        Raylib.InitWindow(Style.WindowWidth, Style.WindowHeight, "Relics of Adofai");
        Raylib.SetWindowMinSize(Style.WindowWidth, Style.WindowHeight);

        Style.Font = Raylib.LoadFontEx("Resources/Anta-Regular.ttf", 128, null, 0);

        var bgImage = Raylib.LoadImage("Resources/bg.png");
        Raylib.ImageBlurGaussian(ref bgImage, 10);
        Style.Textures["bg"] = Raylib.LoadTextureFromImage(bgImage);
        Raylib.UnloadImage(bgImage);

        Context.GuiInit();

        while (!Raylib.WindowShouldClose())
        {
            Context.HandleInput();

            if (Raylib.IsWindowResized())
            {
                Style.WindowWidth = Raylib.GetRenderWidth();
                Style.WindowHeight = Raylib.GetRenderHeight();  // @cleanup: we might listen to an event and update the window size.
            }
            Context.RecalculateUIPosition();

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.RayWhite);

                if (Context.GuiState == GuiState.Splashscreen) GameRender.SplashScreen();

                GameRender.RenderGui();
            }
            Raylib.EndDrawing();
        }

        Raylib.UnloadFont(Style.Font);
        foreach (var texture in Style.Textures.Values) Raylib.UnloadTexture(texture);

        Raylib.CloseWindow();
    }
}