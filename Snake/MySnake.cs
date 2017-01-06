using System.Collections.Generic;
using System.Windows.Controls;

namespace Snake
{
   
    class MySnake
    { //KLASA 'WAZ'
        public SnakePart Head { get; private set; }
        public List<SnakePart> Parts { get; private set; }
        public MySnake()
        { //DEFINICJA BLOKU WEZA
            Head = new SnakePart(30, 0);
            Head.Rectang.Width = Head.Rectang.Height = 50;
            Head.Rectang.Fill = System.Windows.Media.Brushes.Black;
            Parts = new List<SnakePart>();
            Parts.Add(new SnakePart(29, 0));
        }
        public void RedrawSnake()
        { //RYSOWANIE WEZA
            Grid.SetColumn(Head.Rectang, Head.X);
            Grid.SetRow(Head.Rectang, Head.Y);
            foreach (SnakePart snakePart in Parts)
            {
                Grid.SetColumn(snakePart.Rectang, snakePart.X);
                Grid.SetRow(snakePart.Rectang, snakePart.Y);
            }
        }
    }

}
