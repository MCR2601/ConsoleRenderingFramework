using BasicScreenManagerPackage;
using ConsoleRenderingFramework;
using BasicRenderProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    class CardGameStartup
    {
        static void Main(string[] args)
        {
            GMU gmu = new GMU(102, 62, 0, 21);            
            gmu.PlacePixels(BasicProvider.getInked(102, 62, new PInfo().SetBg(ConsoleColor.DarkGray)), 0, 0, null);
            gmu.PrintFrame();
            gmu.PlacePixels(BasicProvider.getInked(100, 60, new PInfo().SetBg(ConsoleColor.Black).SetFg( ConsoleColor.White).SetC(' ')), 1, 1, null);
            gmu.PrintFrame();

            MultiSplitScreenManager SmallScreen = new MultiSplitScreenManager(gmu.PlacePixels, 102, 62);
           
            MultiSplitScreenManager splitScreen = new MultiSplitScreenManager(SmallScreen.App_DrawScreen, 100, 60,new PInfo().SetBg(ConsoleColor.Gray),new PInfo().SetBg(ConsoleColor.White).SetC(' '));
            SmallScreen.AddScreen(splitScreen, new System.Drawing.Rectangle(1, 1, 100, 60));

            WindowScreenManager Test = new WindowScreenManager(20, 30, splitScreen.App_DrawScreen,new PInfo().SetBg(ConsoleColor.Black).SetC(' '));

            WindowScreenManager Test2 = new WindowScreenManager(20, 30, splitScreen.App_DrawScreen, new PInfo().SetBg(ConsoleColor.Black).SetC(' '));

            splitScreen.AddScreen(Test, new System.Drawing.Rectangle(79, 5, 20, 30),2);
            splitScreen.AddScreen(Test2, new System.Drawing.Rectangle(69, 8, 20, 30),1);


            TextBox tb1 = new TextBox("Name", "Test text for some things", new PInfo().SetFg(ConsoleColor.White), 0, 0, Alignment.TopLeft, 20, 3);
            TextBox tb2 = new TextBox("try", "Another text", new PInfo().SetFg(ConsoleColor.Red).SetBg(ConsoleColor.DarkGray), 0, 0, Alignment.BottomLeft, 20, 3);

            List<TextBox> content = new List<TextBox>();
            content.Add(tb1);
            content.Add(tb2);

            WindowSheet sheet = new WindowSheet("Test", content);

            Test.LoadSheet(sheet);
            Test2.LoadSheet(sheet);

            splitScreen.Render();
            gmu.PrintFrame();            
            Console.ReadKey();
            splitScreen.ChangeLayerOf(Test2, 3);
            splitScreen.Render();
            gmu.PrintFrame();
            Console.ReadKey();
            splitScreen.ChangeLayerOf(Test2, 1);
            splitScreen.Render();
            gmu.PrintFrame();
            Console.ReadKey();
            splitScreen.ChangeLayerOf(Test2, 3);
            splitScreen.Render();
            gmu.PrintFrame();
            Console.ReadKey();
            splitScreen.ChangeLayerOf(Test2, 1);
            splitScreen.Render();
            gmu.PrintFrame();

            Console.ReadKey();
        }
    }
}
