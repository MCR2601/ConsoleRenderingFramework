using ConsoleRenderingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class TextBox
    {
        public string Indexer;
        public string Content;
        public PInfo Style;
        public int X;
        public int Y;
        public Alignment ParentAnchor;
        public int Width;
        public int Heigth;

        public TextBox(string indexer, string content, PInfo style, int x, int y, Alignment parentAnchor, int width, int heigth)
        {
            Indexer = indexer;
            Content = content;
            Style = style;
            X = x;
            Y = y;
            ParentAnchor = parentAnchor;
            Width = width;
            Heigth = heigth;
        }

        public TextBox Copy()
        {
            return new TextBox(Indexer, Content, Style.Copy(), X, Y, ParentAnchor, Width, Heigth);
        }
    }
}
