namespace GameOfLife
{
    public sealed class Sandbox : GameField
    {
        public Sandbox() =>
            _resumed = false;
        public override void Update()
        {
            CellsLife();
            Draw();
        }
    }
}
