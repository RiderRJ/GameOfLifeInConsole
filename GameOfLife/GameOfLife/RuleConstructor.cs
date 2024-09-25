using System.Collections.Generic;

namespace GameOfLife
{
    public sealed class RuleConstructor
    {
        public List<(short, char)> deadArgs = new();
        public List<(short, char)> liveArgs = new();
        public int neighsArgs;
        public Cell[][] neightWMaskArgs;
        public int fillPercent;
        //меню с выбором сверху
        //снизу список с выбранным и деталями.
        //функционал меню выбора:
        //шанс заполнения клетки при генерации (не стакается)
        //одно if правило для живой клетки (стакается):
        //одно if правило для мертвой клетки (стакается):
        //предел поиска соседа (не стакается)
        //правила для поиска соседа:
        //открыть меню с одной клеткой в центре и нужно закрасить соседние клетки которые будут в роли маски
        //правила перезаписывают друг друга
    }
}
