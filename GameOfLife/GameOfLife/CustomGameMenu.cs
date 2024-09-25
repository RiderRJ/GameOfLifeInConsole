using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Linq;

namespace GameOfLife
{
    public class CustomGameMenu : Program //Мне не нравится что этот класс лишь чуть-чуть отличается от choice menu.
    {
        private static RuleConstructor ruleConstructor = new RuleConstructor();
        Button[] buttons = new Button[5];
        private string[] choices = { 
            "Максимальное расстояние до соседей", "Маска приема соседей", "Правило для мертвых клеток", 
            "Правило для живых клеток", "Процент заполняемости поля"
        };
        private RectangleShape split;
        private Action[] actions = new Action[] //надо убрать дублирование кода
        { () =>
            {
                string input = "";
                Window message = new("Изменение диапазона поиска клеток может применятся лишь один раз.",new WindowYoN());
                if(message.Denied) return;
                //DoSMTH
                ruleConstructor.neighsArgs = int.Parse(input);
                //post
            }, 
          () =>
            {
                Cell[][] input = null;
                Window message = new("Маска соседей может быть лишь одна",new WindowYoN());
                if(message.Denied) return;
                //DoSMTH
                ruleConstructor.neightWMaskArgs = input.ToArray();
                //post
            },
          () =>
            {
                short neighCount = 0;
                char sign = '>';
                //DoSMTH
                ruleConstructor.deadArgs.Add((neighCount,sign));
                //post
            },
          () =>
            {
                short neighCount = 0;
                char sign = '>';
                //DoSMTH
                ruleConstructor.liveArgs.Add((neighCount,sign));
                //post
            },
            () =>
            {
                short percent = 0;
                Window message = new("Заполненность поля клетками может быть лишь одна",new WindowYoN());
                if(message.Denied) return;
                //DoSMTH
                ruleConstructor.fillPercent = percent;
                //post
            }
        };
        public override void Init()
        {
            int padding = 10;
            split = new RectangleShape(new Vector2f(window.Size.X, 5));
            split.Position = new Vector2f(0, window.Size.Y / 2);
            split.FillColor = Color.White;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = Button.CreateButton(new RectangleShape(new Vector2f(240, 20))
                {
                    Position = new Vector2f(window.Size.X / 2 - 120, (window.Size.Y - padding) / 2 / choices.Length * i + padding)
                },
                actions[i], choices[i]);
                buttons[i].btnText.CharacterSize = 15;
            }
            gameController.X = 0;
            gameController.Y = 5;
        }
        public override void OnKeyPressed(KeyEventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
                buttons[gameController.Choice[0, 1]].OnClickEvent();
        }
        public override void Update()
        {
            for(int i = 0; i < choices.Length; i++)
            {
                if (gameController.Choice[0,1] == i)
                {
                    buttons[i].FillColor = Color.Green;
                    buttons[i].btnText.FillColor = Color.Black;
                }
                else
                {
                    buttons[i].FillColor = Color.Black;
                    buttons[i].btnText.FillColor = Color.White;
                }
                window.Draw(buttons[i] as IExtentedDrawable);
            }
            window.Draw(split);
        }
    }
}
