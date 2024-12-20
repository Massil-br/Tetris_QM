

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace Tetris_QMJ.src.Core{
    public class Grid{
        int[,] GridArray{get;set;}
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

    }
}