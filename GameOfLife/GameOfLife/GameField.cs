using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Linq;
using System.Threading.Tasks;
using static GameOfLife.Cell;

namespace GameOfLife
{
    public abstract class GameField : Program
    {
        protected static short height = 100; //300:300 = 5fps
        protected static short width = 100;
        public static char[,] Map { get; protected set; } = new char[width, height];
        protected bool _resumed = true;
        protected RectangleShape screenDot = new RectangleShape();
        protected Text debugNum = new Text();
        RuleConstructor Rules { get; set; } //
        public void CellsLife()
        {
            if (_resumed)
            {
                foreach (var cell in Thinker.thinkers)
                    cell.Think();
                foreach (var cell in Thinker.thinkers)
                    cell.NextTurn();
            }
        }
        public override void Init()
        {
            rule = delegate (short neighbours, short alive, Cell itself)
            {
                if (alive == 1)
                    if (neighbours < 2 || neighbours > 3)
                    {
                        return 0;
                    }
                if (alive == 0)
                    if (neighbours == 3)
                    {
                        return 1;
                    }
                return alive;
            };
            gameController.X = (ushort)width;
            gameController.Y = (ushort)height;
            CreateCellField(width, height);
        }
        protected void Draw()
        {
            float cWidth = window.Size.X * (1f / Map.GetLength(0)) * 1.25f;
            float cHeight = window.Size.Y * (1f / Map.GetLength(1)) * 1.75f;
            screenDot.OutlineColor = new Color(120, 120, 120);
            screenDot.OutlineThickness = 1f;
            for (short i = 0; i < Map.GetLength(0); i++)
            {
                for (short j = 0; j < Map.GetLength(1); j++)
                {
                    screenDot.Position = new Vector2f(i * cWidth / 2, j * cHeight / 2);
                    screenDot.Size = new Vector2f(cWidth, cHeight);
                    screenDot.FillColor = cells[i][j].Alive == 1 ? Color.White : new Color(25, 25, 25);
                    if (!_resumed && gameController.Choice[0, 0] == i && gameController.Choice[0, 1] == j)
                    {
                        screenDot.FillColor = Color.Green;
                    }
                    window.Draw(screenDot);
                }
            }
        }
        public override void OnKeyPressed(KeyEventArgs e)
        {
            if (e.Code == (Keyboard.Key.Enter))
            {
                Cell cellRef = cells[gameController.Choice[0, 0]][gameController.Choice[0, 1]];
                cellRef.Alive = cellRef.Alive == 1 ? (short)0 : (short)1;
                cellRef.onChangeState(cellRef, (cellRef.Alive == 0) ? (short)-1 : (short)1);
            }
            if (e.Code == (Keyboard.Key.Space))
            {
                _resumed = !_resumed;
            }
        }
    }
}
