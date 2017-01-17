using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;

namespace Snake
{
    public partial class SnakeWindow : Window, IRank, IMusic, IChecker, IInitializer
    {
        private MySnake _snake;
        
        private DispatcherTimer _timer;
        private SnakePart _food, _food2, _food3;
        private int _partsToAdd;
        private int _directionX = 1;
        private int _directionY = 0;
        private List<Obstacles> _walls;
        private int score = 0;

        public SnakeWindow()
        {
            InitializeComponent();
            InitBoard();
            InitSnake();
            InitTimer();
            InitFood();
            InitWall();
            SoundPlayer();
        }

        

        public void SoundPlayer()
        {//MUZYKA W TLE
            SoundPlayer player = new SoundPlayer("sound.wav");
            player.PlayLooping();
        }

        public void InitBoard()
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

        public void InitSnake()
        { //INICJALIZACJA WEZA
            grid.Children.Add(_snake.Head.Rectang);
            foreach (SnakePart snakePart in _snake.Parts) grid.Children.Add(snakePart.Rectang);
            _snake.RedrawSnake();
        }

        public void InitTimer()
        { //INICJALIZACJA I DEFINICJA USTAWIEN TIMERA (RUCH WEZA)
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(_timer_Tick);
            //PREDKOSC WEZA ZALEZNA OD ZDOBYTYCH PUNKTOW
            if (score==0) _timer.Interval = new TimeSpan(0, 0, 0, 0, 70);
            if (score==7) _timer.Interval = new TimeSpan(0, 0, 0, 0, 60);
            if (score==14) _timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            if (score==20) _timer.Interval = new TimeSpan(0, 0, 0, 0, 40);
            if (score==26) _timer.Interval = new TimeSpan(0, 0, 0, 0, 35);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {//PORUSZANIE SIE WEZA ZALEZNE OD ODLICZEN TIMERA
            MoveSnake();
            ScoreDisplay.Text = Convert.ToString(score);
        }

        public void InitFood()
        { //DEFINICJA 'JEDZENIA'
            _food = new SnakePart(10, 10);
            _food.Rectang.Width = _food.Rectang.Height = 50;
            _food.Rectang.Fill = Brushes.Blue;
            grid.Children.Add(_food.Rectang);
            Grid.SetColumn(_food.Rectang, _food.X);
            Grid.SetRow(_food.Rectang, _food.Y);

            _food2 = new SnakePart(30, 30);
            _food2.Rectang.Width = _food2.Rectang.Height = 50;
            _food2.Rectang.Fill = Brushes.Blue;
            grid.Children.Add(_food2.Rectang);
            Grid.SetColumn(_food2.Rectang, _food2.X);
            Grid.SetRow(_food2.Rectang, _food2.Y);

            _food3 = new SnakePart(75, 30);
            _food3.Rectang.Width = _food3.Rectang.Height = 50;
            _food3.Rectang.Fill = Brushes.Blue;
            grid.Children.Add(_food3.Rectang);
            Grid.SetColumn(_food3.Rectang, _food3.X);
            Grid.SetRow(_food3.Rectang, _food3.Y);
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
            if (CheckCollision()) EndGame();
            else
            {
                if (CheckFood()) RedrawFood();
                _snake.RedrawSnake();
            }
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

        private void RedrawFood()
        {//RYSOWANIE JEDZENIA NA DANYCH WSPOLRZEDNYCH
            Grid.SetColumn(_food.Rectang, _food.X);
            Grid.SetRow(_food.Rectang, _food.Y);

            Grid.SetColumn(_food2.Rectang, _food2.X);
            Grid.SetRow(_food2.Rectang, _food2.Y);

            Grid.SetColumn(_food3.Rectang, _food3.X);
            Grid.SetRow(_food3.Rectang, _food3.Y);
        }

        public bool IsFieldFree(int x, int y)
        {//SPRAWDZENIE CZY POLE O DANYCH WSPOLZEDNYCH JEST PUSTE
            if (_snake.Head.X == x && _snake.Head.Y == y)
                return false;
            foreach (SnakePart snakePart in _snake.Parts)
            {
                if (snakePart.X == x && snakePart.Y == y)
                    return false;
            }
            foreach (Obstacles wall in _walls)
            {
                if (x >= wall.X && x < wall.X + wall.Width &&
                    y >= wall.Y && y < wall.Y + wall.Height)
                    return false;
            }
            return true;
        }

        public bool CheckFood()
        {
            if ((_snake.Head.X == _food.X && _snake.Head.Y == _food.Y) 
                || (_snake.Head.X == _food2.X && _snake.Head.Y == _food2.Y) 
                || (_snake.Head.X == _food3.X && _snake.Head.Y == _food3.Y))
            {
                _partsToAdd += 5;
                score++;
                SystemSounds.Beep.Play();
            }
            if (_snake.Head.X == _food.X && _snake.Head.Y == _food.Y)
            {
                for (int i = 0; i < 1; i++)
                {
                    Random rand = new Random();
                    int x = rand.Next(0, (int)(grid.Width / 10));
                    int y = rand.Next(0, (int)(grid.Height / 10));
                    if (IsFieldFree(x, y))
                    {
                        _food.X = x;
                        _food.Y = y;
                        return true;
                    }
                }
                for (int i = 0; i < grid.Width / 10; i++)
                    for (int j = 0; j < grid.Height / 10; j++)
                    {
                        if (IsFieldFree(i, j))
                        {
                            _food.X = i;
                            _food.Y = j;
                            return true;
                        }
                        EndGame();
                    }
            }
            if (_snake.Head.X == _food2.X && _snake.Head.Y == _food2.Y)
            {
                for (int i = 0; i < 1; i++)
                {
                    Random rand = new Random();
                    int x2 = rand.Next(0, (int)(grid.Width / 10));
                    int y2 = rand.Next(0, (int)(grid.Height / 10));
                    if (IsFieldFree(x2, y2))
                    {
                        _food2.X = x2;
                        _food2.Y = y2;
                        return true;
                    }
                }
                for (int i = 0; i < grid.Width / 10; i++)
                    for (int j = 0; j < grid.Height / 10; j++)
                    {
                        if (IsFieldFree(i, j))
                        {
                            _food2.X = i;
                            _food2.Y = j;
                            return true;
                        }
                        EndGame();
                    }
            }
            if (_snake.Head.X == _food3.X && _snake.Head.Y == _food3.Y)
            {
                for (int i = 0; i < 1; i++)
                {
                    Random rand = new Random();
                    int x3 = rand.Next(0, (int)(grid.Width / 10));
                    int y3 = rand.Next(0, (int)(grid.Height / 10));
                    if (IsFieldFree(x3, y3))
                    {
                        _food3.X = x3;
                        _food3.Y = y3;
                        return true;
                    }
                }
                for (int i = 0; i < grid.Width / 10; i++)
                    for (int j = 0; j < grid.Height / 10; j++)
                    {
                        if (IsFieldFree(i, j))
                        {
                            _food3.X = i;
                            _food3.Y = j;
                            return true;
                        }
                        EndGame();
                    }
            }
            return false;   
        }

        public void InitWall()
        { //PRZESZKODY
            _walls = new List<Obstacles>();
            Obstacles wall1 = new Obstacles(5, 12, 5, 40);
            grid.Children.Add(wall1.Rectang);
            Grid.SetColumn(wall1.Rectang, wall1.X);
            Grid.SetRow(wall1.Rectang, wall1.Y);
            Grid.SetColumnSpan(wall1.Rectang, wall1.Width);
            Grid.SetRowSpan(wall1.Rectang, wall1.Height);
            _walls.Add(wall1);

            Obstacles wall2 = new Obstacles(70, 12, 5, 40);
            grid.Children.Add(wall2.Rectang);
            Grid.SetColumn(wall2.Rectang, wall2.X);
            Grid.SetRow(wall2.Rectang, wall2.Y);
            Grid.SetColumnSpan(wall2.Rectang, wall2.Width);
            Grid.SetRowSpan(wall2.Rectang, wall2.Height);
            _walls.Add(wall2);

            Obstacles wall3 = new Obstacles(28, 20, 25, 5);
            grid.Children.Add(wall3.Rectang);
            Grid.SetColumn(wall3.Rectang, wall3.X);
            Grid.SetRow(wall3.Rectang, wall3.Y);
            Grid.SetColumnSpan(wall3.Rectang, wall3.Width);
            Grid.SetRowSpan(wall3.Rectang, wall3.Height);
            _walls.Add(wall3);

            Obstacles wall4 = new Obstacles(28, 35, 25, 5);
            grid.Children.Add(wall4.Rectang);
            Grid.SetColumn(wall4.Rectang, wall4.X);
            Grid.SetRow(wall4.Rectang, wall4.Y);
            Grid.SetColumnSpan(wall4.Rectang, wall4.Width);
            Grid.SetRowSpan(wall4.Rectang, wall4.Height);
            _walls.Add(wall4);

            Obstacles wall5 = new Obstacles(20, 27, 5, 5);
            grid.Children.Add(wall5.Rectang);
            Grid.SetColumn(wall5.Rectang, wall5.X);
            Grid.SetRow(wall5.Rectang, wall5.Y);
            Grid.SetColumnSpan(wall5.Rectang, wall5.Width);
            Grid.SetRowSpan(wall5.Rectang, wall5.Height);
            _walls.Add(wall5);

            Obstacles wall6 = new Obstacles(56, 27, 5, 5);
            grid.Children.Add(wall6.Rectang);
            Grid.SetColumn(wall6.Rectang, wall6.X);
            Grid.SetRow(wall6.Rectang, wall6.Y);
            Grid.SetColumnSpan(wall6.Rectang, wall6.Width);
            Grid.SetRowSpan(wall6.Rectang, wall6.Height);
            _walls.Add(wall6);

        }

       public bool CheckCollision()
        {
            if (_snake.Head.X < 0 || _snake.Head.X > grid.Width / 10)
                return true; //SPRAWDZENIE KOLIZYJNOSCI Z GRANICAMI PLANSZY
            if (_snake.Head.Y < 0 || _snake.Head.Y > grid.Height / 10)
                return true;
            foreach (SnakePart snakePart in _snake.Parts)
            {
                if (_snake.Head.X == snakePart.X && _snake.Head.Y == snakePart.Y)
                    return true; //SPRAWDZENIE KOLIZYJNOSCI WEZA SAMEGO Z SOBA
            }
            foreach (Obstacles wall in _walls)
            {
                if (_snake.Head.X >= wall.X && _snake.Head.X < wall.X + wall.Width &&
                    _snake.Head.Y >= wall.Y && _snake.Head.Y < wall.Y + wall.Height)
                    return true; //SPRAWDZENIE KOLIZYJNOSCI Z PRZESZKODAMI
            }
            return false;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {//START GRY
            _timer.Start();
            button1.Visibility = Visibility.Hidden;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {//PRZYCISK EXIT
            Environment.Exit(0);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SnakeWindow s = new SnakeWindow();
            s.Show();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {//UKRYCIE POLA TEKSTOWEGO PO PODANIU IMIENIA(PUSTE NIE UKRYWA SIE)
            if (textBox.Text != "") if (Keyboard.IsKeyDown(Key.Enter)) textBox.Visibility = Visibility.Hidden;
        }
        
        private void textBoxFocus(object sender, RoutedEventArgs e)
        {//CZYSCI POLE TEKSTOWE PO KLIKNIECIU
            textBox.Text = "";
        }

        public void rankLoad()
        {//WCZYTANIE RANKINGU Z PLIKU; CZYSZCZENIE RANKINGU POWYZEJ 10 WPISOW
            var lineCount = File.ReadAllLines("rank.txt").Length;
            if (lineCount > 10)
            {
                StreamWriter SW;
                SW = File.CreateText("rank.txt");
                SW.WriteLine("");
                SW.Close();

            }
            using (StreamReader reader = new StreamReader("rank.txt"))
            {
                textBlock3.Text = reader.ReadToEnd();
            }
        }
        private void rankLoadStart(object sender, RoutedEventArgs e)
        {
            rankLoad();
        }
        public void RankSaver()
        {//ZAPIS DO PLIKU WYNIKU (PLIK GENEROWANY W KATALOGU W KTORYM URUCHOMIONO PROGRAM)
            StreamWriter SW;
            SW = File.AppendText("rank.txt");
            SW.WriteLine("I: {0}  W: {1}pkt ", textBox.Text, score);
            SW.Close();
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
        private void EndGame()
        {
            _timer.Stop();
            button1_Copy.Visibility = Visibility.Visible;
            RankSaver();           
        }

    }
}
