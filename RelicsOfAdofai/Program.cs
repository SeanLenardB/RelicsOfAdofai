using Raylib_cs;

internal class Program
{
    private static void Main()
    {
        Raylib.InitWindow(1920, 1080, "Relics of Adofai");

        var font = Raylib.LoadFontEx("Resources/Anta-Regular.ttf", 64, null, 0);

        Raylib.SetTargetFPS(144);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);
            Raylib.DrawTextEx(font, "Congrats! You created your first window!", new(190, 200), font.BaseSize, 0, Color.Gray);
            Raylib.EndDrawing();
        }

        Raylib.UnloadFont(font);

        Raylib.CloseWindow();
    }
}