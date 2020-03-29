using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] arrBuff = System.IO.File.ReadAllBytes("calibri.ttf");
            //engine.Text.TrueTypeFont font = new engine.Text.TrueTypeFont(arrBuff);
            //Console.WriteLine($"Rendering font. Font info:");
            //Console.WriteLine($"Version: {font.version}");
            //Console.WriteLine($"Glyph cont: {font.length}");
            //DateTime then = DateTime.Now;
            //for(uint i = 0; i < font.length; i++)
            //{
            //    font.DrawGlyph(i);
            //    double progress = ((double)(i+1)/font.length)*100;
            //    Console.WriteLine($"Progress: {progress}%");
            //}
            //Console.WriteLine($"Rendered out all {font.length} glyphs in {(DateTime.Now - then).ToString()}");
            //Console.ReadLine();
            /*return;*/
            using (var gameWindow = new EngineGameWindow(800, 450))
            {
                gameWindow.VSync = OpenTK.VSyncMode.Off;
                gameWindow.Run();
            }
        }
    }
}
