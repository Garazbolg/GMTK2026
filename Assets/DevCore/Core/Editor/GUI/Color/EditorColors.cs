using UnityEngine;

namespace DevCore.Core.Editor {
    public enum EditorColors {
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Cyan,
        Magenta,
        Dark,
        Grey,
        Light
    }
    
    public static class EditorColorExtensions{
        public static Color Color(this EditorColors col) {
            switch (col) {
                case EditorColors.Red:
                    return new Color(1f, 0.52f, 0.5f);
                case EditorColors.Green:
                    return new Color(0.65f, 1f, 0.62f);
                case EditorColors.Blue:
                    return new Color(0.66f, 0.67f, 1f);
                case EditorColors.Yellow:
                    return new Color(1f, 0.97f, 0.58f);
                case EditorColors.Orange:
                    return new Color(1f, 0.78f, 0.57f);
                case EditorColors.Cyan:
                    return new Color(0.67f, 0.97f, 1f);
                case EditorColors.Magenta:
                    return new Color(1f, 0.79f, 0.98f);
                case EditorColors.Dark:
                    return new Color(0.08f, 0.08f, 0.08f);
                case EditorColors.Grey:
                    return new Color(0.44f, 0.44f, 0.44f);
                case EditorColors.Light:
                    return new Color(0.92f, 0.92f, 0.92f);
            }
            
            return UnityEngine.Color.clear;
        }
    }
}