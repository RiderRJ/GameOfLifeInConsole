using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public delegate short Rule(short neighs, short alive, Cell itself);//общее правило, состоящее из >=1 правил живого и >=1 правил мертвого

    #region Переделать в методы
    public delegate short RuleForLiving(short neighs, char _operator, Cell itself);//правило для живой клетки -> цель смерть
    public delegate short RuleForDead(short neighs, char _operator, Cell itself);//правило для мертвой клетки -> цель ожить
    public delegate void NeighbourCheckByMask(Cell itself, Cell[] mask); //поиск соседей по маске
    public delegate void NeighbourCheck(Cell itself, int maxDelta);//поиск соседей по расстоянию
    #endregion

    public delegate void CellAction(Cell sender);//для сообщения соседям об оживлении или омертвлении клетки

    [Serializable]
    public sealed class Cell : Thinker
    {
        //превратить в список списков, чтобы при рождении и смерти клетки, было проще добавлять или удалять соседям neighbours
        //найти способ уменьшить потребление
        //public static List<Cell> cells = new List<Cell>();
        //public static List<Cell> nMLivingCells = new List<Cell>();
        //public static List<Cell> livingCells = new List<Cell>();   
        public static List<List<Cell>> cells = new List<List<Cell>>();
        public static List<List<Cell>> nMLivingCells = new List<List<Cell>>();
        public static List<List<Cell>> livingCells = new List<List<Cell>>();
        public static Rule rule;
        public CellAction onBirth;
        public CellAction onDeath;
        private short x, y;
        public short neighbours;
        private short alive;
        public short Alive //переделать
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
            get => nextMoveAlive;
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
            //if (Alive == 1)
            //{
            //    livingCells[x][y] = this;
            //}
            //cells[x][y] = this;
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
            if (cells[x][y].Alive == 1)
                onBirth?.Invoke(this);
        }
        /// <summary>
        /// Принимают решение на следующий ход
        /// </summary>
        public override void Think()
        {
            neighbours = 0;
            foreach (var row in cells)
                foreach (var cell in row)
                {
                    //Код для поиска соседей
                    for(int i = 0; i < 3; i++)
                    {
                        for (int k = 0; k < 3; k++)
                            ;
                    }

                    //if (cell == this) continue;
                    //Cell delta = cell - this;
                    //if (Math.Abs(delta.x) > 1 || Math.Abs(delta.y) > 1)//Заменить подсчет на то, чтобы кажда живая клетка прибавляла к числу соседей соседних клеток 1 -> мб так будет быстрее
                    //    continue;
                    neighbours++;
                } //Подсчет живых соседей
            nextMoveAlive = rule(neighbours, Alive, this);
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
            Alive = nextMoveAlive;
            //livingCells = nMLivingCells;
        }
        public static void CreateCellField(short x, short y)
        {
            for (short i = 0; i < x; i++)
            {
                List<Cell> cells = new List<Cell>();
                for (short k = 0; k < y; k++)
                {
                    cells.Add(new Cell(i, k, 0));
                }
                Cell.cells.Add(cells);
            }
        }
        private void OnBirth(Cell sender)
        {
            //добавить соседей
            //int footer = cells.FindIndex(cell => cell == new Cell((short)(sender.x - 1), sender.y));//
            //int upper = cells.FindIndex(cell => cell == new Cell((short)(sender.x + 1), sender.y));
            //int right = cells.FindIndex(cell => cell == new Cell(sender.x, (short)(sender.y + 1)));
            //int left = cells.FindIndex(cell => cell == new Cell(sender.x, (short)(sender.y - 1)));//
            //if (right != -1 && right + 1 < cells.Count)
            //    cells[right].neighbours++;
            //if (left != -1 && left - 1 > 0)
            //    cells[left].neighbours++;
            //if (upper != -1)
            //    cells[upper].neighbours++;
            //if (footer != -1 && footer < cells.Count)
            //    cells[footer].neighbours++;
        }
        private void OnDeath(Cell sender)
        {
            //удалить соседей
            //int footer = cells.FindIndex(cell => cell == new Cell((short)(sender.x - 1), sender.y));//
            //int upper = cells.FindIndex(cell => cell == new Cell((short)(sender.x + 1), sender.y));
            //int right = cells.FindIndex(cell => cell == new Cell(sender.x, (short)(sender.y + 1)));
            //int left = cells.FindIndex(cell => cell == new Cell(sender.x, (short)(sender.y - 1)));//
            //if (right != -1 && right + 1 < cells.Count)
            //    cells[right].neighbours--;
            //if (left != -1 && left - 1 > 0)
            //    cells[left].neighbours--;
            //if (upper != -1)
            //    cells[upper].neighbours--;
            //if (footer != -1 && footer < cells.Count)
            //    cells[footer].neighbours--;
        }
        public override string ToString()
        {
            return $"Cell x= {x} y= {y}";
        }
        public override bool Equals(object obj)
        {
            return obj is Cell cell &&
                   obj != null &&
                   x == cell.x &&
                   y == cell.y &&
                   alive == cell.alive;
        }

        public override int GetHashCode()
        {
            int hashCode = 1921429393;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + alive.GetHashCode();
            return hashCode;
        }
    }
}
