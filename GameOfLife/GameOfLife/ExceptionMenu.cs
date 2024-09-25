using System;

namespace GameOfLife
{
    public sealed class ExceptionMenu : Window
    {
        private static string[] exceptionWord = new string[] { "GOT IT!", "OH...", "OK", "GTFO!" ,"KK", "MAMA MIA", "БЛЯТЬ", "ЗАВАЛИ"
        ,"ЛАГАЕТ","*зевок*","*заткнул уши*","OK, THEN?","HUESOSI!!!"};
        public ExceptionMenu(string content, WindowType type, string title = "") : base(content, type, title) //педик на вижуал заставил меня это сделать
        {
            throw new NotImplementedException("Программист хуесос!");
        }
        public static void Throw(object exception)
        {
            string message = exception is Exception ?
                (exception as Exception).Message :
                exception.ToString();
            new Window(message, new WindowMessage(exceptionWord));
        }
    }
}
