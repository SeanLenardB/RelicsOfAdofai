using System.Diagnostics;

namespace RelicsAdofai.Game
{
    public partial class GameContext
    {
        // The format of the save file is as follows:
        /* 
         * <PlayerName>
         * <Seed>
         * <Action type> <Argument>
         * <Action type> <Argument>
         * <Action type> <Argument>
         * ...
         * 
         */

        // Does not include the first two lines.
        public List<string> SaveLines = [];
        public void LoadPersistency(List<string> saveFileLines)
        {
            Debug.Assert(saveFileLines.Count >= 2, "Invalid save file!");
            this.PlayerName = saveFileLines[0];
            // @note: the seed has already been set once this class is instantiated. No need to load.
            
            for (int i = 2; i < saveFileLines.Count; i++)
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(saveFileLines[i]), "There is an empty line in the save file!");
                string[] lineSplit = saveFileLines[i].Replace("\n", "").Replace("\r", "").Split(' ');  // fuck carriage returns
                switch (lineSplit[0]) 
                {
                    case "e":  // ChoiceEvent
                        int eventIndex = int.Parse(lineSplit[1]);
                        int choiceIndex = int.Parse(lineSplit[2]);
                        var targetEvent = this.ChoiceEvents[eventIndex];
                        Debug.Assert(targetEvent.Choices[choiceIndex].IsChoiceAvailable(this), "Trying to take an unavailable choice!");
                        this.TakeChoice(targetEvent, targetEvent.Choices[choiceIndex]);
                        break;
                    case "c":  // Chart attempt & practice
                        int chartIndex = int.Parse(lineSplit[1]);
                        bool isAttempt = lineSplit[2] == "a";
                        if (isAttempt) this.AttemptChart(this.Charts[chartIndex]);
                        else this.PracticeChart(this.Charts[chartIndex]);  // @hack: now we can only attempt or practice
                        break;
                    case "d":  // Finish a day
                        this.FinishDay();
                        break;
                    default: continue;
                }
            }
        }

        public string SavePersistency()
        {
            File.WriteAllLines($"save.{this.Seed.ToString().Replace('-', 'n')}.txt", [this.PlayerName, this.Seed.ToString()]);
            File.AppendAllLines($"save.{this.Seed.ToString().Replace('-', 'n')}.txt", this.SaveLines);

            return $"save.{this.Seed.ToString().Replace('-', 'n')}.txt";
        }
    }
}
