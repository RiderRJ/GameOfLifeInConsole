using System;
using System.Threading.Tasks;

namespace GameOfLife
{
    public static class KeyCatcher
    {
        public static ConsoleKey pressedKey;
        public static void StartTracking()
        {
           _ = WaitCatch();
        }
        public static async Task WaitCatch()
        {
            await Task.Run(async() =>
            {
                while (true)
                {
                    pressedKey = Console.ReadKey(true).Key;
                    await Task.Delay(1);
                    pressedKey = new ConsoleKey();
                }
            });
        }
    }
}
