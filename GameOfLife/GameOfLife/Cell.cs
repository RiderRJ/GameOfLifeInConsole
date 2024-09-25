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

    public delegate void CellAction(Cell sender, short? forcedIncrement = null);//для сообщения соседям об оживлении или омертвлении клетки

    [Serializable]
    public sealed class Cell : Thinker
    {
        public Cell(short x, short y, short alive)
        {
            onChangeState += OnChangeState;
            this.x = x; this.y = y;
            Alive = alive;
        }
        //найти способ уменьшить потребление
        public static List<List<Cell>> cells = new List<List<Cell>>();
        public static Rule rule;
        public CellAction onChangeState;
        private readonly short x, y;
        public short neighbours;
        private short alive;
        public short Alive //переделать
        {
            get => alive;
            set => alive = value;
        }
        private short nextMoveAlive;
        /// <summary>
        /// Принимают решение на следующий ход
        /// </summary>
        public override void Think()
        {
            nextMoveAlive = rule(neighbours, Alive, this);
        }
        public override void NextTurn()
        {
            if (Alive != nextMoveAlive)
                onChangeState(this);
            Alive = nextMoveAlive;
        }
        public static void CreateCellField(short x, short y)
        {
            cells.Clear();
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
        private void OnChangeState(Cell sender, short? forcedIncrement = null)
        {
            int maxDelta = 1;
            int xMin = Math.Max(0, x - maxDelta);
            int xMax = Math.Min(cells.Count - 1, x + maxDelta);
            int yMin = Math.Max(0, y - maxDelta);
            int yMax = Math.Min(cells.First().Count - 1, y + maxDelta);
            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    if (x == this.x && y == this.y)
                        continue;
                    cells[x][y].neighbours += forcedIncrement ?? ((sender.nextMoveAlive == 1) ? (short)1 : (short)-1);
                }
            }
        }
    }
}
