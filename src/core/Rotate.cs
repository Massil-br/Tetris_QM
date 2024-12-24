using System;
using Raylib_cs;

namespace Tetris_QMJ.src.Core {
    public class Rotation {
        private Grid grid;
        private Rotation rotation;
        private Entities.Piece piece;

        public Rotation(Grid grid)
        {
            this.grid = grid;
        }

        public Rotation (Grid grid, Rotation rotation){
            this.grid = grid;
            this.rotation = rotation;
        }

        public void HandleInput() {
            // Vérifie si la barre d'espace est pressée
            if (Raylib.IsKeyPressed(KeyboardKey.Up)) {
                RotatePiece();
            }
        }

        public void RotatePiece() {
            piece = grid.GetPiece();

            if (piece != null && piece.IsActive) { // Vérifiez si la pièce est active
                grid.RemovePiece(piece); 
                piece.Rotation90();

                if (!grid.AddPiece(piece)) {
                    // Annule la rotation en cas de collision ou de dépassement
                    piece.Rotation90();
                    piece.Rotation90();
                    piece.Rotation90();
                    grid.AddPiece(piece);
                }
            }
        }
    }
}
