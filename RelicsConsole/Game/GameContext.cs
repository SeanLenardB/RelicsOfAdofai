using System;
using System.Collections.Generic;
using System.Text;

namespace RelicsConsole.Game
{
    public class GameContext
    {
        public int Version = 1;

        public int Seed = 0;
        public int DailyEventCount = 0;
        public int DailyEnergyRecharge = 5;

        public string PlayerName = "";
        public int FollowerCount = 0;
        public double Skill = 0.0;
        public int Energy = 0;

        public Random Random;
        public List<ChoiceEvent> ChoiceEvents = [];



        public void GenerateChoiceEvents()
        {
            
        }

        public GameContext(int seed)
        {
            this.Seed = seed; this.Random = new(seed);
        }
        public GameContext()
        {
            this.Seed = new Random().Next();
            this.Random = new(this.Seed);
        }
    }
}
