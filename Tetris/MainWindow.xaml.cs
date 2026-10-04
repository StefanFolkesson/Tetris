using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This is a simple implementation of Tetris game using WPF. The main window contains a canvas where the game will be drawn. The constructor initializes the canvas by drawing a grid of squares, each 30x30 pixels in size, to represent the game area.
    /// When the game is running , the canvas will be updated to show the falling Tetris pieces and the current state of the game. The grid is drawn using rectangles with a light gray stroke and white fill, creating a clear visual representation of the game area.
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int BlockSize = 30; // Size of each block in pixels
        private const int Rows = 20; // Number of rows in the game area
        private const int Columns = 10; // Number of columns in the game area
        private const int NextPieceRows = 4; // Number of rows in the next piece area
        private const int NextPieceColumns = 4; // Number of columns in the next piece area
        private int[,] gameBoard = new int[Rows, Columns]; // The game board represented as a 2D array
        private tetris_piece nextPiece; // The next Tetris piece to be displayed
        private tetris_piece currentPiece; // The current Tetris piece that is falling
        private int score; // The player's score

        public MainWindow()
        {
            InitializeComponent();
            // Draw a squarenet 30x30 pixels on the canvas. 
            UpdateCanvas();
            score=0;
            nextPiece = new tetris_piece();
            currentPiece = new tetris_piece();

        }

        public void UpdateCanvas()
        {
            GameCanvas.Children.Clear();
            // Draw every square in the grid if the gameBoard says it is filled. fill it with that color
            // 1 = green, 2 = red, 3 = blue, 4 = yellow, 5 = purple, 6 = cyan, 7 = orange
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (gameBoard[row, col] != 0)
                    {
                        Rectangle rect = new Rectangle
                        {
                            Width = BlockSize,
                            Height = BlockSize,
                            Stroke = Brushes.Black,
                            Fill = Enums.GetColorForNumber(gameBoard[row, col])
                        };
                        Canvas.SetLeft(rect, col * BlockSize);
                        Canvas.SetTop(rect, row * BlockSize);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }
            ScoreLabel.Content = $"Score: {score}";
            // Draw next piece in the next piece area
            NextPieceCanvas.Children.Clear();
            if (nextPiece != null)
            {
                int[,] shape = nextPiece.GetShape();
                for (int row = 0; row < NextPieceRows; row++)
                {
                    for (int col = 0; col < NextPieceColumns; col++)
                    {
                        if (shape[row, col] != 0)
                        {
                            Rectangle rect = new Rectangle
                            {
                                Width = BlockSize,
                                Height = BlockSize,
                                Stroke = Brushes.Black,
                                Fill = Enums.GetColorForNumber(shape[row, col])
                            };
                            Canvas.SetLeft(rect, col * BlockSize);
                            Canvas.SetTop(rect, row * BlockSize);
                            NextPieceCanvas.Children.Add(rect);
                        }
                    }
                }
            }
            if (currentPiece != null)
            {
                int[,] shape = currentPiece.GetShape();
                for (int row = 0; row < shape.GetLength(0); row++)
                {
                    for (int col = 0; col < shape.GetLength(1); col++)
                    {
                        if (shape[row, col] != 0)
                        {
                            Rectangle rect = new Rectangle
                            {
                                Width = BlockSize,
                                Height = BlockSize,
                                Stroke = Brushes.Black,
                                Fill = Enums.GetColorForNumber(shape[row, col])
                            };
                            Canvas.SetLeft(rect, (currentPiece.GetX() + col) * BlockSize);
                            Canvas.SetTop(rect, (currentPiece.GetY() + row) * BlockSize);
                            GameCanvas.Children.Add(rect);
                        }
                    }
                }
            }

        }

        public void SpawnNextPiece()
        {
            currentPiece = nextPiece;
            nextPiece = new tetris_piece();
            // Reset the position of the current piece to the top of the game board
            currentPiece.Move(-currentPiece.GetX(), -currentPiece.GetY()); // Move to (0,0)
            currentPiece.Move(3, 0); // Move to the middle of the board
        }

        public void lockCurrentPiece()
        {
            // Lock the current piece into the game board
            int[,] shape = currentPiece.GetShape();
            for (int row = 0; row < shape.GetLength(0); row++)
            {
                for (int col = 0; col < shape.GetLength(1); col++)
                {
                    if (shape[row, col] != 0)
                    {
                        int boardRow = currentPiece.GetY() + row;
                        int boardCol = currentPiece.GetX() + col;
                        if (boardRow >= 0 && boardRow < Rows && boardCol >= 0 && boardCol < Columns)
                        {
                            gameBoard[boardRow, boardCol] = shape[row, col];
                        }
                    }
                }
            }
            currentPiece = null; // Clear the current piece
        }

        public void HandleGamestate()
        {
            // For now, just move the current piece down by one row
            if (currentPiece != null)
            {
                currentPiece.Move(0, 1);
                // Check if the current piece has reached the bottom or collided with another piece
                bool collision = false;
                int[,] shape = currentPiece.GetShape();
                for (int row = 0; row < shape.GetLength(0); row++)
                {
                    for (int col = 0; col < shape.GetLength(1); col++)
                    {
                        if (shape[row, col] != 0)
                        {
                            int boardRow = currentPiece.GetY() + row;
                            int boardCol = currentPiece.GetX() + col;
                            if (boardRow >= Rows || boardCol < 0 || boardCol >= Columns || gameBoard[boardRow, boardCol] != 0)
                            {
                                collision = true;
                                break;
                            }
                        }
                    }
                    if (collision) break;
                }
                if (collision)
                {
                    lockCurrentPiece();
                    SpawnNextPiece();
                }
            }
            else
            {
                SpawnNextPiece();
            }
            score++;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle key presses for moving and rotating the current piece
            if (currentPiece != null)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        currentPiece.Move(-1, 0);
                        break;
                    case Key.Right:
                        currentPiece.Move(1, 0);
                        break;
                    case Key.Down:
                        currentPiece.Move(0, 1);
                        break;
                    case Key.Up:
                        currentPiece.Rotate(1); // Rotate clockwise
                        break;
                }
                UpdateCanvas();
            }
        }



        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Start the game logic here
            // For now, just clear the game board and update the canvas
            Array.Clear(gameBoard, 0, gameBoard.Length);
            UpdateCanvas();
            // Tick the game and update every tick. For now, just update the canvas every second
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, args) =>
            {
                // For now, just update the canvas every tick
                HandleGamestate();
                UpdateCanvas();
            };
            timer.Start();

            // Listen to Keys
            this.KeyDown += Window_KeyDown;


        }
    }
}