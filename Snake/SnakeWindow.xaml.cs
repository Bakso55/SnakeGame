using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Snake
{
    public partial class SnakeWindow : Window
    {
        private MySnake _snake;
        private DispatcherTimer _timer;
        private SnakePart _food;
        private int _partsToAdd;
        private int _directionX = 1;
        private int _directionY = 0;
        public SnakeWindow()
        {
            InitializeComponent();
            InitBoard();
            InitSnake();
            InitTimer();
            InitFood();
        }
        void InitBoard()
        { //INICJALIZACJA PLANSZY
            for (int i = 0; i < grid.Width / 10; i++)
            {
                ColumnDefinition columnDefinitions = new ColumnDefinition();
                columnDefinitions.Width = new GridLength(10);
                grid.ColumnDefinitions.Add(columnDefinitions);
            }
            for (int j = 0; j < grid.Height / 10; j++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(10);
                grid.RowDefinitions.Add(rowDefinition);
            }
            _snake = new MySnake();
        }
        void InitSnake()
        { //INICJALIZACJA WEZA
            grid.Children.Add(_snake.Head.Rectang);
            foreach (SnakePart snakePart in _snake.Parts) grid.Children.Add(snakePart.Rectang);
            _snake.RedrawSnake();
        }
        void InitTimer()
        { //INICJALIZACJA I DEFINICJA USTAWIEN TIMERA (RUCH WEZA)
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            _timer.Start();
        }
        void _timer_Tick(object sender, EventArgs e)
        {//PORUSZANIE SIE WEZA ZALEZNE OD ODLICZEN TIMERA
            MoveSnake();
        }
        void InitFood()
        { //DEFINICJA 'JEDZENIA'
            _food = new SnakePart(10, 10);
            _food.Rectang.Width = _food.Rectang.Height = 50;
            _food.Rectang.Fill = Brushes.Blue;
            grid.Children.Add(_food.Rectang);
            Grid.SetColumn(_food.Rectang, _food.X);
            Grid.SetRow(_food.Rectang, _food.Y);
        }
        private void MoveSnake()
        { //RUCH WEZA
            int snakePartCount = _snake.Parts.Count;
            if (_partsToAdd > 0)
            {
                SnakePart newPart = new SnakePart(_snake.Parts[_snake.Parts.Count - 1].X,
                _snake.Parts[_snake.Parts.Count - 1].Y);
                grid.Children.Add(newPart.Rectang);
                _snake.Parts.Add(newPart);
                _partsToAdd--;
            }

            for (int i = snakePartCount - 1; i >= 1; i--)
            {
                _snake.Parts[i].X = _snake.Parts[i - 1].X;
                _snake.Parts[i].Y = _snake.Parts[i - 1].Y;
            }
            _snake.Parts[0].X = _snake.Head.X;
            _snake.Parts[0].Y = _snake.Head.Y;
            _snake.Head.X += _directionX;
            _snake.Head.Y += _directionY;
            _snake.RedrawSnake();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            { //STEROWANIE WEZEM, SWITCH I WYBOR SRZALKAMI KIERUNKU
                case Key.Left:
                    {
                        if (_directionX != 1) _directionX = -1;
                        _directionY = 0;
                    }
                    break;

                case Key.Right:
                    {
                        if (_directionX != -1) _directionX = 1;
                        _directionY = 0;
                    }
                    break;
                case Key.Up:
                    {
                        if (_directionY != 1) _directionY = -1;
                        _directionX = 0;
                    }
                    break;

                case Key.Down:
                    {
                        if (_directionY != -1) _directionY = 1;
                        _directionX = 0;
                    }
                    break;
            }
        }
    }
}
