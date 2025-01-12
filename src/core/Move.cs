using System;
using Raylib_cs;
using Tetris_QMJ.src.Audio;

namespace Tetris_QMJ.src.Core {
    public class Move {
        private Grid grid;
        Options options = new Options();
        private Entities.Piece piece;
        private float timer;
        private float intervalMove = 0.8f;

        // Délai pour chaque contrôle utilisateur
        private float controlIntervalTimer = 0f;

        // Intervalles pour les contrôles utilisateurs
        private const float controlInterval = 0.1f;

        private const float maxSpeed = 0.2f;

        private Timer timeGen;
        private int lastMinForSpeed = 0;

        public Move(Grid grid) {
            this.grid = grid;
            this.timeGen = new Timer();
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
                piece.IsActive = false;
                grid.ClearFullLines();
                Entities.Piece ActivePiece = Entities.PieceFactory.GenerateRandomPiece(grid.GridArray.GetLength(1));
                grid.AddPiece(ActivePiece);
                grid.SetActivePiece(ActivePiece);
                SetPiece(ActivePiece);
            } else {
                grid.AddPiece(piece);
            }
        }

        public void HandleInput(float deltaTime) {

            timeGen.UpdateTimer();

            int minutes = (int)(timeGen.Rapidite()/60);
            if (minutes > lastMinForSpeed){
                Speed();
                lastMinForSpeed = minutes;
            }
            // Incrémentation des timers
            controlIntervalTimer += deltaTime;

            // Déplacement à gauche
            if (Raylib.IsKeyDown(options.KeyBindings["MoveLeft"]) && controlIntervalTimer >= controlInterval) {
                // AudioGame.PlaySound(AudioGame.soundPieceMove);
                MoveLeft();
                controlIntervalTimer = 0f;
            }

            // Déplacement à droite
            if (Raylib.IsKeyDown(options.KeyBindings["MoveRight"]) && controlIntervalTimer >= controlInterval) {
                // AudioGame.PlaySound(AudioGame.soundPieceMove);
                MoveRight();
                controlIntervalTimer = 0f;
            }

            // Déplacement vers le bas
            if (Raylib.IsKeyDown(options.KeyBindings["MoveDown"]) && controlIntervalTimer >= controlInterval) {
                // AudioGame.PlaySound(AudioGame.soundPieceMove);
                MoveDown();
                controlIntervalTimer = 0f;
            }
        }

        public void MoveLeft() {
            if (piece == null) {
                throw new InvalidOperationException("Piece is null in MoveLeft");
            }
            if (piece.Y != 0) {
                grid.RemovePiece(piece);
            }

            piece.Y -= 1;

            if (!grid.AddPiece(piece)) {
                piece.Y += 1; 
            } else {
                grid.AddPiece(piece);
            }
        }

        public void MoveRight() {
            if (piece == null) {
                throw new InvalidOperationException("Piece is null in MoveRight");
            }
            if (piece.Y != grid.GridArray.GetLength(1) - 1) {
                grid.RemovePiece(piece);
            }

            piece.Y += 1;

            if (!grid.AddPiece(piece)) {
                piece.Y -= 1; 
            } else {
                grid.AddPiece(piece);
            }
        }

        public void UpdateTimer(float deltaTime) {
            timer += deltaTime;

            if (timer >= intervalMove) {
                MoveDown();
                timer = 0f;
            }
        }

        public void SetPiece(Entities.Piece ActivePiece) {
            this.piece = ActivePiece;
            grid.SetActivePiece(ActivePiece);

            // Réinitialise les délais pour la nouvelle pièce
            controlIntervalTimer = 0f;
        }

        //changement de vitesse minute en minute
        public void Speed(){
            if (intervalMove > maxSpeed){
                intervalMove -= 0.1f;
                if (intervalMove < maxSpeed){
                    intervalMove = maxSpeed;
                }
            }
        }
    }
}
