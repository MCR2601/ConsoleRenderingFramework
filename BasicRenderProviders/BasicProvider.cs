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
            int linesLeft = maxheight;
            int charsLeft = maxwidth;
            int currIndex = 0;
            int currCharIndex = 0;
            int currLineIndex = 0;

            bool done = false;

            PInfo[,] data = new PInfo[maxwidth, maxheight];

            data.Populate(apperence);

            while (!done)
            {
                if (currIndex==parts.Length)
                {
                    done = true;
                }
                else
                {
                    // if there is enough space in the current line print everything
                    if (parts[currIndex].Length<=charsLeft)
                    {
                        for (int i = 0; i < parts[currIndex].Length; i++)
                        {
                            
                            PInfo tmp = new PInfo(parts[currIndex][i], apperence.foreground, apperence.background);
                            data[currCharIndex + i, currLineIndex] = tmp; 
                        }
                        currCharIndex += parts[currIndex].Length;
                        charsLeft -= parts[currIndex].Length;
                        currIndex++;
                    }
                    else
                    {
                        // no space move down a line
                        if (linesLeft>0)
                        {
                            linesLeft--;
                            currLineIndex++;

                            charsLeft = data.GetLength(1);
                            currCharIndex = 0;

                        }
                        else
                        {
                            done = true;
                        }
                    }
                }
            }


            //PInfo[,] info = new PInfo[1,1];

            return data;

        }





    }
}
