using Raylib_cs;

namespace RelicsOfAdofai.Game
{
    public class Style
    {
        public static Color ColorTextGeneral = Color.White;
        public static Color ColorTextSkill = Color.Purple;
        public static Color ColorTextMoney = Color.Gold;

        public static Color ColorBgLight = new(96, 144, 224, 239);
        public static Color ColorBgMedium = new(96, 0, 180, 216);
        public static Color ColorBgDark = new(96, 3, 4, 94);

        public static Dictionary<string, Texture2D> Textures = [];
        public static Font Font;

        public static int WindowWidth = 1920;
        public static int WindowHeight = 1080;
    }
}
