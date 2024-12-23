using System;
using System.Collections.Generic;

namespace Tetris_QMJ.src.Entities
{
    public class Piece
    {
        public int Id { get; set;}
        public int X { get; set;}
        public int Y { get; set;}
        public int[,] Shape { get; set; }

        public Piece(int id, int x, int y, int[,] shape)
        {
            Id = id;
            X = x;
            Y = y;
            Shape = shape;
        }
    }

    public static class PieceFactory
    {
        private static readonly Random RandomGenerator = new Random(); // Générateur aléatoire

        public static List<Piece> GeneratePieces()
        {
            return new List<Piece>
            {
                new Piece(1, 0, 0, new int[,] { { 1, 1, 1, 1 } }), // I
                new Piece(2, 0, 0, new int[,] { { 1, 1 }, { 1, 1 } }), // O
                new Piece(3, 0, 0, new int[,] { { 0, 1, 0 }, { 1, 1, 1 } }), // T
                new Piece(4, 0, 0, new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }), // S
                new Piece(5, 0, 0, new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }), // Z
                new Piece(6, 0, 0, new int[,] { { 1, 0, 0 }, { 1, 1, 1 } }), // J
                new Piece(7, 0, 0, new int[,] { { 0, 0, 1 }, { 1, 1, 1 } })  // L
            };
        }

        public static Piece GenerateRandomPiece(int largeur)
        {
            List<Piece> pieces = GeneratePieces();
            int randomIndex = RandomGenerator.Next(pieces.Count); // Sélection aléatoire
            Piece selectedPiece = pieces[randomIndex];

            // Retourner une copie pour éviter les modifications accidentelles
            return new Piece(
                selectedPiece.Id,
                selectedPiece.X,
                selectedPiece.Y,
                selectedPiece.Shape
            );
        }
    }
}
