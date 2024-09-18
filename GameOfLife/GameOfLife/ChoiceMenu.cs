using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameOfLife
{
    public class ChoiceMenu : Program
    {
        string[] choices = { "Рандомная генерация поля", "Режим песочницы" };
        Program[] scenes = new Program[] { new RandomField(), new Sandbox()};
        private int choice = 1;
        private int Choice
        {
            get => choice;
            set
            {
                if (value >= choices.Length) value = 0;
                if (value < 0) value = choices.Length - 1;
                choice = value;
            }
        }
        public override void Init()
        {

        }
        public override void Update()
        {
            Init();
            Draw();
        }
        private void Draw()
        {
            for (int i = 0; i < choices.Length; i++)
            {
                RectangleShape button = new RectangleShape(new Vector2f(200f, 40f))
                {
                    FillColor = Color.Black,
                    Position = new Vector2f(0 + window.Size.X / 2 - 100f, i * 40f / 2 + window.Size.X / 2 - 40f)
                };
                Text btnText = new Text(choices[i], font)
                {
                    CharacterSize = 10,
                    Position = button.Position,
                    FillColor = Color.White
                };
                if (i == Choice)
                {
                    button.FillColor = Color.Green;
                    btnText.FillColor = Color.Black;
                }
                window.Draw(button); // пометить Cell как Drawable!!!! 
                window.Draw(btnText);
            }
        }
        public override void OnKeyPressed(KeyEventArgs e)
        {
            if (e.Code == (Keyboard.Key.Up))
                Choice--;
            if (e.Code == (Keyboard.Key.Down))
                Choice++;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
            {
                instance = scenes[Choice];
                leave = true;
            }
        }
    }
}
