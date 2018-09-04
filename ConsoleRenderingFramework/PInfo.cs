using System;
using System.Drawing;

namespace ConsoleRenderingFramework
{
    /// <summary>
    /// This class provides information over one pixel
    /// it has all console colors and characters known
    /// to console
    /// </summary>
    public class PInfo
    {
        private ConsoleColor _foreground = ConsoleColor.White;
        public ConsoleColor foreground
        {
            get { return _foreground; }
            set
            {
                if (value != foreground)
                {
                    _foreground = value;
                    isChanged = true;
                }
            }
        }
        private ConsoleColor _background = ConsoleColor.Black;
        public ConsoleColor background
        {
            get { return _background; }
            set
            {
                if (background != value)
                {
                    _background = value;
                    isChanged = true;
                }
            }
        }
        private char _character = ' ';
        public char character
        {
            get { return _character; }
            set
            {
                if (character != value)
                {
                    _character = value;
                    isChanged = true;
                }
            }
        }
        private bool _hasForeground = false;
        public bool hasForeground
        {
            get { return _hasForeground; }
            set
            {
                if (value != hasForeground)
                {
                    _hasForeground = value;
                    isChanged = true;
                }
            }
        }
        private bool _hasBackground = false;
        public bool hasBackground
        {
            get { return _hasBackground; }
            set
            {
                if (value != hasBackground)
                {
                    _hasBackground = value;
                    isChanged = true;
                }
            }
        }
        private bool _hasCharacter = false;
        public bool hasCharacter
        {
            get { return _hasCharacter; }
            set
            {
                if (value != hasCharacter)
                {
                    _hasCharacter = value;
                    isChanged = true;
                }
            }
        }
        public bool isChanged = false;

        public PInfo()
        {

        }
        /// <summary>
        /// Creates a new pixel with information for print
        /// </summary>
        /// <param name="c">character that should be displayed in the pixel, ' ' for blank</param>
        /// <param name="fg">Foregroundcolor, standard = white</param>
        /// <param name="bg">Backgroundcolor, standard = black</param>
        public PInfo(char c = ' ', ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
        {
            character = c;
            foreground = fg;
            background = bg;
        }
        public void PrintPixel()
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.Write(character);
        }
        /// <summary>
        /// adds the value of an other <see cref="PInfo"/> to this
        /// </summary>
        public void Override( PInfo info)
        {
            if (this.hasBackground)
            {
                if (info.hasBackground)
                {
                    this.background = info.background;
                }
            }
            else
            {
                this.hasBackground = info.hasBackground;
                this.background = info.background;
            }

            if (this.hasForeground)
            {
                if (info.hasForeground)
                {
                    this.foreground = info.foreground;
                }
            }
            else
            {
                this.hasForeground = info.hasForeground;
                this.foreground = info.foreground;
            }

            if (this.hasCharacter)
            {
                if (info.hasCharacter)
                {
                    this.character = info.character;
                }
            }
            else
            {
                this.hasCharacter = info.hasCharacter;
                this.character = info.character;
            }
        }
        
    }
    public static class PInfoUtil
    {
        public static void Populate(this PInfo[,] info, PInfo data = null)
        {
            if (data == null)
            {
                data = new PInfo() { hasBackground = false, hasCharacter = false, hasForeground = false };
            }
            for (int x = 0; x < info.GetLength(0); x++)
            {
                for (int y = 0; y < info.GetLength(1); y++)
                {
                    info[x, y] = new PInfo()
                    {
                        background = data.background,
                        hasBackground = data.hasBackground,
                        character = data.character,
                        hasCharacter = data.hasCharacter,
                        foreground = data.foreground,
                        hasForeground = data.hasForeground
                    };
                }
            }
        }
        public static void Merge(this PInfo[,] me, PInfo[,] m)
        {
            for (int x = 0; x < me.GetLength(0); x++)
            {
                for (int y = 0; y < me.GetLength(1); y++)
                {
                    me[x, y].Override(m[x, y]);
                }
            }
        }
        public static Brush getPInfoBrush(PInfo pi)
        {
            switch (pi.background)
            {
                case ConsoleColor.Black:
                    return Brushes.Black;
                    break;
                case ConsoleColor.DarkBlue:
                    return Brushes.DarkBlue;
                    break;
                case ConsoleColor.DarkGreen:
                    return Brushes.DarkGreen;
                    break;
                case ConsoleColor.DarkCyan:
                    return Brushes.DarkCyan;
                    break;
                case ConsoleColor.DarkRed:
                    return Brushes.DarkRed;
                    break;
                case ConsoleColor.DarkMagenta:
                    return Brushes.DarkMagenta;
                    break;
                case ConsoleColor.DarkYellow:
                    return Brushes.Yellow;
                    break;
                case ConsoleColor.Gray:
                    return Brushes.Gray;
                    break;
                case ConsoleColor.DarkGray:
                    return Brushes.DarkGray;
                    break;
                case ConsoleColor.Blue:
                    return Brushes.Blue;
                    break;
                case ConsoleColor.Green:
                    return Brushes.Green;
                    break;
                case ConsoleColor.Cyan:
                    return Brushes.Cyan;
                    break;
                case ConsoleColor.Red:
                    return Brushes.Red;
                    break;
                case ConsoleColor.Magenta:
                    return Brushes.Magenta;
                    break;
                case ConsoleColor.Yellow:
                    return Brushes.LightYellow;
                    break;
                case ConsoleColor.White:
                    return Brushes.White;
                    break;
                default:
                    return Brushes.White;
                    break;
            }
        }
    }
}