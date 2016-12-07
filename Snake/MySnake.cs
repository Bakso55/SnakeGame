using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class SnakePart
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
        internal Brush PickBrush()
        { //LOSOWANIE KOLORU
            Brush result = Brushes.Transparent;
            Random rnd = new Random();
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();
            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            if (result == Brushes.White) return PickBrush();
            return result;
        }
    }
    class MySnake
    {
    }
}
