using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mater.WebJobs
{
    class Program
    {
        static void Main(string[] args)
        {
            JobIndexContent.RunAsync().Wait();
        }
    }
}
