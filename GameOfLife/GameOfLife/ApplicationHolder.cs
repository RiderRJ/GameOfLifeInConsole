using SFML.Graphics;
using System;

namespace GameOfLife
{
    public abstract class ApplicationHolder
    {
        public static Random rnd = new Random();
        public static Font font = new Font("fonts/arial.ttf");
    }
}
