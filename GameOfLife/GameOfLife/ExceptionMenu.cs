using System;

namespace GameOfLife
{
    public sealed class ExceptionMenu : Window
    {
        private static string[] exceptionWord = new string[] { "GOT IT!", "OH...", "OK", "GTFO!" ,"KK", "MAMA MIA", "БЛЯТЬ", "ЗАВАЛИ"};
        public ExceptionMenu(string content, WindowType type, string[] btnVariants = null, string title = "") : base(content, type, btnVariants,title)
        {

        }
        public static void Throw(object exception)
        {
            string message = exception is Exception ?
                (exception as Exception).Message :
                exception.ToString();
            new Window(message, WindowType.Message, exceptionWord);
        }
    }
}
