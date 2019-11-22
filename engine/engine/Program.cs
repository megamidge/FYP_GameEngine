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
            using (var gameWindow = new EngineGameWindow(800,450))
                gameWindow.Run();
        }
    }
}
