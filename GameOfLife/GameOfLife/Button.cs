using SFML.Graphics;
using System;

namespace GameOfLife
{
    public class Button : RectangleShape
    {
        //Добавить текст прям сюда
        public Action OnClickEvent;
        public static Button CreateButton(RectangleShape shape, Action action = null, string buttonText = "") => action switch
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
                OnClickEvent = action ?? delegate ()
                {
                    ExceptionMenu.Throw(new NotImplementedException("Функция кнопки не реализована"));
                }
            }
        };
        
            //if (shape == null) return null;
            //return new Button
            //{
            //    Position = shape.Position,
            //    FillColor = shape.FillColor,
            //    OutlineColor = shape.OutlineColor,
            //    OutlineThickness = shape.OutlineThickness,
            //    Rotation = shape.Rotation,
            //    Origin = shape.Origin,
            //    Scale = shape.Scale,
            //    Size = shape.Size,
            //    OnClickEvent = action ?? delegate ()
            //    {
            //        ExceptionMenu.Throw(new NotImplementedException("Функция кнопки не реализована"));
            //    }
            //};
        
        //Хз как добавить отрисовку текста вместе с кнопкой
    }
}
