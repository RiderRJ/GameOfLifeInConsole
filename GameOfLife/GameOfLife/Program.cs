using System;
using SFML;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GameOfLife.KeyCatcher;
using SFML.Graphics;
using System.Linq.Expressions;
using SFML.System;
using SFML.Window;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;

namespace GameOfLife
{
    [Serializable]
    public abstract class Thinker
    {
        public static List<Thinker> thinkers = new List<Thinker>();
        public abstract void Ready();
        public abstract void Think();
        public abstract void NextTurn();

    }
    public delegate short Rule(short neighs, short alive, Cell itself);
    public delegate void CellAction(Cell sender);
    [Serializable]
    public class Cell : Thinker
    {
        //превратить в список списков, чтобы при рождении и смерти клетки, было проще добавлять или удалять соседям neighbours
        //найти способ уменьшить потребление
        public static List<Cell> cells = new List<Cell>();
        public static List<Cell> nMLivingCells = new List<Cell>();
        public static List<Cell> livingCells = new List<Cell>();
        public static Rule rule;
        public CellAction onBirth;
        public CellAction onDeath;
        private short x, y;
        private short alive;
        public short neighbours;
        public short Alive //идентификатор
        {
            get
            {
                return alive;
            }
            set
            {
                alive = value;
            }
        }
        private short nextMoveAlive;
        public short NextMoveAlive 
        {
            get
            {
                return nextMoveAlive;
            }
            private set
            {
                nextMoveAlive = value;
            }
        }
        #region Constructors
        public Cell(short x, short y, short alive)
        {
            onBirth += OnBirth;
            onDeath += OnDeath;
            this.x = x; this.y = y;
            Alive = alive;
            if (Alive == 1)
            {
                livingCells.Add(this);
            }
            cells.Add(this);
            thinkers.Add(this);
        }
        public Cell(short x, short y)
        {
            this.x = x; this.y = y;
            Alive = 0;
        }

        #endregion
        #region Operators
        public static Cell operator +(Cell a, Cell b)
        {
            return new Cell((short)(a.x + b.x), (short)(a.y + b.y));
        }
        public static Cell operator -(Cell a, Cell b)
        {
            return new Cell((short)(a.x - b.x), (short)(a.y - b.y));
        }
        public static bool operator ==(Cell a, Cell b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Cell a, Cell b)
        {
            return a.x != b.x || a.y != b.y;
        }

        #endregion
        /// <summary>
        /// Сообщает системе, что готова к следующему ходу.
        /// </summary>
        public override void Ready()
        {
            if(livingCells.Contains(this))
                onBirth?.Invoke(this);
        }
        /// <summary>
        /// Принимают решение на следующий ход
        /// </summary>
        public override void Think()
        {
            neighbours = 0;
            foreach (var cell in livingCells)
            {
                if (cell == this) continue;
                Cell delta = cell - this;
                if (Math.Abs(delta.x) > 1 || Math.Abs(delta.y) > 1)//Заменить подсчет на то, чтобы кажда живая клетка прибавляла к числу соседей соседних клеток 1 -> мб так будет быстрее
                    continue;
                neighbours++;
            } //Подсчет живых соседей
            if (nMLivingCells.Count != 0)
                ;
            nextMoveAlive = rule(neighbours, Alive, this);//Пропажа живой клетки 
        }
        public override void NextTurn()
        {
            if (Alive != nextMoveAlive)
            {
                //if (nextMoveAlive == 1)
                //    onBirth?.Invoke(this);
                //if (nextMoveAlive == 0)
                //    onDeath?.Invoke(this);
            }
            if (nMLivingCells.Count != 0)
                ;
            Alive = nextMoveAlive;
            livingCells = nMLivingCells;
        }
        private void OnBirth(Cell sender)
        {
            //добавить соседей
            int footer = cells.FindIndex(cell => cell == new Cell((short)(sender.x - 1), sender.y));//
            int upper  = cells.FindIndex(cell => cell == new Cell((short)(sender.x + 1), sender.y));
            int right = cells.FindIndex(cell => cell == new Cell(sender.x, (short)(sender.y + 1)));
            int left = cells.FindIndex(cell => cell == new Cell(sender.x, (short)(sender.y - 1)));//
            if (right != -1 && right + 1 < cells.Count) 
                cells[right].neighbours++;
            if (left != -1 && left - 1 > 0) 
                cells[left].neighbours++;
            if (upper != -1) 
                cells[upper].neighbours++;
            if (footer != -1 && footer < cells.Count) 
                cells[footer].neighbours++;
        }
        private void OnDeath(Cell sender)
        {
            //удалить соседей
            int footer = cells.FindIndex(cell => cell == new Cell((short)(sender.x - 1), sender.y));//
            int upper = cells.FindIndex(cell => cell == new Cell((short)(sender.x + 1), sender.y));
            int right = cells.FindIndex(cell => cell == new Cell(sender.x, (short)(sender.y + 1)));
            int left = cells.FindIndex(cell => cell == new Cell(sender.x, (short)(sender.y - 1)));//
            if (right != -1 && right + 1 < cells.Count)
                cells[right].neighbours--;
            if (left != -1 && left - 1 > 0)
                cells[left].neighbours--;
            if (upper != -1)
                cells[upper].neighbours--;
            if (footer != -1 && footer < cells.Count)
                cells[footer].neighbours--;
        }

    }
    public class Program
    {
        public Font font;
        public static RenderWindow window;
        private static short height = 80;
        private static short width = 30;
        private static char[,] map = new char[width, height];
        private static int updates = 0;
        private static int frames = 0;
        private static int fps = 0;
        private int mode;
        //private static ConsoleKey key = ConsoleKey.Tab;
        private bool _resumed = true;
        private static Random rnd = new Random();
        public static Program instance = new Program(0, true);
        private int[,] choice = new[,]
        {
            { 0, 0 },
        };
        private int[,] Choice
        {
            get => choice;
            set
            {
                if (value[0, 0] >= map.GetLength(0)) value[0, 0] = 0;
                if (value[0, 0] < 0) value[0, 0] = map.GetLength(0);
                if (value[0, 1] >= map.GetLength(1)) value[0, 1] = 0;
                if (value[0, 1] < 0) value[0, 1] = map.GetLength(1);
                choice = value;
            }
        }

        public Program(int mode, bool resumedFromStart)
        {
            this.mode = mode;
            _resumed = resumedFromStart;
        } 
        public Program()
        {
            //выглядит очень не правильно
        }
        private static void CreateMap()
        {
            for (int i = 0; i < width; i++)
                for (int k = 0; k < height; k++)
                {
                    map[i, k] = '-';
                }
            //map = new char[,]
            //{
            //    {'-', '-', '-'},
            //    {'-','#','-' },
            //    {'-','#','-' },
            //    {'-','#','-' },
            //    {'-', '-', '-'},
            //};
            if (instance.mode == 0)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                    for (int k = 0; k < map.GetLength(1); k++)
                    {
                        if (rnd.Next(0, 99) < 35)
                            map[i, k] = '#';
                    }
            }
        }
        private static void ReadMap()
        {
            for (short i = 0; i < map.GetLength(0); i++)
                for (short k = 0; k < map.GetLength(1); k++)
                {
                    if (map[i, k] == '#')
                        new Cell(i, k, 1);
                    else
                        new Cell(i, k, 0);
                }
            //foreach (var cell in Cell.cells)
            //    cell.Ready();
        }
        private async static Task UpdateMap()
        {
            await Task.Run(() =>
            {
                for (short i = 0; i < map.GetLength(0); i++)
                    for (short k = 0; k < map.GetLength(1); k++)
                    {
                        //map[i, k] = Cell.cells[Cell.cells.FindIndex(c => c == new Cell(i, k))].neighbours.ToString().ToCharArray()[0];
                        List<Cell> aliveCells = Cell.livingCells;
                        if (aliveCells.FindIndex(c => c == new Cell(i, k)) != -1)
                            map[i, k] = '#';
                        else map[i, k] = '-';
                    }
            });
        }
        private async static Task FPSReset()
        {
            while(true)
            {
                fps = frames;
                frames = 0;
                await Task.Delay(1000);
            }
        }
        private static void Init()
        {
            CreateMap();
            ReadMap();
        }
        public virtual void Update()
        {
            Init();
            while (window.IsOpen)
            {
                frames++;
                window.DispatchEvents();
                if (_resumed) //обработка просаживает с 100 до 10 фпс
                {
                    Cell.nMLivingCells = Cell.livingCells.ToList();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    updates++;
                    if (updates > 6000)
                        Environment.Exit(0);
                    foreach (var cell in Thinker.thinkers)
                        cell.Think();//т.к поле в это время статичное можно распределить на потоки.
                    foreach (var cell in Thinker.thinkers)
                        cell.NextTurn();
                }
                Input();
                UpdateMap();//просадка со 7 до 4 фпс
                Draw(); //без отрисовки ~20 кадров
            }
            instance.Update();
        }
        private async Task Draw()
        {
            window.Clear(Color.Black);
            for(short i = 0; i < map.GetLength(0); i++)
            {
                for (short j = 0; j < map.GetLength(1); j++)
                {
                    float cWidth = window.Size.X * (1f / map.GetLength(0)) * 1.25f;
                    float cHeight = window.Size.Y * (1f / map.GetLength(1)) * 1.75f;
                    RectangleShape cell = new RectangleShape(new Vector2f(cWidth, cHeight))
                    {
                        FillColor = Color.Blue,
                        Position = new Vector2f(i * cWidth / 2, j * cHeight / 2)
                    };
                    if (Cell.livingCells.FindIndex(c => c == new Cell(i, j)) != -1)
                        cell.FillColor = Color.White;
                    if (!_resumed)
                    {
                        if (Choice[0, 0] == i && Choice[0, 1] == j)
                        {
                            cell.FillColor = Color.Red;
                        }
                    }
                    window.Draw(cell);
                }
            }
            window.Display();
        }
        private void Input()//перенести в другой класс. подсосать из того проекта
        {
        }
        /// <summary>
        /// Правило возвращае целочисленое значение alive -> 0 = мёртв, 1 = жив;
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(800, 600), "Game of life");
            window.Closed += (sender, e) => window.Close();
            window.KeyPressed += (sender, e) =>
            {
                if (e.Code == Keyboard.Key.Escape) { window.Close(); Environment.Exit(0); };
                instance.OnKeyPressed(e);
            };
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
            Thread sec = new Thread(new ThreadStart(StartTracking));
            sec.Priority = ThreadPriority.Highest;
            sec.IsBackground = false;
            sec.Start();
            FPSReset();
            instance = new ChoiceMenu();
            instance.Update();
        }
        public virtual void OnKeyPressed(KeyEventArgs e)
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
    public class ChoiceMenu : Program
    {
        string[] choices = { "Рандомная генерация поля", "Режим песочницы" };
        Program[] scenes = new Program[] { new Program(0, true), new Program(1, false) };
        private int choice = 1;
        private int Choice
        {
            get => choice;
            set
            {
                if (value >= choices.Length) value = 0;
                if (value < 0) value = choices.Length - 1;
                choice = value;
            }
        }
        public static void Init()
        {
            instance.font = new Font("fonts/arial.ttf");
        }
        public override void Update()
        {
            Init();
            while(true)
            {
                window.DispatchEvents();
                if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
                {
                    instance = scenes[Choice];
                    break;
                }
                Draw();
            }
            instance.Update();
        }
        private void Draw()
        {
            window.Clear();
            for (int i = 0; i < choices.Length; i++)
            {
                RectangleShape button = new RectangleShape(new Vector2f(200f, 40f))
                {
                    Position = new Vector2f(0, i * 40f / 2)
                };
                Text btnText = new Text(choices[i], instance.font)
                {
                    CharacterSize = 10,
                    Position = button.Position,
                    FillColor = Color.White
                };
                if (i == Choice)
                {
                    button.FillColor = Color.Green;
                    btnText.FillColor = Color.Black;
                }
                window.Draw(button); // пометить Cell как Drawable!!!! 
                window.Draw(btnText);
            }
            window.Display();
        }
        public override void OnKeyPressed(KeyEventArgs e)
        {
            if (e.Code == (Keyboard.Key.Up))
                Choice--;
            if (e.Code == (Keyboard.Key.Down))
                Choice++;
        }
    }
}
