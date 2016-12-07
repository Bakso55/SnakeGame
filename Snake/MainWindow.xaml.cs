using System;
using System.Windows;


namespace Snake
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SnakeWindow s = new SnakeWindow();
            s.Show();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);

        }
    }
}
