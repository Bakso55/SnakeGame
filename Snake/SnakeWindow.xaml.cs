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
            foreach (SnakePart snakePart in _snake.Parts)
                grid.Children.Add(snakePart.Rectang);
            _snake.RedrawSnake();

        }
        void InitTimer()
        { //INICJALIZACJA I DEFINICJA USTAWIEN TIMERA (RUCH WEZA)
            _timer = new DispatcherTimer();
           // _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
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
    }
}
