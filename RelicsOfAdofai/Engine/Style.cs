using Raylib_cs;

namespace RelicsOfAdofai.Engine
{
    public class Style
    {
        public static Color ColorTextGeneral = Color.White;
        public static Color ColorTextSkill = Color.Purple;
        public static Color ColorTextMoney = Color.Gold;

        public static Color ColorBgLight = new(0, 88, 240, 48);
        public static Color ColorBgMedium = new(0, 34, 132, 96);
        public static Color ColorBgDark = new(0, 0, 0, 144);
        public static Color ColorBgInputGradientInactive = new(255, 255, 255, 64);
        public static Color ColorBgInputGradientActive = new(255, 255, 255, 128);

        public static Color ColorBorderDark = Color.Gray;
        public static Color ColorBorderMedium = Color.LightGray;
        public static Color ColorBorderLight = Color.White;

        public static Dictionary<string, Texture2D> Textures = [];
        public static Font FontTitle;
        public static Font FontNormal;

        public static int SizeNormal = 32;
        public static int SizeTitle = 72;
        public static int SizeHeaderTitle = 48;

        public static int WindowWidth = 1920;
        public static int WindowHeight = 1080;
    }
}
