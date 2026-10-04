using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Tetris
{
    internal class tetris_piece
    {
        // Define the properties and methods for a Tetris piece

        private int[,] shape; // The shape of the piece represented as a 2D array
        private int x; // X position of the piece on the game board
        private int y; // Y position of the piece on the game board
        private int width;
        private int height;

        // The shapes should be represented as 2D arrays of integers, where each integer represents a different color.
        // So for example, a square piece could be represented as:
        // 0 0 0 0
        // 0 3 3 0
        // 0 3 3 0
        // 0 0 0 0


        // Allways initialize the shape to a square piece for now. Later we will add more shapes.

        public tetris_piece()
        {
            shape = new int[4, 4]
            {
                { 0, 1, 0, 0 },
                { 0, 3, 3, 0 },
                { 0, 3, 3, 0 },
                { 0, 0, 0, 0 }
            };
            x = 3; // Start in the middle of the board
            y = 0; // Start at the top of the board
        }
        public int[,] GetShape()
        {
            return shape;
        }

        public int GetX()
        {
            return x;
        }
        public int GetY()
        {
            return y;
        }

        public void Rotate(int dir)
        {
            // Rotate the shape by the specified direction (1 for clockwise, -1 for counterclockwise)
            int rows = shape.GetLength(0); 
            int cols = shape.GetLength(1);
            int[,] rotatedShape = new int[cols, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (dir == 1)
                    {
                        // Clockwise
                        rotatedShape[j, rows - 1 - i] = shape[i, j];
                    }
                    else
                    {
                        // Counterclockwise
                        rotatedShape[cols - 1 - j, i] = shape[i, j];
                    }
                }
            }
            shape = rotatedShape;
        }
        public void Move(int dx, int dy)
        {
            x += dx;
            y += dy;
        }

        public void Draw(DrawingContext dc, int blockSize)
        {
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] != 0)
                    {
                        // Draw the block at the correct position
                        int drawX = (x + j) * blockSize;
                        int drawY = (y + i) * blockSize;
                        dc.DrawRectangle(Enums.GetColorForNumber(shape[i, j]), null, new System.Windows.Rect(drawX, drawY, blockSize, blockSize));
                    }
                }
            }
        }

    }
}
