using System;

namespace GameOfLife
{
    public abstract class WindowType
    {
        public ushort buttonsNum;
        protected string[] BtnVariants { get; set; }
        private uint position = 0;
        private uint Position
        {
            get => position;
            set
            {
                if (value > BtnVariants.Length) value = (uint)BtnVariants.Length - 1;
                position = value;
            }
        }
        public void Prev() => Position--;
        public void Next() => Position++;
        public void First() => Position = 0;
        public void Last() => Position = (uint)BtnVariants.Length;
        public virtual string GetText()
        {
            string text = BtnVariants[Position];
            Next();
            return text;
        }
    }
}

