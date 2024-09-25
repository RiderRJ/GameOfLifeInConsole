using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class Window
    {
        public bool Accepted { get; private set; }
        public bool Denied { get; private set; }
        private WindowType m_Type;
        private string m_Content;
        private string m_Title;
        private List<Button> btns = new List<Button>();
        private RectangleShape window;
        private Text title;
        private Text content;
        Controller global_Controller; //Кажется необходимо создать локальный контроллер для окна сообщения
        public Window(string content, WindowType type, string title = "Something happened!")
        {
            m_Content = content;
            m_Title = title;
            m_Type = type;
            global_Controller = ApplicationHolder.gameController;
            global_Controller.X = type.buttonsNum;
            global_Controller.Y = 0;
            CreateWindow();
        }
        private Vector2f GetTextSize(Text text) =>
            new Vector2f(text.DisplayedString.Length * text.CharacterSize, text.CharacterSize);

        private void CreateWindow()
        {
            InitObjects();
            while (!Accepted || !Denied)
            {
                DrawWindow();
            }
        }
        private void InitObjects() //код не читаемый
        {
            float padding = 20f;
            float buttonHeight = 30f;
            float buttonSpacing = 10f;
            content = new Text(m_Content, ApplicationHolder.font)
            {
                CharacterSize = 15,
                FillColor = Color.White,
            };
            float contentSize = GetTextSize(content).X;
            title = new Text(m_Title, ApplicationHolder.font)
            {
                CharacterSize = 20,
            };
            Vector2f windowSize = new Vector2f(
                Math.Max(contentSize + padding * 2, 200),
                content.CharacterSize + title.CharacterSize + buttonHeight + padding * 3 + 10
            );
            window = new RectangleShape(windowSize)
            {
                Position = ((Vector2f)Program.window.Size / 2) - (windowSize / 2),
                FillColor = Color.Black
            };
            title.Position = window.Position + new Vector2f(padding, padding);
            content.Position = title.Position + new Vector2f(0, title.CharacterSize + 10);
            float buttonWidth = 80f;
            for (int i = 0; i < m_Type.buttonsNum; i++)
            {
                float buttonPosX = window.Position.X + padding + i * (buttonWidth + buttonSpacing);
                float buttonPosY = window.Position.Y + windowSize.Y - buttonHeight - padding;
                btns.Add(Button.CreateButton(new RectangleShape(new Vector2f(buttonWidth, buttonHeight))
                {
                    Position = new Vector2f(buttonPosX, buttonPosY),
                    FillColor = new Color(105, 105, 105),
                },
                (i == 0) ? () => { Accepted = true; } : () => { Denied = true; }, //Заменить
                m_Type.GetText()));
                btns.Last().btnText.Position = btns.Last().Position + new Vector2f(
                    (buttonWidth - btns.Last().btnText.GetLocalBounds().Width) / 2,
                    (buttonHeight - btns.Last().btnText.CharacterSize) / 2
                );
                btns.Last().btnText.CharacterSize = 12;
            }
        }
        private void DrawWindow()
        {
            Program.window.DispatchEvents();
            Program.window.Clear(new Color(25, 25, 25));
            btns[global_Controller.Choice[0, 1]].FillColor = new Color(55, 255, 55);
            btns[global_Controller.Choice[0, 1]].btnText.FillColor = new Color(55, 55, 55);
            Program.window.Draw(window);
            Program.window.Draw(title);
            Program.window.Draw(content);
            btns.ForEach(btn =>
            {
                Program.window.Draw(btn as IExtentedDrawable);
            });
            Program.window.Display();
        }
    }
}