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
        public List<string> Log = [];

        // Gamestat, event-listened
        public double FollowerCount
        {
            get;
            set {
                this.Log.Add($"<p>名声: <span class=\"color-follower\">{field}</span> >>> <span class=\"color-follower\">{value}</span></p>");
                field = value;
            }
        } = 0.0;
        public double Skill {
            get;
            set
            {
                this.Log.Add($"<p>技术: <span class=\"color-skill\">{field:0.00}</span> >>> <span class=\"color-skill\">{value:0.00}</span></p>");
                field = value;
            }
        } = 0.0;
        public double Money {
            get;
            set
            {
                this.Log.Add($"<p>资金: <span class=\"color-money\">{field:0.00}</span> >>> <span class=\"color-money\">{value:0.00}</span></p>");
                field = value;
            }
        } = 0.0;
        // @note: this is not listened, although it might be useful to listen to this.
        public int Day = 1;
        public double Hour
        {
            get; set
            {
                if (value > 23.0)
                {
                    this.FinishDay();
                    this.Log.Add($"<p><strong>超过23:00，已强制结束第{Day - 1}天</strong></p>");
                }
                else
                {
                    this.Log.Add($"<p>时间经过{(value - field <= 0 ? value - field + 24 : value - field):0.0}小时</p>");
                    field = value;
                }
            }
        } = 8.0;

        // Internal
        public Random Random;
        public List<ChoiceEvent> ChoiceEvents = [];
        public List<Chart> Charts = [];
        public Dictionary<Chart, double> ChartFamiliarities = [];
        public Dictionary<Chart, double> ChartAccuracies = [];
        public List<Tuple<int, Func<ChoiceEvent>>> ChoiceEventWeights = [];
        public List<Tuple<string, string>> ArtistsAndSongs = [];
        public List<string> Creators = [];
        public List<string> OtherPlayers = [];



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

                    this.ChoiceEvents.Add(eventPair.Item2()); break;
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
                    Id = i + 1,
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
        public void AttemptChart(Chart chart)
        {
            this.Hour += 0.2;
            // This is a rough estimation of how playing adofai is. By no means is this an accurate model.

            // @note: Players attempting charts that are too hard will not gain anything from the chart (no skill gain).
            // And it's a guaranteed failure.
            // If I end up adding SANITY as an element of the game, I will change this.
            // Because playing easy charts regains your sanity.
            if (chart.RequiredSkill > this.Skill * Math.Pow(this.MultiplierPerRating, 8))
            {
                this.Log.Add($"<p>尝试谱面{chart}失败，判定万紫千红</p>");
                return;
            }

            // If the player is too good then it's a guaranteed pure perfect.
            // But this easy chart will not hone the player's skill because it's too easy.
            if (chart.RequiredSkill * Math.Pow(this.MultiplierPerRating, 8) < this.Skill)
            {
                this.Log.Add($"<p>击破谱面{chart}，精准<span style=\"color: gold\">100.00%</span>（啊！完美无瑕！）</p>");
                this.ChartAccuracies[chart] = 100;
                this.ChoiceEvents.Add(ChoiceEvent.YesNo(
                    "发布击破视频",
                    $"是否发布{chart}的击破视频？",
                    context => { context.FollowerCount += Math.Sqrt(chart.RequiredSkill); context.Hour += 0.5; },
                    _ => { }));
                return;
            }

            

            double clearChance = this.Skill / chart.RequiredSkill / Math.Pow(this.MultiplierPerRating, 8);
            double familiarity = this.ChartFamiliarities.GetValueOrDefault(chart);
            clearChance = (clearChance + familiarity) / (1 + familiarity);
            if (this.Random.NextDouble() > clearChance)
            {
                this.Log.Add($"<p>尝试谱面{chart}失败，死在{Math.Floor(clearChance * 100)}%</p>");
                return;
            }

            // @hack: we will reuse the clear chance and estimate the clear accuracy with it.
            // From 96.00, we roll random steps to obtain the preeliminary accuracy.
            // Then, we will introduce a random deduction by the step value.
            double accuracy = 96.0;
            int stepIndex = 0;
            bool canStep = true;
            while (canStep)
            {
                if (this.Random.NextDouble() > clearChance) { canStep = false; }
                accuracy += this.AccuracySteps[stepIndex];
                stepIndex++;
                if (stepIndex >= this.AccuracySteps.Length) { break; }
            }
            stepIndex--;
            accuracy -= this.AccuracySteps[stepIndex] * this.Random.NextDouble();
            bool isPurePerfect = accuracy > 99.99;

            if (isPurePerfect)
                this.Log.Add($"<p>击破谱面{chart}，精准<span style=\"color: gold\">100.00%</span>（啊！完美无瑕！）</p>");
            else
                this.Log.Add($"<p>击破谱面{chart}，精准<span style=\"color: lightgoldenrodyellow\">{accuracy:0.00}%</span></p>");

            if (this.ChartAccuracies.TryGetValue(chart, out var previousAccuracy) && previousAccuracy > accuracy) return;
            this.ChartAccuracies[chart] = isPurePerfect ? 100 : accuracy;  // prevent precision loss
            this.Skill += (accuracy - previousAccuracy) / 100 * chart.RequiredSkill * Math.Pow(this.MultiplierPerRating, 1);

            this.ChoiceEvents.Add(ChoiceEvent.YesNo(
                "发布击破视频",
                $"是否发布{chart}的击破视频？",
                context => { context.FollowerCount += Math.Sqrt(chart.RequiredSkill * Math.Pow(accuracy / 100, 4)); context.Hour += 0.5; },
                _ => { }));
        }
        public double[] AccuracySteps = 
        [
            0.40, 0.40,                          // 96.80
            0.30, 0.30, 0.30, 0.30,              // 98.00
            0.20, 0.20, 0.20,                    // 98.60
            0.10, 0.10, 0.10, 0.10,              // 99.00 
            0.05, 0.05, 0.05, 0.05, 0.05, 0.05,  // 99.30
            0.05, 0.05, 0.05, 0.05, 0.05, 0.05,  // 99.60
            0.04, 0.04, 0.04, 0.04, 0.04,        // 99.80
            0.02, 0.02, 0.02, 0.02, 0.02,        // 99.90
            0.02, 0.02, 0.02, 0.02, 0.02,        // 100.00
        ];
        public void PracticeChart(Chart chart)
        {
            this.Hour += 0.5;

            double previousFamiliarity = this.ChartFamiliarities.GetValueOrDefault(chart);
            double interpolation = this.Skill / chart.RequiredSkill / Math.Pow(this.MultiplierPerRating, 4);  // @note: nerfed from 8 to 4
            if (interpolation > 1.0) interpolation = 1.0;

            double newFamiliarity = (previousFamiliarity + interpolation) / 2.0;
            this.ChartFamiliarities[chart] = newFamiliarity;

            this.Log.Add($"<p>练习谱面{chart}，" +
                $"熟练度: <span style=\"color: gray\">{previousFamiliarity:0.00%}</span> " +
                $">>> <span style=\"color: gray\">{newFamiliarity:0.00%}</span></p>");

            if (newFamiliarity < 0.3) return;
            this.Skill += (newFamiliarity - previousFamiliarity) * chart.RequiredSkill * Math.Pow(this.MultiplierPerRating, 0.5);
        }

        public void FinishDay()
        {
            this.Log.Clear();  // @note: might not be very ideal
            this.Day++;
            this.Hour = 8.0;

            this.ChoiceEvents.ForEach(e => e.RemainingDays--);
            foreach (var choiceEvent in this.ChoiceEvents)
            {
                if (choiceEvent.RemainingDays >= 0) continue;

                this.Log.Add($"<p>事件“{choiceEvent.Title}”已结束</p>");
                choiceEvent.OvertimeConsequence(this);
            }
            // @cleanup: This OnDayEnd loop is before the RemoveAll because there are events
            // where they have a chance to be discarded at the end of a day.
            //
            // As we can't modify the ChoiceEvents list in OnDayEnd itself
            // (which causes enumeration failure as the array got reallocated in memory)
            // we now manually assign the remaining days of those events to -1,
            // so, during the remove event phase they will get removed.
            this.ChoiceEvents.ForEach(e => e.OnDayEnd(this));
            this.ChoiceEvents.RemoveAll(e => e.RemainingDays < 0);


            foreach (var chart in this.ChartFamiliarities.Keys)
            {
                double previousFamiliarity = this.ChartFamiliarities[chart];
                if (previousFamiliarity >= 0.9) continue;

                double newFamiliarity = Math.Pow(previousFamiliarity, 1.5);
                this.Log.Add($"<p>谱面{chart}的熟练度下降: " +
                    $"<span style=\"color: gray;\">{previousFamiliarity:0.00%}</span> >>> " +
                    $"<span style=\"color: gray;\">{newFamiliarity:0.00%}</span></p>");
                this.ChartFamiliarities[chart] = newFamiliarity;
            }

            if (this.Day % 7 == 0)
            {
                double videoIncome = Math.Log(this.FollowerCount) * 10;
                this.ChoiceEvents.Add(ChoiceEvent.Info(
                    "视频创作收入",
                    $"上周你的视频共产生收入<span class=\"color-money\">{videoIncome:0.00}</span>",
                    context => context.Money += videoIncome
                    ));
            }
            this.GenerateEvents();
        }

        public string TranslatePgu(Chart chart)
        {
            // If the difficulty is 1.41, then its log is 1, that means a P2.
            int logRequiredSkill = (int)Math.Log(chart.RequiredSkill, this.MultiplierPerRating);
            Debug.Assert(logRequiredSkill >= 0 && logRequiredSkill < 60, "There is something wrong with the difficulty generator!");
            return ((logRequiredSkill / 20) switch { 0 => "P", 1 => "G", 2 => "U", _ => "E" }) + ((logRequiredSkill % 20) + 1).ToString();
        }

        public void TakeChoice(ChoiceEvent choiceEvent, Choice choice)
        {
            bool removeSuccess = this.ChoiceEvents.Remove(choiceEvent);
            Debug.Assert(removeSuccess, "Cannot remove a choice event!");

            this.Log.Add($"事件“{choiceEvent.Title}”选择“{choice.Text}”");
            choice.Consequence(this);
        }
    }
}
