using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Game
{
    public class GameContext
    {
        public int Seed = 0;
        public List<Chart> Charts = [];
        public Random Random = new();

        public void RefreshSeed() => this.Random = new(this.Seed);

        public void StartGame()
        {
            this.Random = new(this.Seed);

            for (int i = 0; i < 9; i++)
            {
                this.Charts.Add(new() { IconColor = new(this.Random.Next(255), this.Random.Next(255), this.Random.Next(255)) });
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
