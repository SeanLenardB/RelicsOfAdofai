namespace RelicsAdofai.Game
{
    public partial class GameContext
    {
        public bool Unlocked_ScIncome_Tier1 = false;
        public bool Unlocked_ScIncome_Tier2 = false;
        public bool Unlocked_ScIncome_Tier3 = false;
        public bool Unlocked_Bounty_Planetary = false;
        public bool Unlocked_Bounty_Galactic = false;
        public bool Unlocked_Bounty_Universal = false;
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
        }
    }
}
