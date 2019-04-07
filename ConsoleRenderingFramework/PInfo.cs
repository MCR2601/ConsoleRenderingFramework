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

        private ConsoleColor _foreground;
        public ConsoleColor Foreground
        {
            get { return _foreground; }
            set
            {
                if (value != Foreground)
                {
                    _foreground = value;
                    isChanged = true;
                    HasForeground = true;
                }
                if (!HasForeground)
                {
                    _foreground = value;
                    isChanged = true;
                    HasForeground = true;
                }
            }
        }
        private ConsoleColor _background;
        public ConsoleColor Background
        {
            get { return _background; }
            set
            {
                if (Background != value)
                {
                    _background = value;
                    isChanged = true;
                    HasBackground = true;
                }
                if (!HasBackground)
                {
                    _background = value;
                    isChanged = true;
                    HasBackground = true;
                }
            }
        }
        private char _character;
        public char Character
        {
            get { return _character; }
            set
            {
                if (Character != value)
                {
                    _character = value;
                    isChanged = true;
                    HasCharacter = true;
                }
                if (!HasCharacter)
                {
                    _character = value;
                    isChanged = true;
                    HasBackground = true;
                }
            }
        }
        private bool _hasForeground = false;
        public bool HasForeground
        {
            get { return _hasForeground; }
            set
            {
                if (value != HasForeground)
                {
                    _hasForeground = value;
                    isChanged = true;
                }
            }
        }
        private bool _hasBackground = false;
        public bool HasBackground
        {
            get { return _hasBackground; }
            set
            {
                if (value != HasBackground)
                {
                    _hasBackground = value;
                    isChanged = true;
                }
            }
        }
        private bool _hasCharacter = false;
        public bool HasCharacter
        {
            get { return _hasCharacter; }
            set
            {
                if (value != HasCharacter)
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
            Character = c;
            HasCharacter = true;
            Foreground = fg;
            HasForeground = true;
            Background = bg;
            HasBackground = true;
        }
        public void PrintPixel()
        {
            Console.BackgroundColor = Background;
            Console.ForegroundColor = Foreground;
            Console.Write(Character);
        }

        /// <summary>
        /// Sets the Character of this PInfo
        /// </summary>
        /// <param name="c">Character</param>
        /// <returns>the changed PInfo</returns>
        public PInfo SetC(char c)
        {
            Character = c;
            return this;
        }

        /// <summary>
        /// Sets the Foreground of this PInfo
        /// </summary>
        /// <param name="f">Foreground color</param>
        /// <returns>the changed PInfo</returns>
        public PInfo SetFg(ConsoleColor f)
        {
            Foreground = f;
            return this;
        }

        /// <summary>
        /// Sets the Background of this PInfo
        /// </summary>
        /// <param name="b">Background color</param>
        /// <returns>the changed PInfo</returns>
        public PInfo SetBg(ConsoleColor b)
        {
            Background = b;
            return this;
        }

        /// <summary>
        /// adds the value of an other <see cref="PInfo"/> to this
        /// </summary>
        public void Override( PInfo info)
        {
            if (this.HasBackground)
            {
                if (info.HasBackground)
                {
                    this.Background = info.Background;
                }
            }
            else
            {
                this.HasBackground = info.HasBackground;
                this.Background = info.Background;
            }

            if (this.HasForeground)
            {
                if (info.HasForeground)
                {
                    this.Foreground = info.Foreground;
                }
            }
            else
            {
                this.HasForeground = info.HasForeground;
                this.Foreground = info.Foreground;
            }

            if (this.HasCharacter)
            {
                if (info.HasCharacter)
                {
                    this.Character = info.Character;
                }
            }
            else
            {
                this.HasCharacter = info.HasCharacter;
                this.Character = info.Character;
            }
        }
        public PInfo Copy()
        {
            PInfo n = new PInfo();
            n.Override(this);
            return n;
        }
        public override bool Equals(object obj)
        {
            if (obj is PInfo)
            {
                PInfo t = obj as PInfo;
                if (t.HasBackground == HasBackground)
                {
                    if (t.HasBackground== true && t.Background != Background)
                    {
                        return false;
                    }
                }
                if (t.HasForeground == HasForeground)
                {
                    if (t.HasForeground == true && t.Foreground != Foreground)
                    {
                        return false;
                    }
                }
                if (t.HasCharacter == HasCharacter)
                {
                    if (t.HasCharacter == true && t.Character != Character)
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1164796431;
            hashCode = hashCode * -1521134295 + _foreground.GetHashCode();
            hashCode = hashCode * -1521134295 + _background.GetHashCode();
            hashCode = hashCode * -1521134295 + _character.GetHashCode();
            hashCode = hashCode * -1521134295 + _hasForeground.GetHashCode();
            hashCode = hashCode * -1521134295 + _hasBackground.GetHashCode();
            hashCode = hashCode * -1521134295 + _hasCharacter.GetHashCode();
            hashCode = hashCode * -1521134295 + isChanged.GetHashCode();
            return hashCode;
        }
    }
    public static class PInfoUtil
    {
        public static void Populate(this PInfo[,] info, PInfo data = null)
        {
            if (data == null)
            {
                data = new PInfo() { HasBackground = false, HasCharacter = false, HasForeground = false };
            }
            for (int x = 0; x < info.GetLength(0); x++)
            {
                for (int y = 0; y < info.GetLength(1); y++)
                {
                    info[x, y] = new PInfo()
                    {
                        Background = data.Background,
                        HasBackground = data.HasBackground,
                        Character = data.Character,
                        HasCharacter = data.HasCharacter,
                        Foreground = data.Foreground,
                        HasForeground = data.HasForeground
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
        public static Brush GetPInfoBrush(PInfo pi)
        {
            switch (pi.Background)
            {
                case ConsoleColor.Black:
                    return Brushes.Black;
                case ConsoleColor.DarkBlue:
                    return Brushes.DarkBlue;
                case ConsoleColor.DarkGreen:
                    return Brushes.DarkGreen;
                case ConsoleColor.DarkCyan:
                    return Brushes.DarkCyan;
                case ConsoleColor.DarkRed:
                    return Brushes.DarkRed;
                case ConsoleColor.DarkMagenta:
                    return Brushes.DarkMagenta;
                case ConsoleColor.DarkYellow:
                    return Brushes.Yellow;
                case ConsoleColor.Gray:
                    return Brushes.Gray;
                case ConsoleColor.DarkGray:
                    return Brushes.DarkGray;
                case ConsoleColor.Blue:
                    return Brushes.Blue;
                case ConsoleColor.Green:
                    return Brushes.Green;
                case ConsoleColor.Cyan:
                    return Brushes.Cyan;
                case ConsoleColor.Red:
                    return Brushes.Red;
                case ConsoleColor.Magenta:
                    return Brushes.Magenta;
                case ConsoleColor.Yellow:
                    return Brushes.LightYellow;
                case ConsoleColor.White:
                    return Brushes.White;
                default:
                    return Brushes.White;
            }
        }         
    }
}