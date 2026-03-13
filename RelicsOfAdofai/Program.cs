using Raylib_cs;
using RelicsOfAdofai.Engine;

public class Program
{
    private static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.SetTargetFPS(144);
        Raylib.InitWindow(Style.WindowWidth, Style.WindowHeight, "Relics of Adofai");
        Raylib.SetWindowMinSize(Style.WindowWidth, Style.WindowHeight);

        Style.FontTitle = Raylib.LoadFontEx("Resources/Quantico-Bold.ttf", Style.SizeTitle, null, 0);
        Style.FontNormal = 
            Raylib.LoadFontFromMemory(".ttf", File.ReadAllBytes("Resources/NotoSansSC-Medium.ttf"), 
                Style.SizeNormal, [.. Enumerable.Range(0x4e00, 0x9fff - 0x4e00), .. Enumerable.Range(0, 256)], 0x9fff - 0x4e00 + 256);

        var bgImage = Raylib.LoadImage("Resources/bg.png");
        Raylib.ImageBlurGaussian(ref bgImage, 10);
        Style.Textures["bg"] = Raylib.LoadTextureFromImage(bgImage);
        Raylib.UnloadImage(bgImage);

        GuiContext.GuiInit();

        while (!Raylib.WindowShouldClose())
        {
            Interactivity.HandleInput();

            if (Raylib.IsWindowResized())
            {
                Style.WindowWidth = Raylib.GetRenderWidth();
                Style.WindowHeight = Raylib.GetRenderHeight();  // @cleanup: we might listen to an event and update the window size.
            }
            GuiContext.RecalculateUIPosition();

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.RayWhite);

                switch (GuiContext.GuiState)
                {
                    case GuiState.Splashscreen: GameRender.SplashScreen(); break;
                    case GuiState.Game: GameRender.Game(); break;
                    default: goto case GuiState.Splashscreen;
                }

                GameRender.RenderGui();
            }
            Raylib.EndDrawing();
        }

        Raylib.UnloadFont(Style.FontTitle);
        Raylib.UnloadFont(Style.FontNormal);
        foreach (var texture in Style.Textures.Values) Raylib.UnloadTexture(texture);

        Raylib.CloseWindow();
    }
}