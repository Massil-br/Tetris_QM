using System;
using Tetris_QMJ.src.Core;
namespace Tetris_QMJ
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new();
            grid.PrintGrid();
            Console.WriteLine("\n");
            grid.AddFullLine();
            grid.PrintGrid();
            Console.WriteLine("\n");
            grid.ClearFullLines();
            grid.PrintGrid();
            Game.InitWindow();
        }
    }
}
