using RelicsConsole.Game;

public class Program
{
    // @cleanup: this should not be a hardcoded data
    public List<Chart> Charts = 
    [
        new() { Artist = "Sean Lenard B. vs Martix Lenard", Song = "QR2", RequiredSkill = 1.14 },
        new() { Artist = "Sean Lenard B.", Song = "The Final Descent of Quartrond", RequiredSkill = 5.14 },
        new() { Artist = "Sean Lenard B.", Song = "Valley of Aer", RequiredSkill = 1.919 },
    ];

    public static void Main()
    {
        GameContext context = new()
        {
            PlayerName = "quartrond",
        };

        bool shouldContinue = true;
        while (shouldContinue)
        {
            context.GenerateChoiceEvents();
            foreach (var choiceEvent in context.ChoiceEvents)
            {
                choiceEvent.AskPlayer();
            }
        }
    }
}