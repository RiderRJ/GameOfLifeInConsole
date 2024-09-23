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
            for (int i = 0; i < width; i++)
                for (int k = 0; k < height; k++)
                {
                    cells[i][k].Alive = 0;
                }
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
        public override void Init()
        {
            base.Init();
            CreateMap();
        }
        public override void Update()
        {
            CellsLife();
            Draw();
        }
    }
}
