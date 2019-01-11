using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class WindowSheet
    {
        public string Name;
        public List<TextBox> Content = new List<TextBox>();

        public WindowSheet(string name, List<TextBox> content)
        {
            Name = name;
            Content = content;
        }
    }
}
