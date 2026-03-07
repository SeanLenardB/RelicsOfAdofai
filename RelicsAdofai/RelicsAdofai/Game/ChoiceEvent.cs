namespace RelicsAdofai.Game
{
    public class ChoiceEvent
    {
        public string Title = "";
        public string Description = "";
        public List<Choice> Choices = [];
        public int RemainingDays = int.MaxValue;
        public Action<GameContext> OvertimeConsequence = _ => { };
        public Action<GameContext> OnDayEnd = _ => { };

        public static ChoiceEvent Info(string title, string description, Action<GameContext> consequence)
        {
            return new()
            {
                Title = title,
                Description = description,
                Choices = [new() { Text = "OK", Consequence = consequence }]
            };
        }

        public static ChoiceEvent YesNo(string title, string description, 
            Action<GameContext> yesConsequence, Action<GameContext> noConsequence)
        {
            return new()
            {
                Title = title,
                Description = description,
                Choices = 
                [
                    new() { Text = "Yes", Consequence = yesConsequence },
                    new() { Text = "No", Consequence = noConsequence }
                ]
            };
        }

        public ChoiceEvent WithTimeLimit(int days, Action<GameContext> overtimeConsequence)
        {
            this.RemainingDays = days;
            this.OvertimeConsequence = overtimeConsequence;
            return this;
        }

        public ChoiceEvent WithOnDayEnd(Action<GameContext> onDayEnd)
        {
            this.OnDayEnd = onDayEnd;
            return this;
        }
    }

    public class Choice
    {
        public Predicate<GameContext> IsChoiceAvailable = _ => true;
        public string Text = "";
        public Action<GameContext> Consequence = _ => { };
    }
}
