using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoSetupInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            throw new Exception("Not allowed");
#endif
            //RunScripts("setup_creator.iss");
            //RunScripts("setup_player.iss");
            RunScripts("setup_core.iss");
            Console.ReadLine();
        }

        private static void RunScripts(string scriptName)
        {
            var file = Path.Combine(Environment.CurrentDirectory, "Resources", "iscc.exe");
            var iss = Path.Combine(Environment.CurrentDirectory, "Resources", scriptName );
            var proc = new Process();
            proc.StartInfo.FileName = "\""+file+"\"";
            proc.StartInfo.Arguments = "\"" +iss+ "\"";
            proc.Start();
            proc.WaitForExit();
            var exitCode = proc.ExitCode;
            proc.Close();
            Console.WriteLine("Exit Code: " + exitCode);
            //  Console.WriteLine(proc.StandardOutput.ReadToEnd());
            
        }
    }
}
