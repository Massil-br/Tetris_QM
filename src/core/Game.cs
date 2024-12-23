using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;


namespace Tetris_QMJ.src.Core{
    public class Game{
        const int width = 800;
        const int height = 600;
        const int gridColumns = 10;
        const int gridRows = 20;
        static  Grid grid = new(gridRows,gridColumns);

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
                
        
        public static void InitWindow(){
            
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(width ,height,"Tetris");
            Raylib.SetTargetFPS(60);
            Font TitleFont = Raylib.LoadFont("assets/font/Team 401.ttf");
            
            

            




            //this while loop is called every frame
            while(!Raylib.WindowShouldClose()){
                int windowHeight = Raylib.GetRenderHeight();
                int windowWidth = Raylib.GetRenderWidth();
                GameLoop(windowHeight,windowWidth, grid);
                
            }
            Raylib.CloseWindow();
        }

        public static void GameLoop(int windowHeight, int windowWidth, Grid grid){
            Piece randomPiece = PieceFactory.GenerateRandomPiece();
            grid.AddPiece(randomPiece);
            int cellSize = Math.Min(windowWidth / (gridColumns + 2), windowHeight / (gridRows + 2));

            int offsetX = (windowWidth - (gridColumns * cellSize)) / 2;
            int offsetY = (windowHeight - (gridRows * cellSize)) / 2;


            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            for (int row = 0; row < gridRows; row++){
                for (int col = 0; col < gridColumns; col++){
                    int cellValue = grid.GridArray[row, col];
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

                   
    }
}

