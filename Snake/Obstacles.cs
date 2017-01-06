using System.Windows.Shapes;


namespace Snake
{
    class Obstacles : IRandomColor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Rectang { get; private set; }
        public Obstacles(int x, int y, int width, int height)
        {//DEFINICJA PRZESZKOD
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Rectang = new Rectangle();
            Rectang.Width = 10 * width;
            Rectang.Height = 10 * height;
            Rectang.Fill = PickBrush();
        }
       
    }
}
