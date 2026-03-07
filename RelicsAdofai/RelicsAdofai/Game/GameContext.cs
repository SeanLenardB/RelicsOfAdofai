using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace RelicsAdofai.Game
{
    public partial class GameContext
    {
        public GameContext(int seed)
        {
            this.Seed = seed; this.Random = new(seed);
            this.ImportDefaultData();

            // @hack: currently the charts will not increase over the game time
            // we might change it to be dynamic throughout the game,
            // but for now, we will generate a fix amount of charts for players to play.
            this.GenerateCharts();
        }

        public int Version = 1;

        // Configuration
        public int Seed = 0;
        public int DailyEventCount = 0;
        public int DailyEnergyRecharge = 5;
        public double MultiplierPerRating = 1.41;

        // Gamestat
        public string PlayerName = "";
        public StringBuilder Log = new();

        // Gamestat, event-listened
        public int FollowerCount
        {
            get;
            set {
                this.Log.Append($"<p>名声: <span class=\"color-follower\">{field}</span> >>> <span class=\"color-follower\">{value}</span></p>");
                field = value;
            }
        } = 0;
        public double Skill {
            get;
            set
            {
                this.Log.Append($"<p>技术: <span class=\"color-skill\">{field:0.00}</span> >>> <span class=\"color-skill\">{value:0.00}</span></p>");
                field = value;
            }
        } = 0.0;
        public double Money {
            get;
            set
            {
                this.Log.Append($"<p>资金: <span class=\"color-money\">{field:0.00}</span> >>> <span class=\"color-money\">{value:0.00}</span></p>");
                field = value;
            }
        } = 0.0;
        // @note: this is not listened, although it might be useful to listen to this.
        public int Day = 1;
        public double Hour { get; set { this.Log.Append($"<p>时间经过{(value - field <= 0 ? value - field + 24 : value - field)}小时</p>"); field = value; } } = 8.0;

        // Internal
        public Random Random;
        public List<ChoiceEvent> ChoiceEvents = [];
        public List<Chart> Charts = [];
        public List<Tuple<int, Func<ChoiceEvent>>> ChoiceEventWeights = [];
        public List<Tuple<string, string>> ArtistsAndSongs = [];
        public List<string> Creators = [];



        public void GenerateEvents()
        {
            int totalWeight = this.ChoiceEventWeights.Sum(c => c.Item1);
            int eventCount = 2 + this.Random.Next(4);  // @cleanup: should not be hardcoded and should be configurable.
            for (int i = 0; i < eventCount; i++)
            {
                int randomWeight = this.Random.Next(totalWeight);
                foreach (var eventPair in this.ChoiceEventWeights)
                {

                    randomWeight -= eventPair.Item1;
                    if (randomWeight >= 0) continue;

                    this.ChoiceEvents.Add(eventPair.Item2());
                }
            }
        }

        public void GenerateCharts()
        {
            for (int i = 0; i < 400; i++)
            {
                (var artist, var song) = this.ArtistsAndSongs[this.Random.Next(this.ArtistsAndSongs.Count)];
                var creator = this.Creators[this.Random.Next(this.Creators.Count)];

                // @note: this might not be necessary and might end up being changed.
                // right now we don't have a good estimation of how the game will end like
                // so we will play safe and generate charts that have the similar difficulty
                // of the player's skill.
                double requiredSkill = Math.Pow(this.MultiplierPerRating, this.Random.NextDouble() * 60);

                this.Charts.Add(new()
                {
                    Artist = artist,
                    Song = song,
                    Creator = creator,
                    RequiredSkill = requiredSkill,
                });
            }
        }
        
        // @todo: players can get more familiar with different charts
        // and the familiarity can help them clear charts easier with a lower skill level.

        // The current skill system is based on a logarithmatic scale.
        // That is, the skill number of P2 = 2 * P1, P3 = 2 * P2, etc.
        // However this is a simplification and ideal situation of the ratings.
        // In reality, the logarithmatic scalar might not be a constant, that is,
        // if P2 = 2 * P1, then U2 = 8 * U1 or so.
        public AttemptResult Attempt(Chart chart)
        {
            // This is a rough estimation of how playing adofai is. By no means is this an accurate model.

            // @note: Players attempting charts that are too hard will not gain anything from the chart (no skill gain).
            // And it's a guaranteed failure.
            // If I end up adding SANITY as an element of the game, I will change this.
            // Because playing easy charts regains your sanity.
            if (chart.RequiredSkill > this.Skill * Math.Pow(this.MultiplierPerRating, 8)) return new() { HasCleared = false };

            // If the player is too good then it's a guaranteed pure perfect.
            // But this easy chart will not hone the player's skill because it's too easy.
            if (chart.RequiredSkill * Math.Pow(this.MultiplierPerRating, 8) < this.Skill) return new() { HasCleared = true, Accuracy = 100.0 };

            

            double clearChance = this.Skill / chart.RequiredSkill * Math.Pow(this.MultiplierPerRating, 8);
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
            this.GenerateEvents();
        }

        public string TranslatePgu(Chart chart)
        {
            // If the difficulty is 1.41, then its log is 1, that means a P2.
            int logRequiredSkill = (int)Math.Log(chart.RequiredSkill, this.MultiplierPerRating);
            Debug.Assert(logRequiredSkill >= 0 && logRequiredSkill < 60, "There is something wrong with the difficulty generator!");
            return ((logRequiredSkill / 20) switch { 0 => "P", 1 => "G", 2 => "U", _ => "E" }) + ((logRequiredSkill % 20) + 1).ToString();
        }
    }
}
