using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using static SFML.Window.Mouse;

namespace GameOfLife
{
    public class Window
    {
        public enum WindowType
        {
            Message,
            YoN,
        };
        public static bool accepted;
        private WindowType m_Type;
        private string[] m_OkBtnVariants;
        private string m_Content;
        private string m_Title;
        private List<Text> btnContents = new List<Text>();
        private List<Button> btns = new List<Button>();//

        Controller global_Controller;
        public Window(string content,WindowType type, string[] btnVariants = null, string title = "Something happened!")
        {
            m_Content = content;
            m_Title = title;
            m_Type = type;
            m_OkBtnVariants = btnVariants ?? (new string[] { "Ok" });
            global_Controller = ApplicationHolder.gameController;
            global_Controller.X = type == WindowType.Message ? (ushort)1 : (ushort)2;
            global_Controller.Y = 0;
            CreateWindow();
        }
        private Vector2f GetTextSize(Text text) =>
            new Vector2f(text.DisplayedString.Length * text.CharacterSize, text.CharacterSize);
        
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
                btns.Add(Button.CreateButton(new RectangleShape(new Vector2f(buttonWidth, buttonHeight))
                {
                    Position = new Vector2f(buttonPosX, buttonPosY),
                    FillColor = new Color(105,105,105),
                }));
                btnContents.Last().Position = btns.Last().Position + new Vector2f(
                    (buttonWidth - btnContents.Last().GetLocalBounds().Width) / 2, 
                    (buttonHeight - btnContents.Last().CharacterSize) / 2 
                );
            }
            while (accepted is false)
            {
                Program.window.DispatchEvents();
                Program.window.Clear(new Color(25,25,25));
                btns[global_Controller.Choice[0,1]].FillColor = new Color(55,255,55);
                btnContents[global_Controller.Choice[0, 1]].FillColor = new Color(55,55,55);
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