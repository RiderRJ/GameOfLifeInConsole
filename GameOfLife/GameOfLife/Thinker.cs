using System;
using System.Collections.Generic;

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

}
