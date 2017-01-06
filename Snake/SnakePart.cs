using System.Windows.Shapes;

namespace Snake
{
    class SnakePart : IRandomColor

    { //KLASA 'ELEMENT WEZA'
        public int X { get; set; }
        public int Y { get; set; }
        public Rectangle Rectang { get; private set; }
        public SnakePart(int x, int y)
        {//DEFINICJA ELEMENTU WEZA
            X = x;
            Y = y;
            Rectang = new Rectangle();
            Rectang.Width = Rectang.Height = 50;
            Rectang.Fill = PickBrush();
        }
    }
}
