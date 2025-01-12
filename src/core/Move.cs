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
            // vérifie si la pièce est nulle ou non
            if (piece == null) {
                throw new InvalidOperationException("Piece is null in MoveDown");
            }

            grid.RemovePiece(piece);

            // vérifie si la pièce est en dehors de la grille en bas
            piece.X += 1;

            // vérifie les collisions en bas de la grille
            if (!grid.AddPiece(piece)) {
                piece.X -= 1; 
                grid.AddPiece(piece);
                piece.IsActive = false;
                grid.ClearFullLines();
                grid.ActivePiece = grid.NextPiece;
                grid.NextPiece = Entities.PieceFactory.GenerateRandomPiece(grid.GridArray.GetLength(1));
                SetPiece(grid.ActivePiece);
            } else {
                grid.AddPiece(piece);
            }
        }

        // permet de gerer la vitesse de la pièce en fonction des entreés clavier du joueur
        public void HandleInput(float deltaTime) {

            timeGen.UpdateTimer();

            // verifie si on doit changer la vitesse de la pièce
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
            // vérifie si la pièce est nulle ou non
            if (piece == null) {
                throw new InvalidOperationException("Piece is null in MoveLeft");
            }

            // vérifie si la pièce est en dehors de la grille
            if (piece.Y != 0) {
                grid.RemovePiece(piece);
            }

            piece.Y -= 1;

            // vérifie les collisions à gauche de la grille
            if (!grid.AddPiece(piece)) {
                piece.Y += 1; 
            } else {
                grid.AddPiece(piece);
            }
        }

        public void MoveRight() {
            // vérifie si la pièce est nulle ou non
            if (piece == null) {
                throw new InvalidOperationException("Piece is null in MoveRight");
            }

            // vérifie si la pièce est en dehors de la grille à droite
            if (piece.Y != grid.GridArray.GetLength(1) - 1) {
                grid.RemovePiece(piece);
            }

            piece.Y += 1;

            // vérifie les collisions à droite de la grille
            if (!grid.AddPiece(piece)) {
                piece.Y -= 1; 
            } else {
                grid.AddPiece(piece);
            }
        }

        //met a jour le timer de l intervalle de temps pour la descente de la pièce
        public void UpdateTimer(float deltaTime) {
            timer += deltaTime;

            if (timer >= intervalMove) {
                MoveDown();
                timer = 0f;
            }
        }

        //permet de changer la pièce active
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
