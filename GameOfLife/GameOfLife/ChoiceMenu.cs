using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Security.Principal;

namespace GameOfLife
{
    public sealed class ChoiceMenu : Program
    {
        string[] choices = { "Рандомная генерация поля", "Режим песочницы" , "Кастомный режим"};
        Program[] scenes = new Program[] { new RandomField(35), new Sandbox(), new CustomGameMenu()};
        public override void Init()
        {
            gameController.X = 0;
            gameController.Y = (ushort)choices.Length;
        }
        public override void Update()
        {
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
                if (i == gameController.Choice[0,1])
                {
                    button.FillColor = Color.Green;
                    btnText.FillColor = Color.Black;
                }
                window.Draw(button); // пометить Cell как Drawable??? 
                window.Draw(btnText);
            }
        }
        public override void OnKeyPressed(KeyEventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
            {
                instance = scenes[gameController.Choice[0,1]];
                leave = true;
            }
        }
    }
}
