using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP.Utility
{
    class Program
    {
        //Run the app with CTRL+F5 and it will remin open 
        //When pressing F5 the app will close autometically. We can either do Console.ReadLine(); or Console.ReadKey(); and wait.
        static void Main(string[] args)
        {
            $"Check FtpUtility.cs for details".WriteNewLine(WriteLine.ColorCyan);

            "".EmptyLine();
            "".EmptyLine();
            "Press any key to exit >>".WriteSameLine(WriteLine.ColorRed);
            Console.ReadKey();
        }
    }
}
