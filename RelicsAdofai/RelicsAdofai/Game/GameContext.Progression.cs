namespace RelicsAdofai.Game
{
    public partial class GameContext
    {
        public bool Unlocked_ScIncome_Tier1 = false;
        public bool Unlocked_ScIncome_Tier2 = false;
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
                        $"你的一位粉丝给你充电{moneyAmount:0.00}元",
                        context => context.Money += moneyAmount);
                }));
            }
            if (!this.Unlocked_ScIncome_Tier2 && this.FollowerCount > 1000)
            {
                this.Unlocked_ScIncome_Tier2 = true;
                this.ChoiceEventWeights.Add(new(1, () =>
                {
                    double moneyAmount = this.Random.NextDouble() * 10;
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
            }
        }
    }
}
