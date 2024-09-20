using SFML.Window;

namespace GameOfLife
{
    public sealed class Controller
    {
        private static ushort xLim;
        private static ushort yLim;
        public ushort X
        {
            get => xLim; set { xLim = value; Choice[0, 0] = (ushort)(value / 2); }
        }
        public ushort Y
        {
            get => yLim;
            set { yLim = value; Choice[0, 1] = (ushort)(value / 2); }
        }
        private ushort[,] choice = new ushort[,]
        {
            { (ushort)(xLim/2), (ushort)(yLim/2) },
        };
        public ushort[,] Choice
        {
            get => choice;
            set
            {
                if (value[0, 0] >= xLim) value[0, 0] = 0;
                if (value[0, 1] >= yLim) value[0, 1] = 0;
                choice = value;
            }
        }
        public void Controll(KeyEventArgs e)
        {
            if (e.Code == (Keyboard.Key.Left))
                Choice = new ushort[,] { { --Choice[0, 0], Choice[0, 1] } };
            if (e.Code == (Keyboard.Key.Right))
                Choice = new ushort[,] { { ++Choice[0, 0], Choice[0, 1] } };
            if (e.Code == (Keyboard.Key.Up))
                Choice = new ushort[,] { { Choice[0, 0], --Choice[0, 1] } };
            if (e.Code == (Keyboard.Key.Down))
                Choice = new ushort[,] { { Choice[0, 0], ++Choice[0, 1] } };
        }
    }
}
