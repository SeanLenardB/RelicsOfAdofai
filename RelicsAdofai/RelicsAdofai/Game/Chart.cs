using System;
using System.Collections.Generic;
using System.Text;

namespace RelicsAdofai.Game
{
    public class Chart
    {
        public string Artist = "";
        public string Song = "";
        public string Creator = "";

        public double RequiredSkill = 0.0;

        public override string ToString() { return $"{Artist} - {Song} [{Creator}]"; }
    }
}
