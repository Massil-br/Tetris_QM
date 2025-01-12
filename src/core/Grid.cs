
using System;
using System.Collections.Generic;
using Raylib_cs;
using Tetris_QMJ.src.Entities;
using Tetris_QMJ.src.Audio;

namespace Tetris_QMJ.src.Core{
    public class Grid{
        static Color Cyan = new Color(0, 255, 255, 255);    // Pièce I
        static Color Yellow = new Color(255, 255, 0, 255);  // Pièce O
        static Color Purple = new Color(128, 0, 128, 255);  // Pièce T
        static Color Green = new Color(0, 255, 0, 255);     // Pièce S
        static Color Red = new Color(255, 0, 0, 255);       // Pièce Z
        static Color Blue = new Color(0, 0, 255, 255);      // Pièce J
        static Color Orange = new Color(255, 165, 0, 255);  // Pièce L

        // Dictionnaire associant chaque ID de pièce à sa couleur
        static Dictionary<int, Color> pieceColors = new Dictionary<int, Color>
        {
            { 1, Cyan },    // Pièce I
            { 2, Yellow },  // Pièce O
            { 3, Purple },  // Pièce T
            { 4, Green },   // Pièce S
            { 5, Red },     // Pièce Z
            { 6, Blue },    // Pièce J
            { 7, Orange }   // Pièce L
        };
        public Piece ActivePiece;
        public Piece NextPiece;

        public int[,] GridArray{get;set;}
        public int LigneComplet{get;set;} = 0;
        public int Score{get;set;} = 0;
        private int[,] InitGrid(int width, int heigth) {
            int[,] array = new int[width,heigth];
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int ii = 0; ii < array.GetLength(1); ii++) {
                    array[i,ii] = 0;
                }
            }
            return array;
        }

        public Grid(int heigth , int width ){
            GridArray = InitGrid(heigth,width);
        }

        public void PrintGrid(int gridRows, int gridColumns, int offsetX, int offsetY, int cellSize, Piece nextPiece){
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            for (int row = 0; row < gridRows; row++){
                for (int col = 0; col < gridColumns; col++){
                    int cellValue = GridArray[row, col];
                    int x = offsetX + (col * cellSize);
                    int y = offsetY + (row * cellSize);

                    // Si la cellule n'est pas vide, colorie en fonction de l'ID
                    if (cellValue != 0)
                    {
                        // Récupère la couleur en fonction de l'ID, sinon utilise une couleur par défaut
                        Color pieceColor = pieceColors.ContainsKey(cellValue) ? pieceColors[cellValue] : Color.White;
                        Raylib.DrawRectangle(x, y, cellSize, cellSize, pieceColor);
                    }
                    int borderX = offsetX + (col * cellSize);
                    int borderY = offsetY + (row * cellSize);
                    Raylib.DrawRectangleLines(borderX, borderY, cellSize, cellSize, Color.Gray);
                }
            }
            Raylib.DrawText($"Lignes complétées : {LigneComplet}", 10, 60, 19, Color.White);
            Raylib.DrawText($"Score : {Score}", 10, 80, 19, Color.White);
            Raylib.DrawText("Prochaine pièce :", offsetX + (gridColumns * cellSize) + 20, offsetY, 19, Color.White);

            // Dessine un cadre pour la prochaine pièce
            int previewX = offsetX + (gridColumns * cellSize) + 40;  // Décalage à droite de la grille
            int previewY = offsetY + 30;  // Ajuste la hauteur

            // Dessine le cadre autour de la zone de la prochaine pièce
            Raylib.DrawRectangleLines(previewX - 10, previewY - 10, cellSize * 4, cellSize * 4, Color.White);

            // Affiche la prochaine pièce bien centrée avec la bonne couleur
            DrawNextPiece(nextPiece, previewX, previewY, cellSize);

            Raylib.EndDrawing();    
        }

        //ajoute une piece a la grille et verifie si elle est en collision avec une autre piece
        public bool AddPiece(Piece piece)
        {
            for (int i = 0; i < piece.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < piece.Shape.GetLength(1); j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        int gridX = piece.X + i;
                        int gridY = piece.Y + j;

                        // Vérification des limites et des collisions
                        if (gridX >= GridArray.GetLength(0) || gridY >= GridArray.GetLength(1) || gridX < 0 || gridY < 0 || GridArray[gridX, gridY] != 0)
                        {
                            return false;
                        }
                    }
                }
            }

            // Si aucune collision, placer la pièce
            for (int i = 0; i < piece.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < piece.Shape.GetLength(1); j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        GridArray[piece.X + i, piece.Y + j] = piece.Id;
                    }
                }
            }

            return true;
        }

        // Supprimer une pièce de la grille
        public void RemovePiece(Entities.Piece piece)
        {
            for (int i = 0; i < piece.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < piece.Shape.GetLength(1); j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        GridArray[piece.X + i, piece.Y + j] = 0;
                    }
                }
            }
        }


        //ClearFullLines func permet de supprimer une ligne si elle est complete de gauche a droite dans le grid
        public void ClearFullLines()
        {   
            int nbLigne = 0;
            for (int i = GridArray.GetLength(0) -1 ; i >= 0 ; i--)
            {
                bool fullLine = true;

                for (int ii = 0; ii < GridArray.GetLength(1); ii++)
                {
                    if (GridArray[i,ii]== 0)
                    {
                        fullLine = false;
                        break;
                    }
                }

                //si la ligne est complete on supprime la ligne et on decale les lignes du dessus vers le bas
                if (fullLine)
                {   
                    nbLigne++;
                    AudioGame.PlaySound(AudioGame.soundClearLineGrid);

                    for (int row = i; row > 0 ; row --)
                    {
                        for (int col = 0; col < GridArray.GetLength(1); col ++)
                        {
                            GridArray[row,col] = GridArray[row -1, col];
                        }
                    }

                    for (int col = 0; col < GridArray.GetLength(1); col++)
                    {
                        GridArray[0,col]= 0;
                    }
                    i++;
                }

            }
            //ajoute le nombre de ligne completé
            LigneComplet += nbLigne;

            //ajoute 100 points par ligne complete
            Score += nbLigne * 100;
        }

        //retourne la piece active dans la grille
        public Entities.Piece GetPiece() {
            return ActivePiece;
        }

        //definit une nouvelle piece comme etant la piece active
        public void SetActivePiece(Piece piece) {
            ActivePiece = piece;
        }

        //definit la prochaine piece
        public void SetNextPiece(Piece piece) {
            NextPiece = piece;
        }

        //dessine la prochaine piece
        void DrawNextPiece(Piece nextPiece, int previewX, int previewY, int cellSize)
        {
            int[,] shape = nextPiece.Shape; 
            int rows = shape.GetLength(0);
            int cols = shape.GetLength(1);

            // Récupère la couleur de la pièce en fonction de son ID
            Color pieceColor = pieceColors.ContainsKey(nextPiece.Id) ? pieceColors[nextPiece.Id] : Color.White;

            // Calcul pour centrer la pièce dans la zone de prévisualisation
            int startX = previewX + (2 * cellSize) - ((cols * cellSize) / 2); // Centre horizontalement
            int startY = previewY + (2 * cellSize) - ((rows * cellSize) / 2); // Centre verticalement

            // Dessine chaque bloc de la pièce avec la bonne couleur
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (shape[row, col] == 1)
                    {
                        Raylib.DrawRectangle(
                            startX + (col * cellSize),
                            startY + (row * cellSize),
                            cellSize, cellSize,
                            pieceColor
                        );
                        Raylib.DrawRectangleLines(
                            startX + (col * cellSize),
                            startY + (row * cellSize),
                            cellSize, cellSize,
                            Color.Black
                        );
                    }
                }
            }
        }

    }
}