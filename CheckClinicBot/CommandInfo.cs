namespace CheckClinic.Bot
{
    internal class CommandInfo
    {
        public Commands Command { get; set; }
        public int Index { get; }
        public string OriginalCommand { get; }

        public CommandInfo(string text)
        {
            OriginalCommand = text;
            switch (text)
            {
                case "+":
                    Command = Commands.Subscribe;
                    break;
                case "00":
                    Command = Commands.Refresh;
                    break;
                case "0":
                    Command = Commands.Back;
                    break;
                default:
                    int idx;
                    if (int.TryParse(text, out idx))
                    {
                        Command = Commands.SelectItem;
                        Index = idx;
                    }
                    break;
            }
        }
    }
}
