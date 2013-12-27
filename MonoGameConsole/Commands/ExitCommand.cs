using Microsoft.Xna.Framework;

namespace MonoGameConsole.Commands
{
    class ExitCommand : IConsoleCommand
    {
        public string Name
        {
            get
            {
                return "exit";
            }
        }
        public string Description
        {
            get
            {
                return "Forcefully exists the game";
            }
        }

        private readonly Game game;
        public ExitCommand(Game game)
        {
            this.game = game;
        }
        public string Execute(string[] arguments)
        {
            game.Exit();
            return "Exiting the game";
        }
    }
}