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

        public bool IsMainQuest = false;

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
        public static ChoiceEvent YesNo(string title, string description, 
            Action<GameContext> yesConsequence, Action<GameContext> noConsequence, Predicate<GameContext> yesCriteria)
        {
            return new()
            {
                Title = title,
                Description = description,
                Choices = 
                [
                    new() { Text = "Yes", Consequence = yesConsequence, IsChoiceAvailable = yesCriteria },
                    new() { Text = "No", Consequence = noConsequence }
                ]
            };
        }

        public static ChoiceEvent MeetCriteria(int days, string title, string description,
            Action<GameContext> succeedConsequence, Action<GameContext> failConsequence,
            Predicate<GameContext> criteria)
        {
            return new ChoiceEvent()
            {
                Title = title,
                Description = description,
                Choices = 
                [
                    new() { Text = "完成", Consequence = succeedConsequence, IsChoiceAvailable = criteria },
                    new() { Text = "放弃", Consequence = failConsequence }
                ]
            }.WithDayLimit(days, failConsequence);
        }

        public ChoiceEvent WithDayLimit(int days, Action<GameContext> overtimeConsequence)
        {
            this.RemainingDays = days;
            this.OvertimeConsequence = overtimeConsequence;
            return this;
        }

        public ChoiceEvent WithOnDayEnd(Action<GameContext, ChoiceEvent> onDayEnd)
        {
            // @hack: the reason this exists is that we need to have a way to
            // remove a choice event on day end. This is a very dirty way to do it
            // but, hey! it works!
            this.OnDayEnd = context => onDayEnd(context, this);
            return this;
        }
        public ChoiceEvent WithOnDayEnd(Action<GameContext> onDayEnd) => this.WithOnDayEnd((context, _) => onDayEnd(context));

        public ChoiceEvent MainQuest() { this.IsMainQuest = true; return this; }
    }

    public class Choice
    {
        public Predicate<GameContext> IsChoiceAvailable = _ => true;
        public string Text = "";
        public Action<GameContext> Consequence = _ => { };
    }
}
