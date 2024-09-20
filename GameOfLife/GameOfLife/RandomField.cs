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
                    Map[i, k] = '-';
                }
            for (int i = 0; i < Map.GetLength(0); i++)
                for (int k = 0; k < Map.GetLength(1); k++)
                {
                    if (rnd.Next(0, 99) < chance)
                        Map[i, k] = '#';
                }
        }
        public override void Init()
        {
            base.Init();
            CreateMap();
            ReadMap();
        }
        public override void Update()
        {
            CellsLife();
            Draw();
        }
    }
}
