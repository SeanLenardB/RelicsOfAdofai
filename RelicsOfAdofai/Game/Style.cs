using Raylib_cs;

namespace RelicsOfAdofai.Game
{
    public class Style
    {
        public static Color ColorTextGeneral = Color.White;
        public static Color ColorTextSkill = Color.Purple;
        public static Color ColorTextMoney = Color.Gold;

        public static Color ColorBgLight = new(144, 224, 239, 96);
        public static Color ColorBgMedium = new(0, 180, 216, 96);
        public static Color ColorBgDark = new(3, 4, 94, 96);

        public static Color ColorBorderDark = Color.Gray;
        public static Color ColorBorderMedium = Color.LightGray;
        public static Color ColorBorderLight = Color.White;

        public static Dictionary<string, Texture2D> Textures = [];
        public static Font Font;

        public static int WindowWidth = 1920;
        public static int WindowHeight = 1080;
    }
}
