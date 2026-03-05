using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RelicsAdofai.Game
{
    public class GameContext
    {
        public GameContext(int seed)
        {
            this.Seed = seed; this.Random = new(seed);
            // @cleanup: remove this, this is just for testing
            this.GenerateCharts();
            this.GenerateEvents();
        }

        public int Version = 1;

        // Configuration
        public int Seed = 0;
        public int DailyEventCount = 0;
        public int DailyEnergyRecharge = 5;

        // Gamestat
        public string PlayerName = "";
        public int FollowerCount = 0;
        public double Skill = 0.0;
        public double Money = 0.0;
        public int Day = 1;
        public double Hour = 8.0;

        // Internal
        public Random Random;
        public List<ChoiceEvent> ChoiceEvents = [];
        public List<Chart> Charts = [];

        // @note: this should be placed in an instantiator.
        // This should not be a constant; events can be unlocked through progression.
        // That's also why we are repeatedly calculating total weight and not hardcode it.
        // @cleanup: this is too ugly, need cleanup but not that desperate.
        public Tuple<int, Func<ChoiceEvent>>[] ChoiceEventWeightConfiguration =
        [
            new(1, () => new())
        ];



        public void GenerateEvents()
        {
            int totalWeight = this.ChoiceEventWeightConfiguration.Sum(c => c.Item1);
            // @cleanup: this (event count) should not be hardcoded and should be configurable.
            int eventCount = this.Random.Next(8);
            for (int i = 0; i < eventCount; i++)
            {
                int randomWeight = this.Random.Next(totalWeight);
                foreach (var eventPair in this.ChoiceEventWeightConfiguration)
                {
                    randomWeight -= eventPair.Item1;
                    if (randomWeight >= 0) continue;

                    this.ChoiceEvents.Add(eventPair.Item2());
                }
            }
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

        // The current skill system is based on a logarithmatic scale.
        // That is, the skill number of P2 = 2 * P1, P3 = 2 * P2, etc.
        // However this is a simplification and ideal situation of the ratings.
        // In reality, the logarithmatic scalar might not be a constant, that is,
        // if P2 = 2 * P1, then U2 = 8 * U1 or so.
        // For now, we just assume that the scaling factor is a constant value of 2.
        public AttemptResult Attempt(Chart chart)
        {
            // This is a rough estimation of how playing adofai is. By no means is this an accurate model.

            // @note: Players attempting charts that are too hard will not gain anything from the chart (no skill gain).
            // And it's a guaranteed failure.
            // If I end up adding SANITY as an element of the game, I will change this.
            // Because playing easy charts regains your sanity.
            if (chart.RequiredSkill > this.Skill * Math.Pow(2, 8)) return new() { HasCleared = false };

            // If the player is too good then it's a guaranteed pure perfect.
            // But this easy chart will not hone the player's skill because it's too easy.
            if (chart.RequiredSkill * Math.Pow(2, 8) < this.Skill) return new() { HasCleared = true, Accuracy = 100.0 };

            

            double clearChance = this.Skill / chart.RequiredSkill * Math.Pow(2, 8);
            if (this.Random.NextDouble() > clearChance) { return new() { HasCleared = false }; }

            // @hack: we will reuse the clear chance and estimate the clear accuracy with it.
            // From 96.00, we roll random steps to obtain the preeliminary accuracy.
            // Then, we will introduce a random deduction by the step value.
            AttemptResult result = new() { HasCleared = true };
            int stepIndex = 0;
            bool canStep = true;
            while (canStep)
            {
                if (this.Random.NextDouble() > clearChance) { canStep = false; }
                result.Accuracy += this.AccuracySteps[stepIndex];
                stepIndex++;
                if (stepIndex >= this.AccuracySteps.Length) { break; }
            }
            stepIndex--;
            result.Accuracy -= this.AccuracySteps[stepIndex] * this.Random.NextDouble();

            return result;
        }
        public double[] AccuracySteps = 
        [
            0.40, 0.40,                          // 96.80
            0.30, 0.30, 0.30,                    // 98.00
            0.20, 0.20, 0.20,                    // 98.60
            0.10, 0.10, 0.10, 0.10,              // 99.00 
            0.05, 0.05, 0.05, 0.05, 0.05, 0.05,  // 99.30
            0.05, 0.05, 0.05, 0.05, 0.05, 0.05,  // 99.60
            0.04, 0.04, 0.04, 0.04, 0.04,        // 99.80
            0.02, 0.02, 0.02, 0.02, 0.02,        // 99.90
            0.02, 0.02, 0.02, 0.02, 0.02,        // 100.00
        ];


        public void FinishDay()
        {
            this.Day++;
            this.Hour = 8.0;
        }

    }
}
