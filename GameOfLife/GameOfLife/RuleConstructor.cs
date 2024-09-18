using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public sealed class RuleConstructor
    {
        private List<Tuple<short, char>> deadArgs;
        private List<Tuple<short, char>> liveArgs;
        private List<int> neighsArgs;
        private List<Cell[]> neightWMaskArgs;
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
