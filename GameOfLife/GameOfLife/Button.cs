using SFML.Graphics;
using System;

namespace GameOfLife
{
    public class Button : RectangleShape, IExtentedDrawable
    {
        public Action OnClickEvent;
        public Text btnText;
        public static Button CreateButton(RectangleShape shape, Action action = null, string buttonText = "") => shape switch
        {
            null => null,
            _ => new Button
            {
                Position = shape.Position,
                FillColor = shape.FillColor,
                OutlineColor = shape.OutlineColor,
                OutlineThickness = shape.OutlineThickness,
                Rotation = shape.Rotation,
                Origin = shape.Origin,
                Scale = shape.Scale,
                Size = shape.Size,
                btnText = new Text(buttonText, ApplicationHolder.font)
                {
                    CharacterSize = 10,
                    FillColor = Color.Black,
                    Position = shape.Position,
                },
                OnClickEvent = action ?? delegate ()
                {
                    ExceptionMenu.Throw(new NotImplementedException("Функция кнопки не реализована"));
                }
            }
        };
        public void Draw(RenderWindow window)
        {
            window.Draw(this);
            window.Draw(btnText);
        }
    }
}
