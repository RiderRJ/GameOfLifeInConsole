using System;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameOfLife
{
    public abstract class Program
    {
        public static RenderWindow window;
        public static bool leave = false;
        public static int Frames { get; private set; } = 0;
        public static int Fps { get; private set; } = 0;
        public static Font font;
        public static Program instance = new ChoiceMenu();
        public static void Main(string[] args)
        {
            font = new Font("fonts/arial.ttf");
            CreateWindow();
            Text fps = new Text("", font)
            {
                Position = new Vector2f(0, 0),
                CharacterSize = 20,
                OutlineColor = Color.White,
                OutlineThickness = 5,
                FillColor = Color.Black,
            };
            _ = FPSReset();
            while(true)
            {
                instance.Init();
                leave = false;
                while(!leave)
                {
                    window.Clear(Color.Black);
                    window.DispatchEvents();
                    instance.Update();
                    Frames++;
                    fps.DisplayedString = Fps.ToString();
                    window.Draw(fps);
                    window.Display();
                }
            }
        }
        private static void CreateWindow()
        {
            window = new RenderWindow(new VideoMode(800, 600), "Game of life");
            window.Closed += (sender, e) => window.Close();
            window.KeyPressed += (sender, e) =>
            {
                if (e.Code == Keyboard.Key.Escape) { window.Close(); Environment.Exit(0); };
                instance.OnKeyPressed(e);
            };
        }
        private async static Task FPSReset()
        {
            while (true)
            {
                Fps = Frames;
                Frames = 0;
                await Task.Delay(1000);
            }
        }
        public abstract void OnKeyPressed(KeyEventArgs e);
        public abstract void Update();
        public abstract void Init();
    }
}
