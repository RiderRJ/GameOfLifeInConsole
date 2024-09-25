using SFML.Graphics;

namespace GameOfLife
{
    public static class RenderWindowExtensions
    {
        public static void Draw(this RenderWindow window, IExtentedDrawable whatToDraw)
        {
            whatToDraw.Draw(window);
        }
        public static float GetAspect(this RenderWindow window) =>
            (float)window.Size.X / window.Size.Y;
    }
}
