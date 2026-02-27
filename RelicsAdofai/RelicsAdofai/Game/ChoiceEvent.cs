using System;
using System.Collections.Generic;
using System.Text;

namespace RelicsAdofai.Game
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
