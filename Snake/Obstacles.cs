using System;
using System.Reflection;
using System.Windows.Media;
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

        public Brush PickBrush()
        { //LOSOWANIE KOLORU
            Brush result = Brushes.Transparent;
            Random rnd = new Random();
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();
            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            if (result == Brushes.White || result == Brushes.Black) return PickBrush();
            else return result;
        }

    }
}
