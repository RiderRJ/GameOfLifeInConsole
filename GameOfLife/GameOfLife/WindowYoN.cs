namespace GameOfLife
{
    public class WindowYoN : WindowType
    {
        public WindowYoN() 
        {
            BtnVariants = new string[] { "Yes", "No" };
            buttonsNum = (ushort)BtnVariants.Length;
        }
    }
}
