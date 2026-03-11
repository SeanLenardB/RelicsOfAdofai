using Raylib_cs;
using RelicsOfAdofai.Game;

public class Program
{
    private static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.SetTargetFPS(144);
        Raylib.InitWindow(Style.WindowWidth, Style.WindowHeight, "Relics of Adofai");

        Style.Font = Raylib.LoadFontEx("Resources/Anta-Regular.ttf", 64, null, 0);
        Style.Textures["bg"] = Raylib.LoadTexture("Resources/bg.png");

        while (!Raylib.WindowShouldClose())
        {
            Style.WindowWidth = Raylib.GetRenderWidth();
            Style.WindowHeight = Raylib.GetRenderHeight();  // @cleanup: we might listen to an event and update the window size.

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);
            if (Context.GuiState == GuiState.Splashscreen) GameRender.SplashScreen();
            Raylib.EndDrawing();
        }

        Raylib.UnloadFont(Style.Font);
        foreach (var texture in Style.Textures.Values) Raylib.UnloadTexture(texture);

        Raylib.CloseWindow();
    }
}