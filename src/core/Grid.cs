
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
        private Piece ActivePiece;

        public int[,] GridArray{get;set;}
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

        public void PrintGrid(int gridRows, int gridColumns, int offsetX, int offsetY, int cellSize){
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
            Raylib.EndDrawing();
        }

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

                if (fullLine)
                {   
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
        }

        public void Update(Piece piece){
            RemovePiece(piece);
            if (!AddPiece(piece)){
                throw new InvalidOperationException("Impossible d'ajouter la pièce après rotation.");
            }
        }

        public Entities.Piece GetPiece() {
            return ActivePiece;
        }

        public void SetActivePiece(Piece piece) {
            ActivePiece = piece;
        }
    }
}