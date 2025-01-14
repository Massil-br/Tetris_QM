using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Raylib_cs;

namespace Tetris_QMJ.src.Core
{
    public class Options
    {   
        public float Volume { get; set; } = 0.5f;
        private const string FilePath = "./src/Interfaces/option_key.txt";
        public Dictionary<string, KeyboardKey> KeyBindings { get; private set; }
        public Options(){
            KeyBindings = new Dictionary<string, KeyboardKey>();
            LoadKey();
        }

        //charge les touches du fichier qui se trouve dans le fichier option_key.txt
        public void LoadKey(){
            if (!File.Exists(FilePath))
            {
                // Créer un fichier avec les touches par défaut si le fichier n'existe pas
                File.WriteAllLines(FilePath, new[]
                {
                    "MoveLeft:Left",
                    "MoveRight:Right",
                    "MoveDown:Down",
                    "Rotate:Space"
                });
            }
            KeyBindings.Clear();
            // charge les touches du fichier option_key.txt
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                var split = line.Split(':');
                if (split.Length == 2 && Enum.TryParse(split[1], out KeyboardKey key))
                {
                    KeyBindings.Add(split[0], key);
                }
            }
        }

        //permet de sauvegarder les touches dans le fichier
        public void SaveKey(){
            List<string> lines = new List<string>();
            foreach (var key in KeyBindings)
            {
                lines.Add($"{key.Key}:{key.Value}");
            }
            File.WriteAllLines(FilePath, lines);
        }

        //permet de changer une touche
        public void SetKey(string action, KeyboardKey key){
            if (KeyBindings.ContainsKey(action))
            {
                KeyBindings[action] = key;
                SaveKey();
            }
        }
    }
}