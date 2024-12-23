using System;
using Raylib_cs;
using Tetris_QMJ.src.Interfaces;

namespace Tetris_QMJ.src.Core {
    public class Move {
        private Grid grid;
        private Entities.Piece piece;
        private float timer;
        private float intervalMove = 0.8f; 

        public Move(Grid grid) {
            this.grid = grid;
        }

        public void MoveDown() {
            if (piece == null) {
                throw new InvalidOperationException("Piece is null in MoveDown");
            }

            grid.RemovePiece(piece);

            piece.X += 1;

            // Vérifie les collisions en bas de la grille
            if (!grid.AddPiece(piece)) {
                piece.X -= 1; 
                grid.AddPiece(piece);

                grid.ClearFullLines();

                piece = Entities.PieceFactory.GenerateRandomPiece(grid.GridArray.GetLength(1));
            } else {

                grid.AddPiece(piece);
            }
        }


        // Gère les entrées du joueur
        public void HandleInput() {
            if (Raylib.IsKeyPressed(KeyboardKey.Left)) {
                MoveLeft();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Right)) {
                MoveRight();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Down)) {
                MoveDown();
            }
        }

        public void MoveLeft() {
            if (piece == null) {
                throw new InvalidOperationException("Piece is null in MoveLeft");
            }

            piece.Y -= 1;

            if (!grid.AddPiece(piece)) {
                piece.Y += 1; 
            }
        }

        public void MoveRight() {
            if (piece == null) {
                throw new InvalidOperationException("Piece is null in MoveRight");
            }

            piece.Y += 1;

            if (!grid.AddPiece(piece)) {
                piece.Y -= 1; 
            }
        }

        public void UpdateTimer(float deltaTime) {
            timer += deltaTime;

            if (timer >= intervalMove) {
                MoveDown();
                timer = 0f;
            }
        }

        public void SetPiece(Entities.Piece newPiece) {
            this.piece = newPiece;
        }

    }
}
