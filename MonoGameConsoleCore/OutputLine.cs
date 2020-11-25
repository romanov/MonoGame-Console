namespace MonoGameConsole
{
    enum OutputLineType
    {
        Command,
        Output
    }

    class OutputLine
    {
        public string Output { get; set; }
        public OutputLineType Type { get; set; }

        public OutputLine(string output, OutputLineType type)
        {
            Output = output;
            Type = type;
        }

        public override string ToString()
        {
            return Output;
        }
    }
}
