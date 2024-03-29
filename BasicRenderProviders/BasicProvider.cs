﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;

namespace ConsoleRenderingFramework.RenderProviders
{
    public static class BasicProvider
    {
        public static PInfo[,] getInked(int width, int height, PInfo pi)
        {
            PInfo[,] data = new PInfo[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int y = 0; y < height; y++)
                {
                    data[i, y] = new PInfo();
                    if (pi.HasForeground)
                    {
                        data[i, y].SetFg(pi.Foreground);
                    }
                    if (pi.HasBackground)
                    {
                        data[i, y].SetBg(pi.Background);
                    }
                    if (pi.HasCharacter)
                    {
                        data[i, y].SetC(pi.Character);
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

        public static string ValuesToString<T>(params T[] data)
        {
            if (data.Length == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(data[0].ToString());
            for (int i = 1; i < data.Length; i++)
            {
                sb.Append(", " + data[i].ToString());
            }

            return sb.ToString();
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
                PInfo[,] info;

                if (look.HasBackground)
                {
                    info = getInked(MaxLength, 1, new PInfo().SetBg(look.Background));
                }
                else
                {
                    info = getInked(MaxLength, 1, new PInfo());
                }


                int i = 0;

                foreach (var item in content)
                {
                    foreach (var c in item.content)
                    {
                        info[i, 0].SetC(c);
                        info[i, 0].SetFg(look.Foreground);
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
