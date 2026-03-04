namespace RelicsAdofai.Game
{
    public class ChoiceEvent
    {
        public string Description = "";
        public List<Choice> Choices = [];
    }

    public class Choice
    {
        public Predicate<GameContext> IsChoiceAvailable = _ => true;
        public string Text = "";
        public Action<GameContext> Consequence = _ => { };
    }
}
