namespace RelicsAdofai.Game.Events
{
    public class ChoiceEvent
    {
        public string Description = "";
        public List<Choice> Choices = [];

        public void AskPlayer()
        {

        }
    }

    public class Choice
    {
        public string Text = "";
        public Action Action = () => { };
    }
}
