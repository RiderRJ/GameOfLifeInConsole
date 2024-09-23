using SFML.Window;
using System;

namespace GameOfLife
{
    public class CustomGameMenu : Program //Мне не нравится что этот класс лишь чуть-чуть отличается от choice menu.
    {
        string[] choices = { 
            "Максимальное расстояние до соседей", "Маска приема соседей", "Правило для мертвых клеток", 
            "Правило для живых клеток", "Процент заполняемости поля"
        };
        object[] links = new object[] { };
        public override void Init()
        {
            ExceptionMenu.Throw(new NotImplementedException());
        }

        public override void OnKeyPressed(KeyEventArgs e)
        {
            ExceptionMenu.Throw(new NotImplementedException());
        }

        public override void Update()
        {
            ExceptionMenu.Throw(new NotImplementedException());
        }
    }
}
