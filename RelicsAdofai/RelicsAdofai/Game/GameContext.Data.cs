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

            this.ArtistsAndSongs.Add(new("7bug", "A Dance of Ice and Fire"));
            this.ArtistsAndSongs.Add(new("7bug", "Onbeats"));
            this.ArtistsAndSongs.Add(new("7bug", "Onbeats but it's too long"));
            this.ArtistsAndSongs.Add(new("7bug", "Thank you for being tortured"));
            this.ArtistsAndSongs.Add(new("7bug", "The Midnoon Airplane"));
            this.ArtistsAndSongs.Add(new("7bug", "Rotate 2 Lose"));



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



            this.OtherPlayers.Add("Reppij");
            this.OtherPlayers.Add("Teaj_");
            this.OtherPlayers.Add("Vate_Jalery");
            this.OtherPlayers.Add("Dodging ball");
            this.OtherPlayers.Add("FireCave");
            this.OtherPlayers.Add("ikun_");
            this.OtherPlayers.Add("HuiZai_");
            this.OtherPlayers.Add("Ling_Centrifuge");
            this.OtherPlayers.Add("A2ra_");
            this.OtherPlayers.Add("listenwind");
            this.OtherPlayers.Add("Firepika");
            this.OtherPlayers.Add("afgiago'");
            this.OtherPlayers.Add("Iris");
            foreach (var player in this.OtherPlayers)
            {
                this.FriendlinessWithOtherPlayers.Add(player, 0);

                this.Unlocked_OtherPlayer_Sc.Add(player, false);
                this.Unlocked_OtherPlayer_Recreation.Add(player, false);
                this.Unlocked_OtherPlayer_BigSc.Add(player, false);
            }



            // More events are unlocked through progression. They are located in GameContext.Progression.
            this.ChoiceEventWeights.Add(new(2, () =>
            {
                string player = this.OtherPlayers[this.Random.Next(this.OtherPlayers.Count)];
                double livestreamTime = this.Random.NextDouble() * 4;
                return ChoiceEvent.YesNo(
                    $"看{player}直播",
                    $"{player}开播了，是否要看一会儿？",
                    context =>
                    {
                        context.Hour += livestreamTime;
                        context.Sanity += livestreamTime / 80;
                        this.FriendlinessWithOtherPlayers[player] += livestreamTime / 2;
                    },
                    _ => { }
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                return ChoiceEvent.YesNo(
                    $"打一会儿Gtg",
                    $"是否要打4小时Gtg休息一下？",
                    context =>
                    {
                        for (int i = 0; i < 20; i++)  // @note: the 20 here is derived from 4h / 0.2h/chart.
                            context.AttemptChart(context.Charts[context.Random.Next(context.Charts.Count)], true);
                        context.Skill *= 1.02;

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
                    $"是否要打2小时Gtg休息一下？",
                    context =>
                    {
                        for (int i = 0; i < 10; i++)  // @note: ditto.
                            context.AttemptChart(context.Charts[context.Random.Next(context.Charts.Count)], true);
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
                            context.AttemptChart(context.Charts[context.Random.Next(context.Charts.Count)], true);
                        context.Skill *= 1.003;

                        if (context.Random.NextDouble() > 0.5) return;
                        context.FollowerCount *= 1.01;
                    },
                    _ => { }
                    ).WithDayLimit(1, _ => { });
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                double money = (this.Random.NextDouble() * 300) + 200;
                return ChoiceEvent.YesNo(
                    $"升级键盘",
                    $"有一些Adofai玩家换了一种新的键盘，看上去还挺顺手，要不要也买一把？消耗资金<span class=\"color-money\">{money:N2}</span>",
                    context =>
                    {
                        context.Money -= money;

                        if (context.Random.NextDouble() > 0.3) return;
                        context.Skill *= 1.05;

                        if (context.Random.NextDouble() > 0.2) return;
                        context.FollowerCount *= 1.02;
                    },
                    _ => { },
                    context => context.Money > money
                    );
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                double money = (this.Random.NextDouble() * 600) + 400;
                return ChoiceEvent.YesNo(
                    $"升级键盘",
                    $"有一些Adofai玩家换了一种新的键盘，看上去还挺顺手，要不要也买一把？消耗资金<span class=\"color-money\">{money:N2}</span>",
                    context => { context.Money -= money; context.Skill *= 1.05; },
                    _ => { },
                    context => context.Money > money
                    );
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                double money = (this.Random.NextDouble() * 200) + 100;
                return ChoiceEvent.YesNo(
                    $"升级摄像头",
                    $"你的摄像头有一点旧了，要不要升级一下摄像头，增加机位或是提高画质？消耗资金<span class=\"color-money\">{money:N2}</span>",
                    context =>
                    {
                        context.Money -= money;
                        if (context.Random.NextDouble() > 0.5) return;
                        context.FollowerCount *= 1.03;
                    },
                    _ => { },
                    context => context.Money > money
                    );
            }));
            this.ChoiceEventWeights.Add(new(1, () =>
            {
                double money = (this.Random.NextDouble() * 400) + 400;
                return ChoiceEvent.YesNo(
                    $"升级摄像头",
                    $"你的摄像头有一点旧了，要不要升级一下摄像头，增加机位或是提高画质？消耗资金<span class=\"color-money\">{money:N2}</span>",
                    context =>
                    {
                        context.Money -= money;
                        context.FollowerCount *= 1.05;
                    },
                    _ => { },
                    context => context.Money > money
                    );
            }));
        }

        public void ImportStartupEvents()
        {
            this.ChoiceEvents.Add(
                ChoiceEvent.Info(
                    $"欢迎来到Relics of Adofai",
                    """
                    <p>欢迎来到Relics of Adofai。这条信息是你的第一个教程。

                    Relics of Adofai是一个基于决策的游戏。
                    界面左上角有你的基本属性。每一种属性数值都有独特的颜色，方便你在其他地方快速分辨。
                    <span class="color-skill">技术</span>是你作为Adofai玩家最重要的数值。你的“技术”点数越高，谱面通过的概率就越高。所有谱面都在界面左侧紫色框内。
                    <span class="color-follower">名声</span>会影响你在社区中的地位。粉丝多也许可以带来更多和其他玩家合作的机会，但也可能带来更困难的决策！
                    <span class="color-money">资金</span>可以通过发布谱面击破视频等活动获得，请保证充足的资金，否则你可能会在关键关头少一些可能的选择。
                    <span class="color-sanity">理智</span>会影响你的打谱状态。很多行为会对你的理智有影响，注意保持良好的精神状态。

                    每一天你都会遇到一些随机事件。根据你前世的经验，以及这一世的情况，做出你的选择！
                    每一个选择都会有不同的结果。

                    在左侧紫色谱面区域内，你可以自由选择谱面练习段落或挑战击破。练谱和打谱都需要时间。
                    每一天从8:00开始（界面右上角），行动会消耗一定的时间。
                    有一些事件有时效性，这些事件的右上角会显示剩余天数（例如本事件）。
                    请注意时间，在23:00后将强制结束一天。
                    
                    你有一个选择：如果现在完成这个事件，你获得的资金会更少。
                    或者，你可以今天不处理这个事件。到第二天时，这个事件会自动结束，你获得的资金会更多。
                    点击下面的“OK”就可以完成这个事件，或者不要管这个事件。
                    
                    这一世，祝你好运！</p>
                    """,
                    context =>
                    {
                        context.Skill += 1;
                        context.Money += 11.4;
                        context.FollowerCount += 1;
                        context.Sanity = 1;
                        context.Hour += 6;
                    })
                .WithDayLimit(
                    0,
                    context =>
                    {
                        context.Skill += 1;
                        context.Money += 72.7;
                        context.FollowerCount += 1;
                        context.Sanity = 1;
                    }
                )
                .MainQuest());
        }
    }
}
