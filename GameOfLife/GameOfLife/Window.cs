using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

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
        public Window(string content,WindowType type, string title = "Oops!", string[] btnVariants = null)
        {
            m_Content = content;
            m_Title = title;
            m_Type = type;
            m_OkBtnVariants = btnVariants;
            CreateWindow();
        }
        private void CreateWindow()
        {
            Text content = new Text(m_Content, ApplicationHolder.font)
            {
                CharacterSize = 15,
            };
            float contentSize = content.DisplayedString.Length * content.CharacterSize;
            Text title = new Text(m_Title, ApplicationHolder.font)
            {
                CharacterSize = 10,
            };
            Vector2f windowSize = new Vector2f(contentSize+ 10, content.CharacterSize + 100);//одностроковый вывод
            List<Text> btnContents = new List<Text>();
            List<RectangleShape> btns = new List<RectangleShape>();//Создать класс Button, который будет хранить событие на нажатии по кнопке.
            RectangleShape window = new RectangleShape(windowSize)
            {
                Position = ((Vector2f)Program.window.Size / 2)- windowSize/2,
                FillColor = Color.Black
            };
            content.Position = window.Position + window.Size / 2 - new Vector2f(contentSize, 35) / 2;
            title.Position = content.Position - new Vector2f(0, content.CharacterSize);
            for (int i = 0; i < (m_Type == WindowType.Message ? 1 : 2); i++)
            {
                btnContents.Add(new Text(m_Type == WindowType.Message? m_OkBtnVariants[ApplicationHolder.rnd
                    .Next(0, m_OkBtnVariants.Length - 1)] : (i == 0 ? "Yes" : "No"), ApplicationHolder.font)
                {
                    Position = window.Position + new Vector2f(0, windowSize.Y)
                });
                float scaleX = btnContents.Last().DisplayedString.Length *
                    btnContents.Last().CharacterSize;
                float scaleY = btnContents.Last().CharacterSize;
                btns.Add(new RectangleShape(new Vector2f(scaleX, scaleY))
                {
                    FillColor = Color.White,
                });
                btns.Last().Position = btnContents.Last().Position;
            }
            while (accepted is false)
            {
                Program.window.DispatchEvents();
                Program.window.Clear(Color.Cyan);
                btns[Choice].FillColor = Color.Green;
                btnContents[Choice].FillColor = Color.White;
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
