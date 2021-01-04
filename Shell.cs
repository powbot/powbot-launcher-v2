using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PowBotLauncher
{
    class Shell
    {
        public static bool Execute(string process, string dir, bool background, IEnumerable<string> args)
        {
            try
            {
                var p = new Process {StartInfo = new ProcessStartInfo(process)};
                foreach (var arg in args)
                {
                    p.StartInfo.ArgumentList.Add(arg);
                }

                p.StartInfo.WorkingDirectory = dir;
                p.StartInfo.CreateNoWindow = background;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = true;
                p.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }
    }
}