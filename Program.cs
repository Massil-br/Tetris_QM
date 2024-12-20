using System;
using System.Numerics;
using Raylib_cs;
namespace Tetris_QMJ
{
    class Program
    {
        static void Main(string[] args)
        {   
            int width = 1000;
            int heigth = 1000;
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(width ,heigth,"Tetris");
            
            Raylib.SetTargetFPS(60);
            Font TitleFont = Raylib.LoadFont("assets/font/Team 401.ttf");
            while(!Raylib.WindowShouldClose()){
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                
                Raylib.DrawTextEx(TitleFont, "TETRIS", new Vector2((Raylib.GetRenderWidth()/2)-35,50), 40, 2, Color.Black);
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}
