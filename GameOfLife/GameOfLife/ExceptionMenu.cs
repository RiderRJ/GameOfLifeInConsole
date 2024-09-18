using SFML.Graphics;
using SFML.System;
using System;
using System.Linq;

namespace GameOfLife
{
    public sealed class ExceptionMenu : Window
    {
        private static string[] exceptionWord = new string[] { "GOT IT!", "OH...", "OK", "GTFO!" ,"KK"};
        public ExceptionMenu(string content, WindowType type, string title = "Oops!", string[] btnVariants = null) : base(content, type, title, btnVariants)
        {
        }
        public static void Throw(object exception)
        {
            string message = exception is Exception ?
                (exception as Exception).Message :
                exception.ToString();
            new Window(message, WindowType.Message, "Oops!", exceptionWord);
        }
    }
}
