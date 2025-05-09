using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private const int TileSize = 60;
        private List<Point> snake = new List<Point>();
        private Point food;
        private Vector direction = new Vector(1, 0);
        private Vector newDirection = new Vector(1, 0); // Новое направление
        private DispatcherTimer gameTimer;
        private bool isGameOver = false;
        private int lastSpeedIncreaseLength = 0;
        private const int MinIntervalMs = 50; // Минимальная скорость
        private const int SpeedIncreaseStep = 10;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => InitializeGame();
        }

        private void InitializeGame()
        {
            // Сброс состояния игры
            snake.Clear();
            snake.Add(new Point(5, 0));
            snake.Add(new Point(4, 0));
            snake.Add(new Point(3, 0));

            direction = new Vector(1, 0);
            newDirection = new Vector(1, 0);
            isGameOver = false;

            GenerateFood();

            if (gameTimer != null)
            {
                gameTimer.Tick -= GameLoop;
                gameTimer.Stop();
            }

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(150);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            GameCanvas.Focus(); // Важно для получения событий клавиатуры
        }

        private void GameLoop(object sender, EventArgs e)
        {
            direction = newDirection; // Обновляем направление

            MoveSnake();

            if (CheckCollision())
            {
                gameTimer.Stop();
                isGameOver = true;
                MessageBox.Show("Игра окончена! Нажмите Пробел для рестарта.");
                return;
            }

            DrawGame();
        }

        private void MoveSnake()
        {
            Point newHead = new Point(
                snake[0].X + direction.X,
                snake[0].Y + direction.Y
            );

            snake.Insert(0, newHead);

            if (newHead == food)
            {
                GenerateFood();

                // Ускорение только когда змейка выросла на 5 с последнего ускорения
                if (snake.Count >= lastSpeedIncreaseLength + 5)
                {
                    double newInterval = gameTimer.Interval.TotalMilliseconds - SpeedIncreaseStep;
                    if (newInterval >= MinIntervalMs)
                    {
                        gameTimer.Interval = TimeSpan.FromMilliseconds(newInterval);
                        lastSpeedIncreaseLength = snake.Count;
                    }
                }
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        private void GenerateFood()
        {
            Random rand = new Random();
            int maxX = Math.Max(1, (int)(GameCanvas.ActualWidth / TileSize) - 1);
            int maxY = Math.Max(1, (int)(GameCanvas.ActualHeight / TileSize) - 1);

            do
            {
                food = new Point(
                    rand.Next(0, maxX),
                    rand.Next(0, maxY)
                );
            }
            while (snake.Contains(food));
        }

        private bool CheckCollision()
        {
            Point head = snake[0];

            // Границы поля
            if (head.X < 0 || head.Y < 0 ||
                head.X >= GameCanvas.ActualWidth / TileSize ||
                head.Y >= GameCanvas.ActualHeight / TileSize)
                return true;

            // Столкновение с собой (пропускаем голову)
            for (int i = 1; i < snake.Count; i++)
            {
                if (head == snake[i])
                    return true;
            }

            return false;
        }

        private void DrawGame()
        {
            GameCanvas.Children.Clear();

            // Отрисовка серой сетки поля
            DrawGrid();

            // Отрисовка змейки (без обводки)
            foreach (Point segment in snake)
            {
                Rectangle rect = new Rectangle
                {
                    Width = TileSize,
                    Height = TileSize,
                    Fill = Brushes.Green // Зеленые без обводки
                };
                Canvas.SetLeft(rect, segment.X * TileSize);
                Canvas.SetTop(rect, segment.Y * TileSize);
                GameCanvas.Children.Add(rect);
            }

            // Отрисовка еды (без обводки)
            Ellipse foodEllipse = new Ellipse
            {
                Width = TileSize,
                Height = TileSize,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(foodEllipse, food.X * TileSize);
            Canvas.SetTop(foodEllipse, food.Y * TileSize);
            GameCanvas.Children.Add(foodEllipse);
        }

        private void DrawGrid()
        {
            int columns = (int)(GameCanvas.ActualWidth / TileSize);
            int rows = (int)(GameCanvas.ActualHeight / TileSize);

            // Вертикальные линии
            for (int x = 0; x <= columns; x++)
            {
                Line line = new Line
                {
                    X1 = x * TileSize,
                    Y1 = 0,
                    X2 = x * TileSize,
                    Y2 = rows * TileSize,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                GameCanvas.Children.Add(line);
            }

            // Горизонтальные линии
            for (int y = 0; y <= rows; y++)
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = y * TileSize,
                    X2 = columns * TileSize,
                    Y2 = y * TileSize,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                GameCanvas.Children.Add(line);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up when direction.Y == 0:
                    newDirection = new Vector(0, -1);
                    break;
                case Key.Down when direction.Y == 0:
                    newDirection = new Vector(0, 1);
                    break;
                case Key.Left when direction.X == 0:
                    newDirection = new Vector(-1, 0);
                    break;
                case Key.Right when direction.X == 0:
                    newDirection = new Vector(1, 0);
                    break;
                case Key.Space when isGameOver:
                    InitializeGame();
                    break;
            }
        }
    }
}