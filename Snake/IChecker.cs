using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    interface IChecker
    {
        bool CheckCollision();
        bool CheckFood();
        bool IsFieldFree(int x, int y);
    }
}
