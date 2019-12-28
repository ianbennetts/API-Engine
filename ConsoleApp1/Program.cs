using APIEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using APIEngine.Processes;

namespace ConsoleApp1
{
    class Program
    {
        static int total = 0;
        static Accumulator accumulator;
        static void Main(string[] args)
        {
            ProcessRequest request = new ProcessRequest("TT4");
            var value = request.Process();
            var t= value.ToString();
            int i = 0;
            Console.WriteLine("The Value Returned was " + t);
            Console.ReadLine();
 
        }
    }

}

