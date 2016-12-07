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
        public SnakeWindow()
        {
            InitializeComponent();
            InitBoard();
            InitSnake();
            InitTimer();
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
    }
}
