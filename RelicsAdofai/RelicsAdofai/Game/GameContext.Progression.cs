using System.Diagnostics;

namespace RelicsAdofai.Game
{
    public partial class GameContext
    {
        public double[] OtherPlayerScAmounts = [0.11, 8.88, 11.45, 19.9, 24.8, 66.6];

        public bool Unlocked_ScIncome_Tier1 = false;
        public bool Unlocked_ScIncome_Tier2 = false;
        public bool Unlocked_ScIncome_Tier3 = false;
        public bool Unlocked_Bounty_Planetary = false;
        public bool Unlocked_Bounty_Galactic = false;
        public bool Unlocked_Bounty_Universal = false;
        public Dictionary<string, bool> Unlocked_OtherPlayer_Sc = [];
        public Dictionary<string, bool> Unlocked_OtherPlayer_Recreation = [];
        public Dictionary<string, bool> Unlocked_OtherPlayer_BigSc = [];
        public bool Unlocked_MainQuests_NetworkingAndUnlocking = false;
        public bool Unlocked_MainQuests_Bounty = false;
        public bool Unlocked_MainQuests_EventCountCap = false;
        public void UpdateProgression()
        {
            if (!this.Unlocked_ScIncome_Tier1 && this.FollowerCount > 100)
            {
                this.Unlocked_ScIncome_Tier1 = true;
                this.ChoiceEventWeights.Add(new(1, () =>
                {
                    double moneyAmount = this.Random.NextDouble() * 10;
                    return ChoiceEvent.Info(
                        $"充电通知",
                        $"<p>你的一位粉丝给你充电<span class=\"color-money\">{moneyAmount:N2}</span></p>",
                        context => context.Money += moneyAmount);
                }));
            }
            if (!this.Unlocked_ScIncome_Tier2 && this.FollowerCount > 5000)
            {
                this.Unlocked_ScIncome_Tier2 = true;
                this.ChoiceEventWeights.Add(new(1, () =>
                {
                    double moneyAmount = this.Random.NextDouble() * 10;
                    return ChoiceEvent.Info(
                        $"充电通知",
                        $"<p>你的一位粉丝给你充电<span class=\"color-money\">{moneyAmount:N2}</span></p>",
                        context => context.Money += moneyAmount);
                }));
                this.ChoiceEventWeights.Add(new(1, () =>
                {
                    double moneyAmount = this.Random.NextDouble() * 50;
                    return ChoiceEvent.Info(
                        $"充电通知",
                        $"<p>你的一位粉丝给你充电<span class=\"color-money\">{moneyAmount:N2}</span></p>",
                        context =>
                        {
                            context.Money += moneyAmount;
                            context.FollowerCount *= 1.01;
                        });
                }));
            }
            if (!this.Unlocked_ScIncome_Tier3 && this.FollowerCount > 100000)
            {
                this.Unlocked_ScIncome_Tier3 = true;
                this.ChoiceEventWeights.Add(new(1, () =>
                {
                    return ChoiceEvent.Info(
                        $"上舰通知",
                        $"你的一位粉丝为你上舰（<span class=\"color-money\">198.00</span>）",
                        context =>
                        {
                            context.Money += 198;
                            context.FollowerCount *= 1.1;
                        });
                }));
            }
            if (!this.Unlocked_Bounty_Planetary && this.Skill > Math.Pow(this.MultiplierPerRating, 16))
            {
                this.Unlocked_Bounty_Planetary = true;
                this.ChoiceEventWeights.Add(new(1, () =>
                {
                    List<Chart> possibleCharts = [.. this.Charts.Where(c => c.RequiredSkill < 1024)];
                    var bountyChart = possibleCharts[this.Random.Next(possibleCharts.Count)];
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
                        $"谱面#{bountyChart.Id}悬赏",
                        $"{bountyChart.Creator}愿意给{bountyDays}天内首通{bountyChart} ({this.TranslatePgu(bountyChart)})的玩家发赏金<span class=\"color-money\">{bountyMoney:N2}</span>。" +
                        $"目前看下来，这张谱面" + tabubIsMineString,
                        context => { context.Money += bountyMoney; context.FollowerCount *= 1.01; context.Sanity += 0.1; },
                        _ => { },
                        context => context.ChartAccuracies.TryGetValue(bountyChart, out var _)
                    ).WithOnDayEnd((context, e) =>
                    {
                        if (context.Random.NextDouble() > tabubIsMineProbability) return;

                        e.RemainingDays = -1;
                        context.Log.Add($"<p>{context.OtherPlayers[context.Random.Next(context.OtherPlayers.Count)]}" +
                            $"已砍下谱面{bountyChart} ({this.TranslatePgu(bountyChart)})的悬赏<span class=\"color-money\">{bountyMoney:N2}</span></p>");
                    }).WithDayLimit(bountyDays, _ => { });
                }));
                this.ChoiceEventWeights.Add(new(1, () =>
                {
                    List<Chart> possibleCharts = [.. this.Charts.Where(c => c.RequiredSkill < 1024)];
                    var bountyChart = possibleCharts[this.Random.Next(possibleCharts.Count)];
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
                        $"谱面#{bountyChart.Id}悬赏",
                        $"{bountyChart.Creator}愿意给{bountyDays}天内第一个完美无瑕{bountyChart} ({this.TranslatePgu(bountyChart)})的玩家发赏金<span class=\"color-money\">{bountyMoney:N2}</span>。" +
                        $"目前看下来，这张谱面" + tabubIsMineString,
                        context => { context.Money += bountyMoney; context.FollowerCount *= 1.02; context.Sanity += 0.1; },
                        _ => { },
                        context => context.ChartAccuracies.TryGetValue(bountyChart, out var accuracy) && accuracy > 99.99
                    ).WithOnDayEnd((context, e) =>
                    {
                        if (context.Random.NextDouble() > tabubIsMineProbability) return;

                        e.RemainingDays = -1;
                        context.Log.Add($"<p>{context.OtherPlayers[context.Random.Next(context.OtherPlayers.Count)]}" +
                            $"已砍下谱面{bountyChart} ({this.TranslatePgu(bountyChart)})的悬赏<span class=\"color-money\">{bountyMoney:N2}</span></p>");
                    }).WithDayLimit(bountyDays, _ => { });
                }));
            }
            if (!this.Unlocked_Bounty_Galactic && this.Skill > Math.Pow(this.MultiplierPerRating, 32))
            {
                this.Unlocked_Bounty_Galactic = true;
                this.ChoiceEventWeights.Add(new(2, () =>
                {
                    List<Chart> possibleCharts = [.. this.Charts.Where(c => c.RequiredSkill >= 1024 && c.RequiredSkill < 1048576)];
                    var bountyChart = possibleCharts[this.Random.Next(possibleCharts.Count)];
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
                        $"谱面#{bountyChart.Id}悬赏",
                        $"{bountyChart.Creator}愿意给{bountyDays}天内首通{bountyChart} ({this.TranslatePgu(bountyChart)})的玩家发赏金<span class=\"color-money\">{bountyMoney:N2}</span>。" +
                        $"目前看下来，这张谱面" + tabubIsMineString,
                        context => { context.Money += bountyMoney; context.FollowerCount *= 1.03; context.Sanity += 0.1; },
                        _ => { },
                        context => context.ChartAccuracies.TryGetValue(bountyChart, out var _)
                    ).WithOnDayEnd((context, e) =>
                    {
                        if (context.Random.NextDouble() > tabubIsMineProbability) return;

                        e.RemainingDays = -1;
                        context.Log.Add($"<p>{context.OtherPlayers[context.Random.Next(context.OtherPlayers.Count)]}" +
                            $"已砍下谱面{bountyChart} ({this.TranslatePgu(bountyChart)})的悬赏<span class=\"color-money\">{bountyMoney:N2}</span></p>");
                    }).WithDayLimit(bountyDays, _ => { });
                }));
                this.ChoiceEventWeights.Add(new(1, () =>
                {
                    List<Chart> possibleCharts = [.. this.Charts.Where(c => c.RequiredSkill >= 1024 && c.RequiredSkill < 1048576)];
                    var bountyChart = possibleCharts[this.Random.Next(possibleCharts.Count)];
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
                        $"谱面#{bountyChart.Id}悬赏",
                        $"{bountyChart.Creator}愿意给{bountyDays}天内第一个完美无瑕{bountyChart} ({this.TranslatePgu(bountyChart)})的玩家发赏金<span class=\"color-money\">{bountyMoney:N2}</span>。" +
                        $"目前看下来，这张谱面" + tabubIsMineString,
                        context => { context.Money += bountyMoney; context.FollowerCount *= 1.05; context.Sanity += 0.1; },
                        _ => { },
                        context => context.ChartAccuracies.TryGetValue(bountyChart, out var accuracy) && accuracy > 99.99
                    ).WithOnDayEnd((context, e) =>
                    {
                        if (context.Random.NextDouble() > tabubIsMineProbability) return;

                        e.RemainingDays = -1;
                        context.Log.Add($"<p>{context.OtherPlayers[context.Random.Next(context.OtherPlayers.Count)]}" +
                            $"已砍下谱面{bountyChart} ({this.TranslatePgu(bountyChart)})的悬赏<span class=\"color-money\">{bountyMoney:N2}</span></p>");
                    }).WithDayLimit(bountyDays, _ => { });
                }));
            }
            if (!this.Unlocked_Bounty_Universal && this.Skill > Math.Pow(this.MultiplierPerRating, 48))
            {
                this.Unlocked_Bounty_Universal = true;
                this.ChoiceEventWeights.Add(new(3, () =>
                {
                    List<Chart> possibleCharts = [.. this.Charts.Where(c => c.RequiredSkill >= 1048576)];
                    var bountyChart = possibleCharts[this.Random.Next(possibleCharts.Count)];
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
                        $"谱面#{bountyChart.Id}悬赏",
                        $"{bountyChart.Creator}愿意给{bountyDays}天内首通{bountyChart} ({this.TranslatePgu(bountyChart)})的玩家发赏金<span class=\"color-money\">{bountyMoney:N2}</span>。" +
                        $"目前看下来，这张谱面" + tabubIsMineString,
                        context => { context.Money += bountyMoney; context.FollowerCount *= 1.05; context.Sanity += 0.15; },
                        _ => { },
                        context => context.ChartAccuracies.TryGetValue(bountyChart, out var _)
                    ).WithOnDayEnd((context, e) =>
                    {
                        if (context.Random.NextDouble() > tabubIsMineProbability) return;

                        e.RemainingDays = -1;
                        context.Log.Add($"<p>{context.OtherPlayers[context.Random.Next(context.OtherPlayers.Count)]}" +
                            $"已砍下谱面{bountyChart} ({this.TranslatePgu(bountyChart)})的悬赏<span class=\"color-money\">{bountyMoney:N2}</span></p>");
                    }).WithDayLimit(bountyDays, _ => { });
                }));
                this.ChoiceEventWeights.Add(new(2, () =>
                {
                    List<Chart> possibleCharts = [.. this.Charts.Where(c => c.RequiredSkill >= 1048576)];
                    var bountyChart = possibleCharts[this.Random.Next(possibleCharts.Count)];
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
                        $"谱面#{bountyChart.Id}悬赏",
                        $"{bountyChart.Creator}愿意给{bountyDays}天内第一个完美无瑕{bountyChart} ({this.TranslatePgu(bountyChart)})的玩家发赏金<span class=\"color-money\">{bountyMoney:N2}</span>。" +
                        $"目前看下来，这张谱面" + tabubIsMineString,
                        context => { context.Money += bountyMoney; context.FollowerCount *= 1.1; context.Sanity += 0.2; },
                        _ => { },
                        context => context.ChartAccuracies.TryGetValue(bountyChart, out var accuracy) && accuracy > 99.99
                    ).WithOnDayEnd((context, e) =>
                    {
                        if (context.Random.NextDouble() > tabubIsMineProbability) return;

                        e.RemainingDays = -1;
                        context.Log.Add($"<p>{context.OtherPlayers[context.Random.Next(context.OtherPlayers.Count)]}" +
                            $"已砍下谱面{bountyChart} ({this.TranslatePgu(bountyChart)})的悬赏<span class=\"color-money\">{bountyMoney:N2}</span></p>");
                    }).WithDayLimit(bountyDays, _ => { });
                }));
            }
            foreach (var player in this.OtherPlayers)
            {
                Debug.Assert(this.Unlocked_OtherPlayer_BigSc.ContainsKey(player), "Cannot find the player in the dictionary!");
                Debug.Assert(this.FriendlinessWithOtherPlayers.ContainsKey(player), "Cannot find the player in the dictionary!");
                Debug.Assert(this.Unlocked_OtherPlayer_Recreation.ContainsKey(player), "Cannot find the player in the dictionary!");
                if (!this.Unlocked_OtherPlayer_Sc[player] && this.FriendlinessWithOtherPlayers[player] > 8)
                {
                    this.Unlocked_OtherPlayer_Sc[player] = true;
                    this.ChoiceEventWeights.Add(new(1, () =>
                    {
                        double scAmount = this.OtherPlayerScAmounts[this.Random.Next(this.OtherPlayerScAmounts.Length)];
                        return ChoiceEvent.YesNo(
                            $"给{player}发SC",
                            $"{player}开播了，是否要给他刷一个<span class=\"color-money\">{scAmount:N2}</span>的礼物？",
                            context =>
                            {
                                context.Money -= scAmount;
                                context.FriendlinessWithOtherPlayers[player] += scAmount;
                                context.Sanity += 0.15;
                                context.Hour += 1;
                                if (context.Random.NextDouble() > scAmount / this.OtherPlayerScAmounts[^1]) return;

                                context.Log.Add($"<p>{player}很高兴，并且在接下来的直播中打了你点的谱面。在看直播的过程中，你学习到了一些更好的手法。</p>");
                                context.Skill *= 1.02;
                                context.FollowerCount *= 1.02;
                            },
                            _ => { },
                            context => context.Money > scAmount
                            ).WithDayLimit(1, _ => { });
                    }));
                }
                if (!this.Unlocked_OtherPlayer_Recreation[player] && this.FriendlinessWithOtherPlayers[player] > 32)
                {
                    this.Unlocked_OtherPlayer_Recreation[player] = true;
                    this.ChoiceEvents.Add(ChoiceEvent.Info(
                        $"你已成为{player}的群管理",
                        $"在长期观看和互动后，{player}将你设置为粉丝群管理。",
                        context => { context.Sanity += 0.1; context.FollowerCount *= 1.04; }));
                    this.ChoiceEventWeights.Add(new(2, () =>
                    {
                        return ChoiceEvent.YesNo(
                            $"{player}邀请你玩AmongSus",
                            "是否要玩2小时？",
                            context => { context.Hour += 2; context.FollowerCount *= 1.005; context.Sanity += 0.5; },
                            _ => { }).WithDayLimit(2, _ => { });
                    }));
                    this.ChoiceEventWeights.Add(new(2, () =>
                    {
                        return ChoiceEvent.YesNo(
                            $"{player}邀请你玩你打我猜",
                            "是否要玩1小时？",
                            context => { context.Hour += 1; context.FollowerCount *= 1.003; context.Sanity += 0.3; },
                            _ => { }).WithDayLimit(2, _ => { });
                    }));
                    this.ChoiceEventWeights.Add(new(1, () =>
                    {
                        return ChoiceEvent.YesNo(
                            $"{player}邀请你玩ScGo",
                            "是否要玩3小时？",
                            context =>
                            {
                                context.Hour += 3;
                                context.Skill *= 1.005;
                                context.FollowerCount *= 1.01;
                                context.Sanity += 0.8;
                            },
                            _ => { }).WithDayLimit(3, _ => { });
                    }));
                }
                if (!this.Unlocked_OtherPlayer_BigSc[player] && this.FriendlinessWithOtherPlayers[player] > 128)
                {
                    this.Unlocked_OtherPlayer_BigSc[player] = true;
                    this.ChoiceEventWeights.Add(new(1, () =>
                    {
                        string streamer = this.OtherPlayers[this.Random.Next(this.OtherPlayers.Count)];
                        return ChoiceEvent.YesNo(
                            $"给{streamer}上舰",
                            $"{streamer}开播了，是否要给他刷一个<span class=\"color-money\">198</span>的舰长？（不续舰不会毁号）",
                            context =>
                            {
                                context.Money -= 198;
                                context.Hour += 2;
                                context.Log.Add($"<p>{streamer}很高兴，并且在接下来的直播中打了你点的谱面。在看直播的过程中，你学习到了一些更好的手法。</p>");
                                context.Skill *= 1.05;
                                context.FollowerCount *= 1.1;
                            },
                            _ => { },
                            context => context.Money > 198
                            ).WithDayLimit(1, _ => { });
                    }));
                }
            }

            // Main Quests
            if (!this.Unlocked_MainQuests_EventCountCap && this.ChoiceEvents.Count >= 6)
            {
                this.Unlocked_MainQuests_EventCountCap = true;
                this.ChoiceEvents.Add(ChoiceEvent
                    .Info(
                        "事件数量有上限",
                        """
                        尽管每一天都会发生新的事件，但是你同时能够遇到的事件总数有限。超过阈值后，你将不会遇到新事件。
                        这既包含限时事件，也包含不限时事件。

                        一个重要的提示是，如果你短期内无法完成一个事件，那么最好的办法就是取消它。
                        这样，你可以遇到一些对你更有用的事件。

                        这个事件没有限时，但很明显你应该现在就完成这个事件。
                        """,
                        _ => { })
                    .MainQuest());
            }
            if (!this.Unlocked_MainQuests_Bounty && this.Skill > Math.Pow(this.MultiplierPerRating, 15))
            {
                this.Unlocked_MainQuests_Bounty = true;
                this.ChoiceEvents.Add(ChoiceEvent
                    .Info(
                        "赏金阶段",
                        """
                        <p>在这个游戏刚发布的时候，赏金任务是随机生成的。
                        这会导致作为新手的你遇到一些高难度谱的赏金事件，但是你又无力完成。
                        
                        现在赏金事件会根据你的<span class="color-skill">技术</span>自动调节。
                        总共有3个阶段，每个阶段分别解锁P、G、U谱面的赏金事件。
                        你现在的<span class="color-skill">技术</span>已经非常接近解锁第一阶段了！
                        加油！</p>
                        """,
                        _ => { })
                    .MainQuest());
            }
            if (!this.Unlocked_MainQuests_NetworkingAndUnlocking && this.FriendlinessWithOtherPlayers.Any(p => p.Value > 0))
            {
                this.Unlocked_MainQuests_NetworkingAndUnlocking = true;
                this.ChoiceEvents.Add(ChoiceEvent
                    .Info(
                        "社交与事件解锁",
                        """
                        你刚刚与Relics of Adofai中的一位玩家（虚拟的，有可能以后会添加多人联机）互动。
                        你可以在“社交”窗口中查看你和其他玩家的互动情况。

                        在这个游戏中，不是所有事件都是一开始就有的。随着你做出更多的选择，你会遇到不同种类的事件。
                        比如，如果你和一位玩家的社交指数较高，则会解锁新的独特事件。

                        当然，进度不仅仅包括社交。在不久的将来（或者你可能已经看到了）你会解锁打谱赏金事件。
                        
                        所有事件解锁都有一定的条件。你可以在“统计”窗口中查看解锁进度。
                        """,
                        _ => { })
                    .MainQuest());
            }
        }
    }
}
