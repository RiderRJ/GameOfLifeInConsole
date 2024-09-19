using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Linq;
using static GameOfLife.Cell;

namespace GameOfLife
{
    public abstract class GameField : Program
    {
        protected static short height = 20;
        protected static short width = 20;
        public static char[,] Map { get; protected set; } = new char[width, height];
        protected bool _resumed = true;
        protected RectangleShape[,] screenDot = new RectangleShape[width,height];
        protected Text[,] debugNum = new Text[width,height];
        RuleConstructor Rules { get; set; } //
        public GameField()
        {
            for(int i = 0; i < screenDot.GetLength(0); i++)
                for (int j = 0; j < screenDot.GetLength(1); j++)
                {
                    screenDot[i, j] = new RectangleShape();
                    debugNum[i, j] = new Text();
                }
        }
        public void CellsLife()
        {
            if (_resumed)
            {
                nMLivingCells = livingCells.ToList();
                foreach (var cell in Thinker.thinkers)
                    cell.Think();
                foreach (var cell in Thinker.thinkers)
                    cell.NextTurn();
            }
            UpdateMap();
        }
        public override void Init()
        {
            rule = delegate (short neighbours, short alive, Cell itself)
            {
                if (alive == 1)
                    if (neighbours < 2 || neighbours > 3)
                    {
                        //nMLivingCells.Remove(itself);
                        return 0;
                    }
                if (alive == 0)
                    if (neighbours == 3)
                    {
                        //nMLivingCells.Add(itself);
                        return 1;
                    }
                return alive;
            };
            gameController.X = (ushort)width;
            gameController.Y = (ushort)height;
            CreateCellField(width, height);
        }
        protected static void ReadMap()
        {
            for (short i = 0; i < Map.GetLength(0); i++)
                for (short k = 0; k < Map.GetLength(1); k++)
                {
                    if (Map[i, k] == '#')
                        cells[i][k].Alive = 1;
                    //new Cell(i, k, 1);
                    else
                        cells[i][k].Alive = 0;
                        //new Cell(i, k, 0);
                }
            //foreach (var cell in cells)
            //    Ready();
        }
        protected static void UpdateMap()
        {
            for (short i = 0; i < Map.GetLength(0); i++)
                for (short k = 0; k < Map.GetLength(1); k++)
                {
                    if (cells[i][k].Alive == 1)
                        Map[i, k] = '#';
                    else Map[i, k] = '-';
                }
        }
        protected void Draw()
        {
            for (short i = 0; i < Map.GetLength(0); i++)
            {
                for (short j = 0; j < Map.GetLength(1); j++)
                {
                    float cWidth = window.Size.X * (1f / Map.GetLength(0)) * 1.25f;
                    float cHeight = window.Size.Y * (1f / Map.GetLength(1)) * 1.75f;
                    screenDot[i, j].FillColor = new Color(25, 25, 25);
                    screenDot[i, j].OutlineColor = new Color(120, 120, 120);
                    screenDot[i, j].OutlineThickness = 1;
                    screenDot[i, j].Position = new Vector2f(i * cWidth / 2, j * cHeight / 2);
                    screenDot[i,j].Size = new Vector2f(cWidth, cHeight);
                    if (cells[i][j].Alive == 1)
                        screenDot[i,j].FillColor = Color.White;
                    if (!_resumed)
                    {
                        if (gameController.Choice[0, 0] == i && gameController.Choice[0, 1] == j)
                        {
                            screenDot[i,j].FillColor = Color.Green;
                        }
                    }
                    window.Draw(screenDot[i,j]);
                }
            }
        }
        public override void OnKeyPressed(KeyEventArgs e)
        {
            //if (e.Code == (Keyboard.Key.Left))
            //    Choice = new int[,] { { --Choice[0, 0], Choice[0, 1] } };
            //if (e.Code == (Keyboard.Key.Right))
            //    Choice = new int[,] { { ++Choice[0, 0], Choice[0, 1] } };
            //if (e.Code == (Keyboard.Key.Up))
            //    Choice = new int[,] { { Choice[0, 0], --Choice[0, 1] } };
            //if (e.Code == (Keyboard.Key.Down))
            //    Choice = new int[,] { { Choice[0, 0], ++Choice[0, 1] } };
            if (e.Code == (Keyboard.Key.Enter))
            {
                Cell cellRef = cells[gameController.Choice[0, 0]][gameController.Choice[0, 1]];
                cellRef.Alive = cellRef.Alive == 1 ? (short)0 : (short)1;
            }
            if (e.Code == (Keyboard.Key.Space))
            {
                //ReadMap();
                _resumed = !_resumed;
            }
        }
    }
}
