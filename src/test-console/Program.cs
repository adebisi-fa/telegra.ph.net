using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegraph.Net.Models;

namespace TestConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
           TestNodeElementParser.Run();
        }
    }
}
