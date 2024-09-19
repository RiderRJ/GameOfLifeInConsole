using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class Window
    {
        public enum WindowType
        {
            Message,
            YoN,
        };
        private int choice = 0;
        private int Choice
        {
            get => choice;
            set
            {
                if (value >= (m_Type == WindowType.Message 
                    ? 1 : 2)) value = 0;
                if (value < 0) value = (m_Type == WindowType.Message
                    ? 1 : 2) - 1;
                choice = value;
            }
        }
        public static bool accepted;
        private WindowType m_Type;
        private string[] m_OkBtnVariants;
        private string m_Content;
        private string m_Title;
        public Window(string content,WindowType type, string[] btnVariants = null, string title = "Something happened!")
        {
            m_Content = content;
            m_Title = title;
            m_Type = type;
            m_OkBtnVariants = btnVariants ?? (new string[] { "Ok" });
            CreateWindow();
        }
        private Vector2f GetTextSize(Text text)
        {
            return new Vector2f(text.DisplayedString.Length*text.CharacterSize,text.CharacterSize);
        }
        private void CreateWindow()
        {
            float padding = 20f;
            float buttonHeight = 30f;
            float buttonSpacing = 10f;

            Text content = new Text(m_Content, ApplicationHolder.font)
            {
                CharacterSize = 15,
                FillColor = Color.White,
            };
            float contentSize = GetTextSize(content).X; 
            Text title = new Text(m_Title, ApplicationHolder.font)
            {
                CharacterSize = 20, 
            };

            Vector2f windowSize = new Vector2f(
                Math.Max(contentSize + padding * 2, 200), 
                content.CharacterSize + title.CharacterSize + buttonHeight + padding * 3 + 10 
            );

            RectangleShape window = new RectangleShape(windowSize)
            {
                Position = ((Vector2f)Program.window.Size / 2) - (windowSize / 2), 
                FillColor = Color.Black
            };
            title.Position = window.Position + new Vector2f(padding, padding);

            content.Position = title.Position + new Vector2f(0, title.CharacterSize + 10); 

            List<Text> btnContents = new List<Text>();
            List<RectangleShape> btns = new List<RectangleShape>();

            float buttonWidth = 80f; 

            for (int i = 0; i < (m_Type == WindowType.Message ? 1 : 2); i++)
            {
                btnContents.Add(new Text(m_Type == WindowType.Message ? m_OkBtnVariants[ApplicationHolder.rnd
                    .Next(0, m_OkBtnVariants.Length)] : (i == 0 ? "Yes" : "No"), ApplicationHolder.font)
                {
                    CharacterSize = 12 
                });
                float buttonPosX = window.Position.X + padding + i * (buttonWidth + buttonSpacing);
                float buttonPosY = window.Position.Y + windowSize.Y - buttonHeight - padding;

                btns.Add(new RectangleShape(new Vector2f(buttonWidth, buttonHeight))
                {
                    Position = new Vector2f(buttonPosX, buttonPosY),
                    FillColor = new Color(105,105,105),
                });

                btnContents.Last().Position = btns.Last().Position + new Vector2f(
                    (buttonWidth - btnContents.Last().GetLocalBounds().Width) / 2, 
                    (buttonHeight - btnContents.Last().CharacterSize) / 2 
                );
            }
            while (accepted is false)
            {
                Program.window.DispatchEvents();
                Program.window.Clear(new Color(25,25,25));
                btns[Choice].FillColor = new Color(55,255,55);
                btnContents[Choice].FillColor = new Color(55,55,55);
                Program.window.Draw(window);
                Program.window.Draw(title);
                Program.window.Draw(content);
                btns.ForEach(btn =>
                {
                    Program.window.Draw(btn);
                });
                btnContents.ForEach(cont =>
                {
                    Program.window.Draw(cont);
                });
                Program.window.Display();
            }
        }
    }
}
