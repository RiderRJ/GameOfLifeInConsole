using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GameOfLife.KeyCatcher;

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
        public virtual async Task Update()//переопределять Init, а не Update
        {
            Init();
            while (true)
            {
                frames++;
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
                await UpdateMap();//просадка со 7 до 4 фпс
                await Draw(); //без отрисовки ~20 кадров
                await Task.Delay(1);
            }
            instance.Update();
        }
        private async Task Draw()
        {
            string screen = "";
            long size = -1;
            await Task.Run(() =>
            {
                Console.Clear();
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int k = 0; k < map.GetLength(1); k++)
                    {
                        if(!_resumed)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            if (Choice[0, 0] == i && Choice[0, 1] == k)
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                        }
                        //screen += map[i, k];
                        Console.Write(map[i, k]);
                    }
                    //screen += "\r\n";
                    Console.WriteLine();
                }
                //using (Stream s = new MemoryStream())
                //{
                //    BinaryFormatter formatter = new BinaryFormatter();
                //    formatter.Serialize(s, Cell.cells);
                //    formatter.Serialize(s, Cell.livingCells);
                //    formatter.Serialize(s, Cell.nMLivingCells);
                //    size = s.Length;
                //}
                //Console.Write(screen);
                Console.WriteLine($"fps = {fps} tick = {updates} lists memory usage = {size/1024f} KB pressed key = {pressedKey} is playing = {_resumed}");
            });
        }
        private void Input()//перенести в другой класс. подсосать из того проекта
        {
            if (pressedKey == ConsoleKey.UpArrow)
                Choice = new int[,] { { --Choice[0, 0], Choice[0, 1] } };
            if (pressedKey == ConsoleKey.DownArrow)
                Choice = new int[,] { { ++Choice[0, 0], Choice[0, 1] } };
            if (pressedKey == ConsoleKey.LeftArrow)
                Choice = new int[,] { { Choice[0, 0], --Choice[0, 1] } };
            if (pressedKey == ConsoleKey.RightArrow)
                Choice = new int[,] { { Choice[0, 0], ++Choice[0, 1] } };
            if (pressedKey == ConsoleKey.Enter)
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
            if (pressedKey == ConsoleKey.Spacebar)
            {
                //ReadMap();
                _resumed = !_resumed;
            }
        }
        /// <summary>
        /// Правило возвращае целочисленое значение alive -> 0 = мёртв, 1 = жив;
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
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
            Thread sec = new Thread(new ThreadStart(StartTracking));
            sec.Priority = ThreadPriority.Highest;
            sec.IsBackground = false;
            sec.Start();
            FPSReset();
            instance = new ChoiceMenu();
            instance.Update();
            while (true)
            {
                ;
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
                if (value < 0) value = choices.Length;
                choice = value;
            }
        }
        public static void Init()
        {
            
        }
        public override async Task Update()
        {
            Init();
            while(true)
            {
                if (pressedKey == ConsoleKey.UpArrow)
                    Choice--;
                if (pressedKey == ConsoleKey.DownArrow)
                    Choice++;
                if (pressedKey == ConsoleKey.Enter)
                {
                    instance = scenes[Choice];
                    break;
                }
                await Draw();
                await Task.Delay(100);
            }
            instance.Update();
        }
        private async Task Draw()
        {
            await Task.Run(() =>
            {
                Console.Clear();
                for (int i = 0; i < choices.Length; i++)
                {
                    if (i == Choice)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("   ");
                    }
                    Console.WriteLine(choices[i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            });
        }
    }
}
