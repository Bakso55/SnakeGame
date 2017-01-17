using System;

namespace Snake
{
    interface IChecker
    {
        bool CheckCollision();
        bool CheckFood();
        bool IsFieldFree(int x, int y);
    }
}
