# MonoGame Console

![monogameconsole-screenshot](http://s30.postimg.org/tqmwc9a8h/2013_12_27_21_11_11.png "Screenshot")

[MonoGame](http://monogame.net) is an open source implementation of the Microsoft XNA 4.x Framework. 

MonoGame Console - fork from [XNAGameConsole] (http://code.google.com/p/xnagameconsole/) with unicode & MonoGame support.

## Supported Platforms

* Windows (OpenGL, DirectX)

## Quick Start

* Include MonoGameConsole.dll to your project references
* Create XNB file with your SpriteFont or use default from repository "Content" folder
* Add code below to your Initialize() logic

            Services.AddService(typeof(SpriteBatch), spriteBatch);

            GameConsole console = new GameConsole(this, spriteBatch, new GameConsoleOptions
              {
                  ToggleKey = 192, 
                  Font = Content.Load<SpriteFont>("ConsoleFont"),
                  FontColor = Color.LawnGreen,
                  Prompt = "~>",
                  PromptColor = Color.Crimson,
                  CursorColor = Color.OrangeRed,
                  BackgroundColor = new Color(Color.Black, 150),
                  PastCommandOutputColor = Color.Aqua,
                  BufferColor = Color.Gold
              });

            console.AddCommand("ping", a =>
            {
                // TODO your logic
                return String.Format("pong");
            });

* Run game and press '~' (tilda) to open console


## License

MonoGame Console is released under GNU GPL v3.
