namespace MonoGameConsole.Commands
{
    class ClearScreenCommand:IConsoleCommand
    {
        public string Name { get
        {
            return "clear";
        } }
        public string Description { get
        {
            return "Clears the console output";
        } }

        private InputProcessor processor;
        public ClearScreenCommand(InputProcessor processor)
        {
            this.processor = processor;
        }
        public string Execute(string[] arguments)
        {
            processor.Out.Clear();
            return "";
        }
    }
}
