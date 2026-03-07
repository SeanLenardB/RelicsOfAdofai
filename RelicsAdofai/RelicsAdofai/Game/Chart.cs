using System;
using System.Collections.Generic;
using System.Text;

namespace RelicsAdofai.Game
{
    public class Chart
    {
        public int Id = 0;
        public string Artist = "";
        public string Song = "";
        public string Creator = "";

        public double RequiredSkill = 0.0;

        public override string ToString() { return $"#{this.Id}. {this.Artist} - {this.Song} [{this.Creator}]"; }
    }
}
