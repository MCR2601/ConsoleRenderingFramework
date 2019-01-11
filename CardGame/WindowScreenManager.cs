using BasicRenderProviders;
using ConsoleRenderingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    /// <summary>
    /// The WindowScreenManager has a Name and multiple Textboxes
    /// </summary>
    class WindowScreenManager : IRenderingApplication, IScreenManager
    {
        public int width { get ; set ; }
        public int height { get ; set ; }

        public event DrawEvent DrawScreen;

        List<TextBox> Content = new List<TextBox>();

        /// <summary>
        /// Used to store layouts
        /// </summary>
        static Dictionary<string, WindowSheet> History = new Dictionary<string, WindowSheet>();

        public PInfo Background = new PInfo();

        public WindowScreenManager(int width, int height,DrawEvent DrawHirachy)
        {
            DrawScreen += DrawHirachy;
            this.width = width;
            this.height = height;
        }

        public WindowScreenManager(int width, int height, DrawEvent DrawHirachy, PInfo BackgroundStyle): this(width, height, DrawHirachy)
        {
            Background = BackgroundStyle;
        }


        public void App_DrawScreen(PInfo[,] information, int x, int y, IRenderingApplication sender)
        {
            DrawScreen?.Invoke(information, x, y, sender);
        }

        public void Render()
        {
            App_DrawScreen(BasicProvider.getInked(width, height, Background), 0, 0, this);
            foreach (var item in Content)
            {
                DrawBox(item);
            }
        }

        public void DrawBox(TextBox box)
        {
            int posX = 0;
            int posY = 0;

            switch (box.ParentAnchor)
            {
                case Alignment.TopLeft:
                    posX = box.X;
                    posY = box.Y;
                    break;
                case Alignment.TopRight:
                    posX = width - box.X - box.Width;
                    posY = box.Y;
                    break;
                case Alignment.BottomLeft:
                    posX = box.X;
                    posY = height - box.Y - box.Heigth;
                    break;
                case Alignment.BottomRight:
                    posX = width - box.X - box.Width;
                    posY = height - box.Y - box.Heigth;
                    break;
                default:
                    break;
            }

            PInfo[,] images = BasicRenderProviders.BasicProvider.TextToPInfo(box.Content, box.Width, box.Heigth, box.Style);

            App_DrawScreen(images, posX, posY, this);
        }


        public void LoadSheet(WindowSheet sheet)
        {
            Content = new List<TextBox>();
            Content.AddRange(sheet.Content.Select(x => x.Copy()));
            if (!History.ContainsKey(sheet.Name))
            {
                History[sheet.Name] = sheet;
            }
        }
        public void LoadSheet(string sheetName)
        {
            if (History.ContainsKey(sheetName))
            {
                LoadSheet(History[sheetName]);
            }
        }

        /// <summary>
        /// Used to change a value on an interface
        /// </summary>
        /// <param name="Index">Index of the Box</param>
        /// <param name="newValue">new Text value for display</param>
        public void ChangeText(string Index, string newValue)
        {
            Content.Where(x => x.Indexer == Index).ToList().ForEach(t => t.Content = newValue);
        }

        /// <summary>
        /// returns the first matching TextBox with the given index
        /// </summary>
        /// <param name="Index">named index</param>
        /// <returns>first found TextBox</returns>
        public TextBox GetTextBox(string Index)
        {
            return Content.Where(x => x.Indexer == Index).Take(1).FirstOrDefault();
        }

    }
}
