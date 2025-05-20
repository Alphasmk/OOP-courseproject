using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using gamelauncher.Model;
using gamelauncher.MVVM;

namespace gamelauncher.Views
{
    public partial class SnakePage : Page, INotifyPropertyChanged
    {
        private const int TileSize = 31;
        private const int GridWidth = 30;
        private const int GridHeight = 20;
        private string _recordTitle;
        private string _currentScoreTitle;

        private int _snakeRecord;
        public int SnakeRecord
        {
            get => _snakeRecord;
            set
            {
                _snakeRecord = value;
                OnPropertyChanged();
            }
        }

        public string RecordTitle
        {
            get => _recordTitle;
            set
            {
                _recordTitle = value;
                OnPropertyChanged();
            }
        }

        public string CurrentScoreTitle
        {
            get => _currentScoreTitle;
            set
            {
                _currentScoreTitle = value;
                OnPropertyChanged();
            }
        }

        private int _currentScore;
        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnPropertyChanged();
            }
        }

        private List<Point> snake = new List<Point>();
        private Point food;
        private Vector direction = new Vector(1, 0);
        private Vector newDirection = new Vector(1, 0);
        private DispatcherTimer gameTimer;
        private bool isGameOver = false;
        private bool isGameRunning = false; 
        private int lastSpeedIncreaseLength = 0;
        private const int MinIntervalMs = 50;
        private const int SpeedIncreaseStep = 10;

        public ICommand StartCommand { get; }

        public SnakePage()
        {
            InitializeComponent();
            CurrentScore = 0;
            if(LanguageManager.CurrentLanguage == "ru-RU")
            {
                CurrentScoreTitle = "Текущий счет: " + CurrentScore.ToString();
            }
            else
            {
                CurrentScoreTitle = "Current score: " + CurrentScore.ToString();
            }
            DataContext = this;
            SnakeRecord = CurrentUser.Instance.SnakeRecord;
            GameCanvas.Focusable = true;
            GameCanvas.Focus();

            GameCanvas.KeyDown += GameCanvas_KeyDown;
            this.PreviewKeyDown += SnakePage_PreviewKeyDown;

            Loaded += (s, e) =>
            {
                GameCanvas.Width = GridWidth * TileSize;
                GameCanvas.Height = GridHeight * TileSize;
                DrawGrid();
            };

            this.Unloaded += (s, e) => Cleanup();

            SnakeRecord = CurrentUser.Instance.SnakeRecord;
            if (LanguageManager.CurrentLanguage == "ru-RU")
            {
                RecordTitle = "Рекорд: " + SnakeRecord.ToString();
            }
            else
            {
                RecordTitle = "Record: " + SnakeRecord.ToString();
            }
            StartCommand = new RelayCommand(_ => StartGame());
        }

        private void StartGame()
        {
                InitializeGame();
                isGameRunning = true;
        }

        public void Cleanup()
        {
            if (gameTimer != null)
            {
                gameTimer.Stop();
                gameTimer.Tick -= GameLoop;
                gameTimer = null;
            }

            GameCanvas.KeyDown -= GameCanvas_KeyDown;
            this.PreviewKeyDown -= SnakePage_PreviewKeyDown;

            snake.Clear();
            GameCanvas.Children.Clear();
        }

        private void InitializeGame()
        {
            CurrentScore = 0;
            if (LanguageManager.CurrentLanguage == "ru-RU")
            {
                CurrentScoreTitle = "Текущий счет: " + CurrentScore.ToString();
            }
            else
            {
                CurrentScoreTitle = "Current score: " + CurrentScore.ToString();
            }
            snake.Clear();
            snake.Add(new Point(5, 0));
            snake.Add(new Point(4, 0));
            snake.Add(new Point(3, 0));

            direction = new Vector(1, 0);
            newDirection = new Vector(1, 0);
            isGameOver = false;
            isGameRunning = true;

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

            GameCanvas.Focus();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            direction = newDirection;

            MoveSnake();

            if (CheckCollision())
            {
                gameTimer.Stop();
                isGameOver = true;
                isGameRunning = false;
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError err = new RegisterError("Игра окончена!");
                    err.ShowDialog();
                }
                else
                {
                    RegisterError err = new RegisterError("Game Over!");
                    err.ShowDialog();
                }
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
                CurrentScore++;

                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    CurrentScoreTitle = "Текущий счет: " + CurrentScore.ToString();
                }
                else
                {
                    CurrentScoreTitle = "Current score: " + CurrentScore.ToString();
                }

                if (snake.Count >= lastSpeedIncreaseLength + 10)
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

            do
            {
                food = new Point(
                    rand.Next(0, GridWidth),
                    rand.Next(0, GridHeight)
                );
            }
            while (snake.Contains(food));
        }

        private bool CheckCollision()
        {
            Point head = snake[0];

            if (head.X < 0 || head.Y < 0 ||
                head.X >= GridWidth ||
                head.Y >= GridHeight)
            {
                if (CurrentUser.Instance.SnakeRecord < snake.Count())
                {
                    SnakeRecord = snake.Count() - 3;
                    CurrentUser.Instance.SnakeRecord = snake.Count() - 3;
                    DataWorker.UpdateUser(CurrentUser.Instance);
                    if (LanguageManager.CurrentLanguage == "ru-RU")
                    {
                        RecordTitle = "Рекорд: " + SnakeRecord.ToString();
                    }
                    else
                    {
                        RecordTitle = "Record: " + SnakeRecord.ToString();
                    }
                }
                return true;
            }

            for (int i = 1; i < snake.Count; i++)
            {
                if (head == snake[i])
                {
                    if(CurrentUser.Instance.SnakeRecord < snake.Count() - 3)
                    {
                        SnakeRecord = snake.Count() - 3;
                        CurrentUser.Instance.SnakeRecord = snake.Count() - 3;
                        DataWorker.UpdateUser(CurrentUser.Instance);
                    }
                    return true;
                }   
            }

            return false;
        }

        private void DrawGame()
        {
            GameCanvas.Children.Clear();

            DrawGrid();

            foreach (Point segment in snake)
            {
                Rectangle rect = new Rectangle
                {
                    Width = TileSize,
                    Height = TileSize,
                    Fill = Brushes.Green
                };
                Canvas.SetLeft(rect, segment.X * TileSize);
                Canvas.SetTop(rect, segment.Y * TileSize);
                GameCanvas.Children.Add(rect);
            }

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
            for (int x = 0; x <= GridWidth; x++)
            {
                Line line = new Line
                {
                    X1 = x * TileSize,
                    Y1 = 0,
                    X2 = x * TileSize,
                    Y2 = GridHeight * TileSize,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                GameCanvas.Children.Add(line);
            }

            for (int y = 0; y <= GridHeight; y++)
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = y * TileSize,
                    X2 = GridWidth * TileSize,
                    Y2 = y * TileSize,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                GameCanvas.Children.Add(line);
            }
        }

        private void GameCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKey(e);
            e.Handled = true;
        }

        private void SnakePage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            HandleKey(e);
        }

        private void HandleKey(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up when direction.Y == 0:
                    newDirection = new Vector(0, -1);
                    e.Handled = true;
                    break;
                case Key.W when direction.Y == 0:
                    newDirection = new Vector(0, -1);
                    e.Handled = true;
                    break;
                case Key.Down when direction.Y == 0:
                    newDirection = new Vector(0, 1);
                    e.Handled = true;
                    break;
                case Key.S when direction.Y == 0:
                    newDirection = new Vector(0, 1);
                    e.Handled = true;
                    break;
                case Key.Left when direction.X == 0:
                    newDirection = new Vector(-1, 0);
                    e.Handled = true;
                    break;
                case Key.A when direction.X == 0:
                    newDirection = new Vector(-1, 0);
                    e.Handled = true;
                    break;
                case Key.Right when direction.X == 0:
                    newDirection = new Vector(1, 0);
                    e.Handled = true;
                    break;
                case Key.D when direction.X == 0:
                    newDirection = new Vector(1, 0);
                    e.Handled = true;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
