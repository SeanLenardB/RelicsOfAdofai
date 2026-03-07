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
        }

        public void ImportStartupEvents()
        {
            this.ChoiceEvents.Add(ChoiceEvent.Info(
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
                每一天从8:00开始（界面右上角），行动会消耗一定的时间。在处理完所有事件前，你无法安稳地睡觉。
                尽管结束一天的时间是非常自由的，但是请尽量在23:00前睡觉。缺少睡眠可能会有一些不好的后果。
                
                点击下面的“OK”就可以完成这个事件。这一世，祝你好运！
                
                - SeanLenardB & quartrond
                """,
                context => context.Skill = 0.1));
        }
    }
}
