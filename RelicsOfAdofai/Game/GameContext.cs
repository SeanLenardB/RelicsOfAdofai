using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Game
{
    public class GameContext
    {
        public static int Seed = 0;
        public static List<Chart> Charts = [];
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static Random Random;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public static void StartGame()
        {
            Random = new(Seed);

            for (int i = 0; i < 9; i++)
            {
                Charts.Add(new() { IconColor = new(Random.Next(255), Random.Next(255), Random.Next(255)) });
            }
        }
    }

    public class Chart
    {
        public string Artist = "";
        public string Song = "";
        public string Creator = "";

        // @todo: Change this to an actual thumbnail or difficulty icon or something
        public Color IconColor = Color.SkyBlue;

        public override string ToString() { return $"{this.Artist} - {this.Song} [{this.Creator}]"; }
    }
}
