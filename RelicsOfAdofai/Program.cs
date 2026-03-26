using System.Diagnostics;
using Raylib_cs;
using RelicsOfAdofai.Engine;
using RelicsOfAdofai.Game;

public class Program
{
    private static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        //Raylib.SetTargetFPS(144);  // @nocheckin
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

        Style.Textures["nodeStart"] = Raylib.LoadTexture("Resources/nodeStart.png");
        Style.Textures["nodeEnd"] = Raylib.LoadTexture("Resources/nodeEnd.png");

        Style.Textures["node-connector-opposite"] = Raylib.LoadTexture("Resources/connector-opposite.png");
        Style.Textures["node-connector-interval"] = Raylib.LoadTexture("Resources/connector-interval.png");
        Style.Textures["node-connector-adjacent"] = Raylib.LoadTexture("Resources/connector-adjacent.png");
        Style.Textures["node-extractor-single"] = Raylib.LoadTexture("Resources/extractor-single.png");
        Style.Textures["node-receiver-neighbor"] = Raylib.LoadTexture("Resources/receiver-neighbor.png");



        GameRender gameRender = new();
        Interactivity interactivity = new();
        GameContext gameContext = new();
        GuiContext guiContext = new();
        guiContext.GuiInit(gameContext);

        Stopwatch stopwatch = new();

        // @note: remove this when publishing?
        if (gameContext.DebugMode && guiContext.GuiState == GuiState.Splashscreen) guiContext.Buttons["startgame"].PressAction();

        while (!Raylib.WindowShouldClose())
        {
#if DEBUG
                stopwatch.Start();
#endif
            Debug.Assert(!Raylib.IsKeyPressed(KeyboardKey.B), "Debug Breakpoint");
            interactivity.HandleInput(guiContext, gameContext);
#if DEBUG
                var interactivityTime = stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset(); stopwatch.Start();
#endif

            if (Raylib.IsWindowResized())
            {
                Style.WindowWidth = Raylib.GetRenderWidth();
                Style.WindowHeight = Raylib.GetRenderHeight();
            }
            guiContext.RecalculateUIPosition();
#if DEBUG
                var layoutTime = stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset(); stopwatch.Start();
#endif

            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Color.RayWhite);

                switch (guiContext.GuiState)
                {
                    case GuiState.Splashscreen: gameRender.SplashScreen(); break;
                    case GuiState.Game: gameRender.Game(gameContext, interactivity); break;
                    default: goto case GuiState.Splashscreen;
                }
#if DEBUG
                var stateTime = stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset(); stopwatch.Start();
#endif
                gameRender.RenderGui(gameContext, guiContext, interactivity);
#if DEBUG
                var guiTime = stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset();

                Raylib.DrawTextEx(Style.FontTitle, 
                    $"Interactivity {interactivityTime}mspt\tLayout {layoutTime}mspt\tState {stateTime}mspt\tGui {guiTime}mspt",
                    new(4, 4), Style.SizeSmall, 0, Style.ColorTextGeneral);
#endif
            }
            Raylib.EndDrawing();

            gameContext.DeltaTime = Raylib.GetFrameTime();
        }

        Raylib.UnloadFont(Style.FontTitle);
        Raylib.UnloadFont(Style.FontNormal);
        foreach (var texture in Style.Textures.Values) Raylib.UnloadTexture(texture);

        Raylib.CloseWindow();
    }
}