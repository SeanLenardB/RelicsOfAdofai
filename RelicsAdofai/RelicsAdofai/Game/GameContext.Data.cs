using System.Diagnostics.Eventing.Reader;

namespace RelicsAdofai.Game
{
    public partial class GameContext
    {
        // @hack: It's better to have a proper resource file to put all of these inside
        // but for now, this works (and probably, for a long time it still will do)
        //
        // There are also a lot of randomly generated data
        // the result is pretty bad, but for now we'll make do with it in the early stage.
        public void ImportDefaultData()
        {
            this.ArtistsAndSongs.Add(new("quartrond", "QR3"));
            this.ArtistsAndSongs.Add(new("quartrond", "The First Ascent of Adofai"));
            this.ArtistsAndSongs.Add(new("quartrond", "Slime Gem"));
            this.ArtistsAndSongs.Add(new("quartrond", "The Relics of Adofai"));
            this.ArtistsAndSongs.Add(new("quartrond", "Ocean Peace ~The Ending~"));
            this.ArtistsAndSongs.Add(new("quartrond", "That Departure"));
            this.ArtistsAndSongs.Add(new("quartrond", "Oceantriangle"));

            this.ArtistsAndSongs.Add(new("2-50", "ARTISAN-1"));
            this.ArtistsAndSongs.Add(new("2-50", "ARTISAN-2"));
            this.ArtistsAndSongs.Add(new("2-50", "ARTISAN-3"));
            this.ArtistsAndSongs.Add(new("2-50", "ARTISAN-4"));
            this.ArtistsAndSongs.Add(new("2-50", "ARTISAN-5"));
            this.ArtistsAndSongs.Add(new("2-50", "ARTISAN-6"));

            this.ArtistsAndSongs.Add(new("Crysanthemum", "GOODBYE (BPM) 2020"));
            this.ArtistsAndSongs.Add(new("Crysanthemum", "GOODBYE (BPM) 2021"));
            this.ArtistsAndSongs.Add(new("Crysanthemum", "GOODBYE (BPM) 2022"));
            this.ArtistsAndSongs.Add(new("Crysanthemum", "GOODBYE (BPM) 2023"));
            this.ArtistsAndSongs.Add(new("Crysanthemum", "GOODBYE (BPM) 2024"));
            this.ArtistsAndSongs.Add(new("Crysanthemum", "GOODBYE (BPM) 2025"));
            this.ArtistsAndSongs.Add(new("Crysanthemum", "GOODBYE (BPM) 2026"));
            this.ArtistsAndSongs.Add(new("Crysanthemum", "Trillion QQ"));
            this.ArtistsAndSongs.Add(new("Crysanthemum", "[wc]"));

            this.ArtistsAndSongs.Add(new("Fnares", "NAND NAND NAND"));
            this.ArtistsAndSongs.Add(new("Fnares", "mono_leg"));
            this.ArtistsAndSongs.Add(new("Fnares", "RADIO, UMBRALOGOS"));
            this.ArtistsAndSongs.Add(new("Fnares", "Photographed As Purple And Pristine"));
            this.ArtistsAndSongs.Add(new("Fnares", "They Desire To Walk"));
            this.ArtistsAndSongs.Add(new("Fnares", "WALL"));
            this.ArtistsAndSongs.Add(new("Fnares", "goto EXIT"));
            this.ArtistsAndSongs.Add(new("Fnares", "settings"));
            this.ArtistsAndSongs.Add(new("Fnares", "cast"));

            this.ArtistsAndSongs.Add(new("Infinite Series", "Beneath the Boundary"));
            this.ArtistsAndSongs.Add(new("Infinite Series", "Laws of the Ordered Bisection"));
            this.ArtistsAndSongs.Add(new("Infinite Series", "This is Proved"));
            this.ArtistsAndSongs.Add(new("Infinite Series", "The Goose Chase"));

            this.ArtistsAndSongs.Add(new("Mulp", "Marscircle"));
            this.ArtistsAndSongs.Add(new("Mulp", "Acro"));
            this.ArtistsAndSongs.Add(new("Mulp", "J"));
            this.ArtistsAndSongs.Add(new("Mulp", "M"));

            this.ArtistsAndSongs.Add(new("Dilucin_", "Risen Orchestra"));
            this.ArtistsAndSongs.Add(new("Dilucin_", "Dilithium"));
            this.ArtistsAndSongs.Add(new("Dilucin_", "Darmstadium"));
            this.ArtistsAndSongs.Add(new("Dilucin_", "Tritanium"));
            this.ArtistsAndSongs.Add(new("Dilucin_", "Found Requiem"));
            this.ArtistsAndSongs.Add(new("Dilucin_", "Memories of Echoes"));



            this.Creators.Add("Sucrose_No_Lactose");
            this.Creators.Add("SSSeanLB");
            this.Creators.Add("Beta");
            this.Creators.Add("Delta");
            this.Creators.Add("Agbr_");
            this.Creators.Add("Miphy");
            this.Creators.Add("Sasinis");
            this.Creators.Add("Zigogo");
            this.Creators.Add("Hajimi");
            this.Creators.Add("Lettuce");
            this.Creators.Add("Ran_Hang");
            this.Creators.Add("Steve512");
            this.Creators.Add("AdfEventLib");
            this.Creators.Add("Magicshaper");
            int singleCreatorCount = this.Creators.Count;
            for (int i = 0; i < singleCreatorCount; i++)
            {
                for (int j = i + 1; j < singleCreatorCount; j++)
                {
                    this.Creators.Add($"{this.Creators[i]} & {this.Creators[j]}");
                }
            }



            // @todo: add more players when we get to "Networking DLC".
            this.OtherPlayers.Add("Reppij");
            this.OtherPlayers.Add("Teaj_");
            this.OtherPlayers.Add("Vate_Jalery");
            this.OtherPlayers.Add("Dodging ball");
            this.OtherPlayers.Add("FireCave");
            this.OtherPlayers.Add("ikun_");
            this.OtherPlayers.Add("HuiZai_");



            this.ChoiceEventWeights.Add(new(2, () =>
            {
                int bountyIndex = this.Random.Next(this.Charts.Count);
                var bountyChart = this.Charts[bountyIndex];
                int bountyDays = this.Random.Next(7);
                double bountyMoney = (this.Random.NextDouble() * 40) + 10;
                return ChoiceEvent.MeetCriteria(
                    bountyDays,
                    $"谱面#{bountyIndex}悬赏",
                    $"{bountyChart.Creator}愿意给{bountyDays}天内击破{bountyChart} ({this.TranslatePgu(bountyChart)})的所有玩家发赏金<span class=\"color-money\">{bountyMoney:0.00}</span>",
                    context => { context.Money += bountyMoney; },
                    _ => { },
                    context => context.ChartAccuracies.TryGetValue(bountyChart, out var _)
                ).WithDayLimit(bountyDays, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                int bountyIndex = this.Random.Next(this.Charts.Count);
                var bountyChart = this.Charts[bountyIndex];
                int bountyDays = this.Random.Next(7);
                double bountyMoney = (this.Random.NextDouble() * 50) + 50;
                double bountyAccuracy = this.Random.NextDouble() + 99;
                return ChoiceEvent.MeetCriteria(
                    bountyDays,
                    $"谱面#{bountyIndex}悬赏",
                    $"{bountyChart.Creator}愿意给{bountyDays}天内击破{bountyChart} ({this.TranslatePgu(bountyChart)})，且精准度高于{bountyAccuracy}的所有玩家发赏金<span class=\"color-money\">{bountyMoney:0.00}</span>",
                    context => { context.Money += bountyMoney; context.FollowerCount *= 1.01; },
                    _ => { },
                    context => context.ChartAccuracies.TryGetValue(bountyChart, out var accuracy) && accuracy >= bountyAccuracy
                ).WithDayLimit(bountyDays, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                int bountyIndex = this.Random.Next(this.Charts.Count);
                var bountyChart = this.Charts[bountyIndex];
                int bountyDays = this.Random.Next(7);
                double bountyMoney = (this.Random.NextDouble() * 80) + 80;
                return ChoiceEvent.MeetCriteria(
                    bountyDays,
                    $"谱面#{bountyIndex}悬赏",
                    $"{bountyChart.Creator}愿意给{bountyDays}天内完美无瑕{bountyChart} ({this.TranslatePgu(bountyChart)})的所有玩家发赏金<span class=\"color-money\">{bountyMoney:0.00}</span>",
                    context => { context.Money += bountyMoney; context.FollowerCount *= 1.02; },
                    _ => { },
                    context => context.ChartAccuracies.TryGetValue(bountyChart, out var accuracy) && accuracy > 99.99
                ).WithDayLimit(bountyDays, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(2, () =>
            {
                int bountyIndex = this.Random.Next(this.Charts.Count);
                var bountyChart = this.Charts[bountyIndex];
                int bountyDays = this.Random.Next(7);
                double bountyMoney = (this.Random.NextDouble() * 100) + 100;

                double tabubIsMineProbability = this.Random.NextDouble() * 100;
                string tabubIsMineString;
                if (tabubIsMineProbability < 0.1) tabubIsMineString = "几乎没有人在打";
                else if (tabubIsMineProbability < 0.3) tabubIsMineString = "有一些人可能会尝试";
                else if (tabubIsMineProbability < 0.5) tabubIsMineString = "偶尔有一两个人在练习";
                else if (tabubIsMineProbability < 0.7) tabubIsMineString = "有不少人在打";
                else if (tabubIsMineProbability < 0.9) tabubIsMineString = "现在有很多人在抢";
                else tabubIsMineString = "很快就会被拿下";

                return ChoiceEvent.MeetCriteria(
                    bountyDays,
                    $"谱面#{bountyIndex}悬赏",
                    $"{bountyChart.Creator}愿意给{bountyDays}天内首通{bountyChart} ({this.TranslatePgu(bountyChart)})的玩家发赏金<span class=\"color-money\">{bountyMoney:0.00}</span>。" +
                    $"目前看下来，这张谱面" + tabubIsMineString,
                    context => { context.Money += bountyMoney; context.FollowerCount *= 1.1; },
                    _ => { },
                    context => context.ChartAccuracies.TryGetValue(bountyChart, out var _)
                ).WithOnDayEnd((context, e) =>
                {
                    if (context.Random.NextDouble() > tabubIsMineProbability) return;

                    e.RemainingDays = -1;
                    context.Log.Add($"<p>{context.OtherPlayers[context.Random.Next(context.OtherPlayers.Count)]}" +
                        $"已砍下谱面{bountyChart} ({this.TranslatePgu(bountyChart)})的悬赏<span class=\"color-money\">{bountyMoney:0.00}</span></p>");
                }).WithDayLimit(bountyDays, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                int bountyIndex = this.Random.Next(this.Charts.Count);
                var bountyChart = this.Charts[bountyIndex];
                int bountyDays = this.Random.Next(7);
                double bountyMoney = (this.Random.NextDouble() * 100) + 100;

                double tabubIsMineProbability = this.Random.NextDouble() * 100;
                string tabubIsMineString;
                if (tabubIsMineProbability < 0.1) tabubIsMineString = "几乎没有人在打";
                else if (tabubIsMineProbability < 0.3) tabubIsMineString = "有一些人可能会尝试";
                else if (tabubIsMineProbability < 0.5) tabubIsMineString = "偶尔有一两个人在练习";
                else if (tabubIsMineProbability < 0.7) tabubIsMineString = "有不少人在打";
                else if (tabubIsMineProbability < 0.9) tabubIsMineString = "现在有很多人在抢";
                else tabubIsMineString = "很快就会被拿下";

                return ChoiceEvent.MeetCriteria(
                    bountyDays,
                    $"谱面#{bountyIndex}悬赏",
                    $"{bountyChart.Creator}愿意给{bountyDays}天内第一个完美无瑕{bountyChart} ({this.TranslatePgu(bountyChart)})的玩家发赏金<span class=\"color-money\">{bountyMoney:0.00}</span>。" +
                    $"目前看下来，这张谱面" + tabubIsMineString,
                    context => { context.Money += bountyMoney; context.FollowerCount *= 1.2; },
                    _ => { },
                    context => context.ChartAccuracies.TryGetValue(bountyChart, out var accuracy) && accuracy > 99.99
                ).WithOnDayEnd((context, e) =>
                {
                    if (context.Random.NextDouble() > tabubIsMineProbability) return;

                    e.RemainingDays = -1;
                    context.Log.Add($"<p>{context.OtherPlayers[context.Random.Next(context.OtherPlayers.Count)]}" +
                        $"已砍下谱面{bountyChart} ({this.TranslatePgu(bountyChart)})的悬赏<span class=\"color-money\">{bountyMoney:0.00}</span></p>");
                }).WithDayLimit(bountyDays, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(2, () =>
            {
                string streamer = this.OtherPlayers[this.Random.Next(this.OtherPlayers.Count)];
                return ChoiceEvent.YesNo(
                    $"给{streamer}发SC",
                    $"{streamer}开播了，是否要给他刷一个<span class=\"color-money\">10</span>的礼物？",
                    context =>
                    {
                        if (context.Random.NextDouble() > 0.2) return;

                        context.Log.Add($"<p>{streamer}很高兴，并且在接下来的直播中打了你点的谱面。在看直播的过程中，你学习到了一些更好的手法。</p>");
                        context.Skill *= 1.02;

                        if (context.Random.NextDouble() > 0.2) return;
                        context.FollowerCount *= 1.02;
                    },
                    _ => { },
                    context => context.Money > 10
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(2, () =>
            {
                string streamer = this.OtherPlayers[this.Random.Next(this.OtherPlayers.Count)];
                return ChoiceEvent.YesNo(
                    $"给{streamer}发SC",
                    $"{streamer}开播了，是否要给他刷一个<span class=\"color-money\">20</span>的礼物？",
                    context =>
                    {
                        if (context.Random.NextDouble() > 0.2) return;

                        context.Log.Add($"<p>{streamer}很高兴，并且在接下来的直播中打了你点的谱面。在看直播的过程中，你学习到了一些更好的手法。</p>");
                        context.Skill *= 1.05;

                        if (context.Random.NextDouble() > 0.5) return;
                        context.FollowerCount *= 1.02;
                    },
                    _ => { },
                    context => context.Money > 20
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(2, () =>
            {
                string streamer = this.OtherPlayers[this.Random.Next(this.OtherPlayers.Count)];
                return ChoiceEvent.YesNo(
                    $"给{streamer}发SC",
                    $"{streamer}开播了，是否要给他刷一个<span class=\"color-money\">50</span>的礼物？",
                    context =>
                    {
                        if (context.Random.NextDouble() > 0.4) return;

                        context.Log.Add($"<p>{streamer}很高兴，并且在接下来的直播中打了你点的谱面。在看直播的过程中，你学习到了一些更好的手法。</p>");
                        context.Skill *= 1.1;

                        if (context.Random.NextDouble() > 0.5) return;
                        context.FollowerCount *= 1.05;
                    },
                    _ => { },
                    context => context.Money > 50
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                string streamer = this.OtherPlayers[this.Random.Next(this.OtherPlayers.Count)];
                return ChoiceEvent.YesNo(
                    $"给{streamer}发SC",
                    $"{streamer}开播了，是否要给他刷一个<span class=\"color-money\">100</span>的礼物？",
                    context =>
                    {
                        if (context.Random.NextDouble() > 0.8) return;

                        context.Log.Add($"<p>{streamer}很高兴，并且在接下来的直播中打了你点的谱面。在看直播的过程中，你学习到了一些更好的手法。</p>");
                        context.Skill *= 1.1;
                        context.FollowerCount *= 1.05;
                    },
                    _ => { },
                    context => context.Money > 100
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                string streamer = this.OtherPlayers[this.Random.Next(this.OtherPlayers.Count)];
                return ChoiceEvent.YesNo(
                    $"给{streamer}上舰",
                    $"{streamer}开播了，是否要给他刷一个<span class=\"color-money\">198</span>的舰长？（不续舰不会毁号）",
                    context =>
                    {
                        context.Log.Add($"<p>{streamer}很高兴，并且在接下来的直播中打了你点的谱面。在看直播的过程中，你学习到了一些更好的手法。</p>");
                        context.Skill *= 1.1;
                        context.FollowerCount *= 1.1;
                    },
                    _ => { },
                    context => context.Money > 198
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                return ChoiceEvent.YesNo(
                    $"打一会儿Gtg",
                    $"是否要打4小时Gtg休息一下？",
                    context =>
                    {
                        for (int i = 0; i < 20; i++)  // @note: the 10 here is derived from 2h / 0.2h/chart.
                            context.PracticeChart(context.Charts[context.Random.Next(context.Charts.Count)]);
                        context.Skill *= 1.03;

                        if (context.Random.NextDouble() > 0.8) return;
                        context.FollowerCount *= 1.03;
                    },
                    _ => { }
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(2, () =>
            {
                return ChoiceEvent.YesNo(
                    $"打一会儿Gtg",
                    $"是否要打2小时Gtg休息一下？",
                    context =>
                    {
                        for (int i = 0; i < 10; i++)  // @note: the 10 here is derived from 2h / 0.2h/chart.
                            context.PracticeChart(context.Charts[context.Random.Next(context.Charts.Count)]);
                        context.Skill *= 1.01;

                        if (context.Random.NextDouble() > 0.8) return;
                        context.FollowerCount *= 1.01;
                    },
                    _ => { }
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(2, () =>
            {
                return ChoiceEvent.YesNo(
                    $"打一会儿Gtg",
                    $"是否要打1小时Gtg休息一下？",
                    context =>
                    {
                        for (int i = 0; i < 5; i++)  // @note: ditto.
                            context.PracticeChart(context.Charts[context.Random.Next(context.Charts.Count)]);
                        context.Skill *= 1.003;

                        if (context.Random.NextDouble() > 0.5) return;
                        context.FollowerCount *= 1.01;
                    },
                    _ => { }
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(2, () =>
            {
                double moneyAmount = (this.Random.NextDouble() * 15) + 5;
                return ChoiceEvent.Info(
                    $"充电通知",
                    $"你的一位粉丝给你充电{moneyAmount:0.00}元",
                    context => context.Money += moneyAmount);
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                double moneyAmount = this.Random.NextDouble() * 50;
                return ChoiceEvent.Info(
                    $"充电通知",
                    $"你的一位粉丝给你充电{moneyAmount:0.00}元",
                    context =>
                    {
                        context.Money += moneyAmount;
                        context.FollowerCount *= 1.01;
                    });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                return ChoiceEvent.YesNo(
                    $"升级键盘",
                    $"有一些Adofai玩家换了一种新的键盘，看上去还挺顺手，要不要也买一把？",
                    context =>
                    {
                        if (context.Random.NextDouble() > 0.3) return;
                        context.Skill *= 1.05;

                        if (context.Random.NextDouble() > 0.2) return;
                        context.FollowerCount *= 1.02;
                    },
                    _ => { },
                    context => context.Money > 300
                    );
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                return ChoiceEvent.YesNo(
                    $"升级摄像头",
                    $"你的摄像头有一点旧了，要不要升级一下摄像头，增加机位或是提高画质？",
                    context =>
                    {
                        if (context.Random.NextDouble() > 0.5) return;
                        context.FollowerCount *= 1.05;
                    },
                    _ => { },
                    context => context.Money > 300
                    );
            }));
        }

        public void ImportStartupEvents()
        {
            this.ChoiceEvents.Add(
                ChoiceEvent.Info(
                    $"欢迎转世者，{this.PlayerName}！",
                    """
                    欢迎来到Relics of Adofai。这条信息是你的第一个教程。

                    Relics of Adofai是一个基于决策的游戏。
                    界面左上角有你的基本属性。每一种属性数值都有独特的颜色，方便你在其他地方快速分辨。
                      > “技术”是你作为Adofai玩家最重要的数值。你的“技术”点数越高，谱面通过的概率就越高。所有谱面都在界面左侧紫色框内。
                      > “名声”会影响你在社区中的地位。粉丝多也许可以带来更多和其他玩家合作的机会，但也可能带来更困难的决策！
                      > “资金”可以通过发布谱面击破视频等活动获得，请保证充足的资金，否则你可能会在关键关头少一些可能的选择。

                    每一天你都会遇到一些随机事件。根据你前世的经验，以及这一世的情况，做出你的选择！
                    每一个选择都会有不同的结果。

                    在左侧紫色谱面区域内，你可以自由选择谱面练习段落或挑战击破。练谱和打谱都需要时间。
                    每一天从8:00开始（界面右上角），行动会消耗一定的时间。
                    有一些事件有时效性，这些事件的右上角会显示剩余天数（例如本事件）。
                    请注意时间，在23:00后将强制结束一天。
                    
                    你有一个选择：如果现在完成这个事件，你会获得更多的资金，但是会消耗比较多的时间。
                    或者，你可以今天不处理这个事件。到第二天时，这个事件会自动结束，你获得的资金会更少，但是完全不会消耗你宝贵的时间。
                    点击下面的“OK”就可以完成这个事件，或者不要管这个事件。
                    
                    这一世，祝你好运！
                    """,
                    context =>
                    {
                        context.Skill = 1;
                        context.Money = 72.7;
                        context.Hour += 6;
                    })
                .WithDayLimit(
                    0,
                    context =>
                    {
                        context.Skill = 1;
                        context.Money = 11.4;
                    }
                ));
        }
    }
}
