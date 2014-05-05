using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MonoGameConsole
{
    class InputProcessor
    {
        public event EventHandler Open = delegate { };
        public event EventHandler Close = delegate { };
        public event EventHandler PlayerCommand = delegate { };
        public event EventHandler ConsoleCommand = delegate { };

        public CommandHistory CommandHistory { get; set; }
        public OutputLine Buffer { get; set; }
        public List<OutputLine> Out { get; set; }

        private const int BACKSPACE = 8;
        private const int ENTER = 13;
        private const int TAB = 9;
        private bool isActive, isHandled;
        private CommandProcesser commandProcesser;



        public InputProcessor(CommandProcesser commandProcesser, GameWindow window)
        {
            this.commandProcesser = commandProcesser;
            isActive = false;
            CommandHistory = new CommandHistory();
            Out = new List<OutputLine>();
            Buffer = new OutputLine("", OutputLineType.Command);

            Form winGameWindow = GetGameWindow();
            winGameWindow.KeyPress += form_KeyPress;
            winGameWindow.KeyDown += EventInput_KeyDown;
        }

        /// <summary>
        /// Force application to find current app window controls with System.Windows.Forms (Windows)
        /// </summary>
        /// <returns></returns>
        private Form GetGameWindow()
        {
            FormCollection winFormsWindow = Application.OpenForms;

            return (Form)Control.FromHandle(winFormsWindow[0].Handle);

        }


        private void form_KeyPress(Object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            if (isHandled)
            {
                isHandled = false;
                return;
            }

            if (!isActive) return; // console is opened -> accept input

            CommandHistory.Reset();


            switch (e.KeyChar)
            {

                case (char)System.Windows.Forms.Keys.Return:
                    ExecuteBuffer();
                    break;

                case (char)System.Windows.Forms.Keys.Back:
                    if (Buffer.Output.Length > 0)
                    {
                        Buffer.Output = Buffer.Output.Substring(0, Buffer.Output.Length - 1);
                    }
                    break;

                case (char)System.Windows.Forms.Keys.Tab:
                    AutoComplete();
                    break;

                default:
                    if (IsPrintable(e.KeyChar))
                    {
                        Buffer.Output += e.KeyChar;
                    }
                    break;
            }
        }



        public void AddToBuffer(string text)
        {
            var lines = text.Split('\n').Where(line => line != "").ToArray();
            int i;
            for (i = 0; i < lines.Length - 1; i++)
            {
                var line = lines[i];
                Buffer.Output += line;
                ExecuteBuffer();
            }
            Buffer.Output += lines[i];
        }

        public void AddToOutput(string text)
        {
            if (GameConsoleOptions.Options.OpenOnWrite)
            {
                isActive = true;
                Open(this, EventArgs.Empty);
            }
            foreach (var line in text.Split('\n'))
            {
                Out.Add(new OutputLine(line, OutputLineType.Output));
            }
        }

        void EventInput_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.V) && Keyboard.GetState().IsKeyDown(Keys.LeftControl)) // CTRL + V
            {
                if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA) //Thread Apartment must be in Single-Threaded for the Clipboard to work
                {
                    AddToBuffer(Clipboard.GetText());
                }
            }

            if (e.KeyValue == GameConsoleOptions.Options.ToggleKey)
            {
                ToggleConsole();
                isHandled = true;
            }

            switch (e.KeyValue)
            {
                case 38: Buffer.Output = CommandHistory.Previous(); break;
                case 40: Buffer.Output = CommandHistory.Next(); break;
            }
        }

        void ToggleConsole()
        {
            isActive = !isActive;
            if (isActive)
            {
                Open(this, EventArgs.Empty);
            }
            else
            {
                Close(this, EventArgs.Empty);
            }
        }


        void ExecuteBuffer()
        {
            if (Buffer.Output.Length == 0)
            {
                return;
            }
            var output = commandProcesser.Process(Buffer.Output).Split('\n').Where(l => l != "");
            Out.Add(new OutputLine(Buffer.Output, OutputLineType.Command));
            foreach (var line in output)
            {
                Out.Add(new OutputLine(line, OutputLineType.Output));
            }
            CommandHistory.Add(Buffer.Output);
            Buffer.Output = "";
        }

        void AutoComplete()
        {
            var lastSpacePosition = Buffer.Output.LastIndexOf(' ');
            var textToMatch = lastSpacePosition < 0 ? Buffer.Output : Buffer.Output.Substring(lastSpacePosition + 1, Buffer.Output.Length - lastSpacePosition - 1);
            var match = GetMatchingCommand(textToMatch);
            if (match == null)
            {
                return;
            }
            var restOfTheCommand = match.Name.Substring(textToMatch.Length);
            Buffer.Output += restOfTheCommand + " ";
        }

        static IConsoleCommand GetMatchingCommand(string command)
        {
            var matchingCommands = GameConsoleOptions.Commands.Where(c => c.Name != null && c.Name.StartsWith(command));
            return matchingCommands.FirstOrDefault();
        }


        static bool IsPrintable(char letter)
        {
            return GameConsoleOptions.Options.Font.Characters.Contains(letter);
        }
    }
}
