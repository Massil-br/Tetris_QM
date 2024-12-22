

using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace Tetris_QMJ.src.Core{
    public class Grid{
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

        public void PrintGrid(){
            for (int i = 0; i < GridArray.GetLength(0); i++){
                for (int ii = 0 ; ii < GridArray.GetLength(1); ii++){
                    Console.Write(GridArray[i,ii]);
                }
                Console.Write("\n");
            }
        }

        public bool AddPiece(Piece piece)
        {
            for (int i = 0; i < piece.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < piece.Shape.GetLength(1); j++)
                {
                    if (piece.Shape[i, j] == 1) // Si c'est une partie de la pièce
                    {
                        int gridX = piece.X + i;
                        int gridY = piece.Y + j;

                        // Vérification des limites et des collisions
                        if (gridX >= GridArray.GetLength(0) || gridY >= GridArray.GetLength(1) || gridX < 0 || gridY < 0 || GridArray[gridX, gridY] != 0)
                        {
                            return false; // Collision ou hors limites
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
                        GridArray[piece.X + i, piece.Y + j] = piece.Id; // Ajout du ID de la pièce
                    }
                }
            }

            return true; // Pièce ajoutée avec succès
        }


        //ClearFullLines func permet de supprimer une ligne si elle est complete de gauche a droite dans le grid
        public void ClearFullLines(){
            for (int i = GridArray.GetLength(0) -1 ; i >= 0 ; i--){
                bool fullLine = true;

                for (int ii = 0; ii < GridArray.GetLength(1); ii++){
                    if (GridArray[i,ii]== 0){
                        fullLine = false;
                        break;
                    }
                }

                if (fullLine){
                    for (int row = i; row > 0 ; row --){
                        for (int col = 0; col < GridArray.GetLength(1); col ++){
                            GridArray[row,col] = GridArray[row -1, col];
                        }
                    }

                    for (int col = 0; col < GridArray.GetLength(1); col++){
                        GridArray[0,col]= 0;
                    }
                    i++;
                }

            }
        }
    }
}