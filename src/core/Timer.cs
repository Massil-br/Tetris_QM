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

        //permet de mettre à jour le timer en fonction du temps écoulé
        public void UpdateTimer(){
            int currentTime = Environment.TickCount;
            timeSec = (currentTime - time)/1000.0f;
        }

        //permet d'afficher le temps en minutes et secondes sur l'écran
        public void ShowTime(int x, int y, Font font, int fontSize, Color color){
            int min = (int)(timeSec/60);
            int sec = (int)(timeSec%60);
            string timeTxt= string.Format("{0} : {1}", min, sec);
            
            Raylib.DrawTextEx(font, timeTxt, new System.Numerics.Vector2(x,y), fontSize, 1, color);
        }

        //permet de retourner le temps en secondes
        public float Rapidite() {
            return timeSec;
        }

    }
}