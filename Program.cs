using System;
using Tetris_QMJ.src.Core;
namespace Tetris_QMJ
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new();
            Piece piece = new Piece(1, 0, 3, new int[,] 
            { 
                { 1, 1, 1, 1 } // Une pièce I horizontale
            });
            grid.PrintGrid();
            Console.WriteLine("\n");
            if (grid.AddPiece(piece))
            {
                Console.WriteLine("\n✅");
            }
            else
            {
                Console.WriteLine("\n❌");
            }
            Console.WriteLine("\nGrid with piece control gg ez");
            grid.PrintGrid();
            Game.InitWindow();
        }
    }
}
