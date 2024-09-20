using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace GameOfLife
{
    public sealed class ChoiceMenu : Program
    {
        string[] choices = { "Рандомная генерация поля", "Режим песочницы" , "Кастомный режим"};
        Program[] scenes = new Program[] { new RandomField(35), new Sandbox(), new CustomGameMenu()};
        List<Button> buttons = new List<Button>();
        List<Text> buttonTexts = new List<Text>();
        public override void Init()
        {
            gameController.X = 0;
            gameController.Y = (ushort)choices.Length;
            InitObjects();
        }
        private void InitObjects()
        {
            for (int i = 0; i < choices.Length; i++)
            {
                InitButtons(i);
                InitText(i);
            }
        }
        private void InitButtons(int i)
        {
            buttons.Add(Button.CreateButton(new RectangleShape(new Vector2f(200f, 40f))
            {
                Position = new Vector2f(0 + window.Size.X / 2 - 100f, i * 40f / 2 + window.Size.X / 2 - 40f)
            }, delegate () {
                instance = scenes[i];
            }));
        }
        private void InitText(int i)
        {
            buttonTexts.Add(new Text(choices[i], font)
            {
                CharacterSize = 10,
                Position = buttons[i].Position,
            });
        }
        public override void Update()
        {
            Draw();
        }
        private void Draw()
        {
            for (int i = 0; i < choices.Length; i++)
            {
                buttons[i].FillColor = new Color(25, 25, 25);
                buttonTexts[i].FillColor = Color.White;
                if (i == gameController.Choice[0, 1])
                {
                    buttons[i].FillColor = Color.Green;
                    buttonTexts[i].FillColor = Color.Black;
                }
                window.Draw(buttons[i]);
                window.Draw(buttonTexts[i]);
            }
        }
        public override void OnKeyPressed(KeyEventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
            {
                buttons[gameController.Choice[0, 1]].OnClickEvent();
                leave = true;
            }
        }
    }
}
