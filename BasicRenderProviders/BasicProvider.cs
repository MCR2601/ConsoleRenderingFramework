using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;

namespace BasicRenderProviders
{
    public static class BasicProvider
    {
        public static PInfo[,] getInked(int width, int height, PInfo pi)
        {
            PInfo[,] data = new PInfo[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int x = 0; x < height; x++)
                {
                    data[i, x] = new PInfo();
                    if (pi.hasForeground)
                    {
                        data[i, x].foreground = pi.foreground;
                    }
                    if (pi.hasBackground)
                    {
                        data[i, x].background = pi.background;
                    }
                    if (pi.hasCharacter)
                    {
                        data[i, x].character = pi.character;
                    }
                }
            }
            return data;
        }
        public static PInfo[,] TextToPInfo(string text, int maxwidth, int maxheight, PInfo apperence)
        {
            string[] parts = text.Split(' ');
            if (parts.Length == 1)
            {
                parts[0] = text;
            }


            PInfo[,] data = getInked(maxwidth, maxheight, apperence);

            // used to know when to stop
            int height = 0;
            // index of current word
            int word = 0;

            Line[] lines = new Line[maxheight];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new Line(maxwidth);
            }

            while (height<maxheight && word<parts.Length)
            {
                if (lines[height].Fit(new Word(parts[word])))
                {
                    // it fits so we will proceede with next word
                    word++;
                }
                else
                {
                    // next line
                    height++;
                }
            }

            for (int h = 0; h < maxheight; h++)
            {
                PInfo[,] info = lines[h].GetDrawn(apperence);
                for (int w = 0; w < info.GetLength(0); w++)
                {
                    data[w, h] = info[w, 0];
                }
            }
            return data;

        }


        private class Word
        {
            public string content;
            public Word(string c)
            {
                content = c;
            }

        }

        private class Line
        {
            public int MaxLength;
            public List<Word> content;

            public int CurrLength
            {
                get
                {
                    return content.Sum(x => x.content.Length) + (content.Count - 1);
                }
            }

            public Line(int mLength)
            {
                MaxLength = mLength;
                content = new List<Word>();
            }

            public bool Fit(Word w)
            {
                if (CurrLength + 1 + w.content.Length <= MaxLength)
                {
                    content.Add(w);
                    return true;
                }
                return false;
            }
            public PInfo[,] GetDrawn(PInfo look)
            {
                PInfo[,] info = getInked(MaxLength, 1, look);

                int i = 0;

                foreach (var item in content)
                {
                    foreach (var c in item.content)
                    {
                        info[i, 0].character = c;
                        i++;
                    }
                    if (i<MaxLength)
                    {
                        i++;
                    }
                }
                return info;
            }

        }


    }
}
