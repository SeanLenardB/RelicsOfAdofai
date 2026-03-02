using System;
using System.Collections.Generic;
using System.Text;
using RelicsAdofai.Game.Events;

namespace RelicsAdofai.Game
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
        public int Day = 1;
        public double Hour = 8.0;

        public Random Random;
        public List<ChoiceEvent> ChoiceEvents = [];
        public List<InfoEvent> InfoEvents = [];
        public List<Chart> Charts = [];



        public void GenerateEvents()
        {
            this.InfoEvents.Add(new() { Information = "Debug Test 1" });
            this.InfoEvents.Add(new() { Information = "测试事件" });
        }

        public void GenerateCharts()
        {
            // @todo: make this happen better
            for (int i = 0; i < 10; i++)
            {
                this.Charts.Add(new()
                {
                    Artist = "Random Artists",
                    Song = $"Test song suite No. {this.Random.Next(10)}",
                    RequiredSkill = this.Random.NextDouble() * 114.514
                });
            }
        }

        public void FinishDay()
        {
            this.Day++;
            this.Hour = 8.0;
        }

        public GameContext(int seed)
        {
            this.Seed = seed; this.Random = new(seed);
            // @cleanup: remove this, this is just for testing
            this.GenerateCharts();
            this.GenerateEvents();
        }
    }
}
