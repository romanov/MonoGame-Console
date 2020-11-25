using System;
using Microsoft.Xna.Framework.Input;

namespace MonoGameConsole.KeyboardCapture
{
    class KeyEventArgs : EventArgs
    {
        public KeyEventArgs( Keys keyCode )
        {
            KeyCode = keyCode;
        }

        public Keys KeyCode { get; private set; }
    }
    
}
