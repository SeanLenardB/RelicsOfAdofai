using System;
using System.Collections.Generic;
using System.Text;

namespace RelicsOfAdofai.Game
{
    public class Chart
    {
        public string Artist = "";
        public string Song = "";

        public double RequiredSkill = 0.0;

        // @cleanup: this should not be here because we need to hack into
        // this function more when we actually use it.
        // also the logic is not correct
        public bool Attempt(GameContext context)
        {
            if (context.Skill < this.RequiredSkill) return false;

            return context.Random.NextDouble() < (context.Skill - this.RequiredSkill) / this.RequiredSkill;
        }
    }
}
