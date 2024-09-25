using static GameOfLife.Cell;

namespace GameOfLife
{
    public sealed class RandomField : GameField
    {
        private int chance;
        public RandomField(int chance = 0)
        {
            this.chance = chance;
        }
        private void CreateMap()
        {
            ClearMap();
            for (int i = 0; i < Map.GetLength(0); i++)
                for (int k = 0; k < Map.GetLength(1); k++)
                {
                    if (rnd.Next(0, 99) < chance)
                    {
                        cells[i][k].Alive = 1;
                        cells[i][k].onChangeState(cells[i][k], 1);
                    }
                }
        }
        private void ClearMap()
        {
            for (int i = 0; i < width; i++)
                for (int k = 0; k < height; k++)
                {
                    cells[i][k].Alive = 0;
                    cells[i][k].neighbours = 0;
                }
        }
        public override void Init()
        {
            base.Init();
            window.KeyPressed += (s, e) =>
            {
                if (e.Code == SFML.Window.Keyboard.Key.F1)
                    CreateMap();
                if (e.Code == SFML.Window.Keyboard.Key.F2)
                    ClearMap();
            };
            CreateMap();
        }
        public override void Update()
        {
            CellsLife();
            Draw();
        }
    }
}
