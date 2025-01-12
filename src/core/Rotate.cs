using System;
using Raylib_cs;
using Tetris_QMJ.src.Audio;

namespace Tetris_QMJ.src.Core {
    public class Rotation {
        private Grid grid;
        Options options = new Options();
        private Entities.Piece piece;

        public Rotation(Grid grid)
        {
            this.grid = grid;
        }

        public Rotation (Grid grid, Rotation rotation){
            this.grid = grid;
            
        }

        public void HandleInput() {
            // Vérifie si la touche de rotation est pressée
            if (Raylib.IsKeyPressed(options.KeyBindings["Rotate"])) 
            {
                AudioGame.PlaySound(AudioGame.soundPieceRotate);
                RotatePiece();
            }
        }

        //permet de faire la rotation de la pièce
        public void RotatePiece() {
            piece = grid.GetPiece();

            // vérifie si la pièce est active et la retire de la grille pour la tourner puis la remet dans la grille
            if (piece != null && piece.IsActive) { 
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
