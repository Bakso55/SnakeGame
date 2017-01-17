using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    interface IInitializer
    {
        void InitBoard();
        void InitSnake();
        void InitTimer();
        void InitFood();
        void InitWall();
    }
}
