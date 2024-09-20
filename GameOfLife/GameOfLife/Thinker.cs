using System;
using System.Collections.Generic;

namespace GameOfLife
{
    [Serializable]
    public abstract class Thinker
    {
        public static List<Thinker> thinkers = new List<Thinker>();
        public Thinker()
        {
            thinkers.Add(this);
        }
        public abstract void Think();
        public abstract void NextTurn();
    }
}
