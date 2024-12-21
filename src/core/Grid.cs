

using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace Tetris_QMJ.src.Core{
    public class Grid{
        public int[,] GridArray{get;set;}
        private int[,] InitGrid(int width, int heigth){
            int[,] array = new int[width,heigth];
            for (int i = 0; i < array.GetLength(0); i++){
                for (int ii = 0; ii < array.GetLength(1); ii++){
                    array[i,ii] = 0;
                }
            }
            return array;
        }

        public Grid(){
            GridArray = InitGrid(30,10);
        }

        public void PrintGrid(){
            for (int i = 0; i < GridArray.GetLength(0); i++){
                for (int ii = 0 ; ii < GridArray.GetLength(1); ii++){
                    Console.Write(GridArray[i,ii]);
                }
                Console.Write("\n");
            }
        }

        // func de teste pour remplir vite fait le tableau et tester le clearFulllines
        public void AddFullLine(){
            for (int i = 0; i < GridArray.GetLength(0); i++){
                for (int ii = 0 ; ii < GridArray.GetLength(1); ii++){
                    if ((i == 2 && ii > 6) || (i == 20 && ii > 8) || ( i == 4 && ii > 5) || (i == 28 && ii > 7) ){
                        break;
                    }
                    GridArray[i,ii] = 1;
                    
                }
            }
        }

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