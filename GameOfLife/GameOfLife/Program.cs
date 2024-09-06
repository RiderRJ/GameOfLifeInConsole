using Microsoft.Win32.SafeHandles;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public abstract class Thinker
    {
        public static List<Thinker> thinkers = new List<Thinker>();
        public abstract void Think();
        public abstract void NextTurn();

    }
    public delegate int Rule(int neighs, int alive);
    class Cell : Thinker
    {
        private List<Cell> cells;
        public static Rule rule;
        private int x, y;
        private int alive;
        public int neighbours;

        public int Alive //идентификатор
        {
            get
            {
                return alive;
            }
            private set
            {
                alive = value;
            }
        }
        private int nextMoveAlive;
        public int NextMoveAlive 
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
        public Cell(int x, int y,int alive, ref List<Cell> cells)
        {
            this.x = x; this.y = y;
            this.cells = cells;
            Alive = alive;
            thinkers.Add(this);
        }
        public Cell(int x, int y)
        {
            this.x = x; this.y = y;
        }
        public static Cell operator +(Cell a, Cell b)
        {
            return new Cell(a.x + b.x, a.y + b.y);
        }
        public static Cell operator -(Cell a, Cell b)
        {
            return new Cell(a.x - b.x, a.y - b.y);
        }
        public static bool operator ==(Cell a, Cell b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Cell a, Cell b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public override void Think()//Принимают решение на следующий ход
        {
            neighbours = 0;
            foreach(var cell in cells.Where(cell => cell.Alive == 1))
            {
                if (cell == this) continue;
                Cell delta = cell - this;
                if (Math.Abs(delta.x) > 1 || Math.Abs(delta.y) > 1)
                    continue;
                neighbours++;
            } //Подсчет живых соседей
            nextMoveAlive = rule(neighbours, Alive);
        }
        public override void NextTurn()
        {
            Alive = nextMoveAlive;
        }
    }
    internal class Program
    {
        private static char[,] map;
        private static List<Cell> cells = new List<Cell>();
        private static int updates = 0;
        private static Random rnd = new Random();
        private static void CreateMap()
        {
            map = new char[,]
{
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','#','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','#','#','#','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','#','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
                {'-', '-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-'},
};
            for(int i =0; i < map.GetLength(0); i++)
                for(int k = 0; k< map.GetLength(1);k++)
                {
                    if (rnd.Next(0, 99) < 10)
                        map[i, k] = '#';
                }
        }
        private static void ReadMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int k = 0; k < map.GetLength(1); k++)
                {
                    if (map[i, k] == '#')
                        cells.Add(new Cell(i, k, 1, ref cells));
                    else
                        cells.Add(new Cell(i, k, 0, ref cells));
                }
        }
        private static void UpdateMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int k = 0; k < map.GetLength(1); k++)
                {
                    //map[i, k] = cells[cells.FindIndex(c => c == new Cell(i, k))].neighbours.ToString().ToCharArray()[0];
                    List<Cell> aliveCells = cells.Where(cell => cell.Alive == 1).ToList();
                    if (aliveCells.FindIndex(c => c == new Cell(i, k)) != -1)
                        map[i, k] = '#';
                    else map[i, k] = '-';
                }
        }
        private static void Init()
        {
            CreateMap();
            ReadMap();
        }
        private async Task Update()
        {
            while(true)
            {
                updates++;
                if(updates > 100)
                    Environment.Exit(0);
                foreach (var cell in Thinker.thinkers)
                    cell.Think();
                foreach (var cell in Thinker.thinkers)
                    cell.NextTurn();
                UpdateMap();
                await Draw();
                await Task.Delay(500);
            }
        }
        private async Task Draw()
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int k = 0; k < map.GetLength(1); k++)
                    Console.Write(map[i, k]);
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Правило возвращаетелочисленое значение alive -> 0 = мёртв, 1 = жив;
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program instance = new Program();
            Cell.rule = delegate (int neighbours, int alive)
            {
                if(alive == 1)
                    if (neighbours < 2 || neighbours > 3) return 0;
                if (alive == 0)
                    if (neighbours == 3)
                        return 1;
                return alive;
            };
            Init();
            instance.Update();
            Console.ReadKey();
        }
    }
}
