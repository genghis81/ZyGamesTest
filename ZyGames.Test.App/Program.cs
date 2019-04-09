using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Script;
using ZyGames.Test.Net;

namespace ZyGames.Test.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var setting = new TaskSetting();
                ScriptEngines.AddReferencedAssembly("ZyGames.Test.dll");
                ScriptEngines.Initialize();

                Console.WriteLine("===============================");
                Console.WriteLine("Stress Test");
                Console.WriteLine("Option:");
                Console.WriteLine("\tPress \"Esc\" is exits!");
                Console.WriteLine("===============================");
                Console.WriteLine("Press any key start run");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    return;
                }
                Console.WriteLine("Running...");

                //ThreadCaseWork<CaseStep> testCase = new ThreadCaseWork<CaseStep>();

                while (true)
                {
                    string result = ThreadManager.RunTest(setting);
                    //string result = testCase.RunTest(setting);
                    Console.WriteLine(result);
                    TraceLog.ReleaseWrite(result);
                    Console.WriteLine("Press any key to continue.");
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    Console.WriteLine("Running...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TraceLog.WriteError("Main error:{0}", ex);
                Console.ReadKey();
            }
        }

    }
}
