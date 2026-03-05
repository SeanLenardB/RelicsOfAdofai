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

            this.ArtistsAndSongs.Add(new("Fnares", "NAND NAND NAND"));
            this.ArtistsAndSongs.Add(new("Fnares", "mono_leg"));
            this.ArtistsAndSongs.Add(new("Fnares", "RADIO, UMBRALOGOS"));
            this.ArtistsAndSongs.Add(new("Fnares", "Photographed As Purple And Pristine"));
            this.ArtistsAndSongs.Add(new("Fnares", "They Desire To Walk"));

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
                    for (int k = j + 1; k < singleCreatorCount; k++)
                    {
                        this.Creators.Add($"{this.Creators[i]} & {this.Creators[j]} & {this.Creators[k]}");
                    }
                }
            }

            
        }
    }
}
