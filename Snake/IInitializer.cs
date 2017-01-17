using System;

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
