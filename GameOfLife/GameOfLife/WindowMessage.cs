namespace GameOfLife
{
    public class WindowMessage : WindowType
    {
        public WindowMessage(string[] strings = null) =>
            BtnVariants = strings ?? new string[] { "GATCHA", "OK", "Хорошо" };
        
        public override string GetText() =>
             BtnVariants[ApplicationHolder.rnd.Next(0, BtnVariants.Length)];
    }
}
