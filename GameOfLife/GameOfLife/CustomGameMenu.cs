using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class CustomGameMenu : Program
    {
        string[] choices = { };
        object[] links = new object[] { };
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
        public override void Init()
        {
            ExceptionMenu.Throw(new NotImplementedException());
        }

        public override void OnKeyPressed(KeyEventArgs e)
        {
            ExceptionMenu.Throw(new NotImplementedException());
        }

        public override void Update()
        {
            ExceptionMenu.Throw(new NotImplementedException());
        }
    }
}
