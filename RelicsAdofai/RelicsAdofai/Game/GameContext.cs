using System;
using System.Collections.Generic;
using System.Text;

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
        public double Hour = 8.5;

        public Random Random;
        public List<ChoiceEvent> ChoiceEvents = [];
        public List<Chart> Charts = [];



        public void GenerateChoiceEvents()
        {
            
        }

        public void GenerateCharts()
        {
            // @todo: make this happen better
            this.Charts.Add(new()
            {
                Artist = "QR2 Dev Team",
                Song = "QR2",
                RequiredSkill = 114
            });
            this.Charts.Add(new()
            {
                Artist = "QR2 Dev Team",
                Song = "QR2 [Nerfed]",
                RequiredSkill = 14
            });
            this.Charts.Add(new()
            {
                Artist = "Sean Lenard B.",
                Song = "The Final Descent of Quartrond",
                RequiredSkill = 51.4
            });
            this.Charts.Add(new() 
            {
                Artist = "Sean Lenard B. vs Martix Lenard",
                Song = "QR2",
                RequiredSkill = 1.14
            });
            this.Charts.Add(new()
            {
                Artist = "Sean Lenard B.",
                Song = "The Final Descent of Quartrond",
                RequiredSkill = 5.14
            });
            this.Charts.Add(new()
            {
                Artist = "Sean Lenard B.",
                Song = "Valley of Aer",
                RequiredSkill = 1.919
            });
            this.Charts.Add(new()
            {
                Artist = "takehirotei",
                Song = "Chronoexplorers",
                RequiredSkill = 81
            });

            for (int i = 0; i < 10; i++)
            {
                this.Charts.Add(new()
                {
                    Artist = "Random Artists",
                    Song = $"Test song suite No. {this.Random.Next(10)}",
                    RequiredSkill = Random.NextDouble() * 114.514
                });
            }
        }

        public GameContext(int seed)
        {
            this.Seed = seed; this.Random = new(seed);
            this.GenerateCharts();  // @cleanup: remove this, this is just for testing
        }
    }
}
