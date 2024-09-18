using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public abstract class GameField : Program
    {
        protected static Random rnd = new Random();
        protected static short height = 100;
        protected static short width = 100;
        public static char[,] Map { get; protected set; } = new char[width, height];
        protected int[,] choice = new[,]
        {
            { 0, 0 },
        };
        protected int[,] Choice
        {
            get => choice;
            set
            {
                if (value[0, 0] >= Map.GetLength(0)) value[0, 0] = 0;
                if (value[0, 0] < 0) value[0, 0] = Map.GetLength(0);
                if (value[0, 1] >= Map.GetLength(1)) value[0, 1] = 0;
                if (value[0, 1] < 0) value[0, 1] = Map.GetLength(1);
                choice = value;
            }
        }
        protected bool _resumed = true;
        protected RectangleShape[,] screenDot = new RectangleShape[width,height];
        public GameField()
        {
            for(int i = 0; i < screenDot.GetLength(0); i++)
            {
                for (int j = 0; j < screenDot.GetLength(1); j++)
                    screenDot[i, j] = new RectangleShape();
            }
        }
        public void CellsLife()
        {
            if (_resumed)
            {
                Cell.nMLivingCells = Cell.livingCells.ToList();
                foreach (var cell in Thinker.thinkers)
                    cell.Think();
                foreach (var cell in Thinker.thinkers)
                    cell.NextTurn();
            }
            UpdateMap();
        }
        public override void Init()
        {
            Cell.rule = delegate (short neighbours, short alive, Cell itself)
            {
                if (alive == 1)
                    if (neighbours < 2 || neighbours > 3)
                    {
                        Cell.nMLivingCells.Remove(itself);
                        return 0;
                    }
                if (alive == 0)
                    if (neighbours == 3)
                    {
                        Cell.nMLivingCells.Add(itself);
                        return 1;
                    }
                return alive;
            };
        }
        protected static void ReadMap()
        {
            for (short i = 0; i < Map.GetLength(0); i++)
                for (short k = 0; k < Map.GetLength(1); k++)
                {
                    if (Map[i, k] == '#')
                        new Cell(i, k, 1);
                    else
                        new Cell(i, k, 0);
                }
            //foreach (var cell in Cell.cells)
            //    cell.Ready();
        }
        protected static void UpdateMap()
        {
            for (short i = 0; i < Map.GetLength(0); i++)
                for (short k = 0; k < Map.GetLength(1); k++)
                {
                    List<Cell> aliveCells = Cell.livingCells;
                    if (aliveCells.FindIndex(c => c == new Cell(i, k)) != -1)
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
                    screenDot[i, j].Position = new Vector2f(i * cWidth / 2, j * cHeight / 2);
                    screenDot[i,j].Size = new Vector2f(cWidth, cHeight);
                    if (Cell.livingCells.FindIndex(c => c == new Cell(i, j)) != -1)
                        screenDot[i,j].FillColor = Color.White;
                    if (!_resumed)
                    {
                        if (Choice[0, 0] == i && Choice[0, 1] == j)
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
            if (e.Code == (Keyboard.Key.Left))
                Choice = new int[,] { { --Choice[0, 0], Choice[0, 1] } };
            if (e.Code == (Keyboard.Key.Right))
                Choice = new int[,] { { ++Choice[0, 0], Choice[0, 1] } };
            if (e.Code == (Keyboard.Key.Up))
                Choice = new int[,] { { Choice[0, 0], --Choice[0, 1] } };
            if (e.Code == (Keyboard.Key.Down))
                Choice = new int[,] { { Choice[0, 0], ++Choice[0, 1] } };
            if (e.Code == (Keyboard.Key.Enter))
            {
                int cellID = Cell.cells.FindIndex(cell => cell == new Cell((short)Choice[0, 0], (short)Choice[0, 1]));
                if (Cell.livingCells.FindIndex(cell => cell == new Cell((short)Choice[0, 0], (short)Choice[0, 1])) == -1)
                {
                    Cell.cells[cellID].Alive++;
                    Cell.livingCells.Add(Cell.cells[cellID]);
                }
                else
                {
                    Cell.cells[cellID].Alive--;
                    Cell.livingCells.Remove(Cell.cells[cellID]);
                }
            }
            if (e.Code == (Keyboard.Key.Space))
            {
                //ReadMap();
                _resumed = !_resumed;
            }
        }
    }
}
