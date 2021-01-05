using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace PowBotLauncher
{
    class Shell
    {
        public static bool Execute(string binary, string dir, bool background, IEnumerable<string> args)
        {
            try
            {
                var process = new Process {StartInfo = new ProcessStartInfo(binary)};
                foreach (var arg in args)
                {
                    process.StartInfo.ArgumentList.Add(arg);
                }

                process.StartInfo.WorkingDirectory = dir;
                process.StartInfo.CreateNoWindow = background;
                process.StartInfo.UseShellExecute = background;
                if (!background)
                {
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardInput = true;
                }

                process.Start();
                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }
    }

}