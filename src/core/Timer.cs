using System;
using System.Data;
using Raylib_cs;

namespace Tetris_QMJ.src.Core {
    public class Timer {
        private float timeSec;
        private int time;

        public Timer(){
            timeSec = 0;
            time = Environment.TickCount;
        }

        public void UpdateTimer(){
            int currentTime = Environment.TickCount;
            timeSec = (currentTime - time)/1000.0f;
        }

        public void ShowTime(int x, int y, Font font, int fontSize, Color color){
            int min = (int)(timeSec/60);
            int sec = (int)(timeSec%60);
            string timeTxt = $"{min:D2}:{sec:D2}";
            Raylib.DrawTextEx(font, timeTxt, new System.Numerics.Vector2(x,y), fontSize, 1, color);
        }

        public void ResetTime(){
            time = Environment.TickCount;
            timeSec = 0;
        }


    }
}