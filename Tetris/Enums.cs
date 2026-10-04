using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Tetris
{
    internal class Enums
    {

        public static Brush GetColorForNumber(int v)
        {
            return v switch
            {
                1 => Brushes.Green,
                2 => Brushes.Red,
                3 => Brushes.Blue,
                4 => Brushes.Yellow,
                5 => Brushes.Purple,
                6 => Brushes.Cyan,
                7 => Brushes.Orange,
                _ => Brushes.White
            };
        }
    }
}
