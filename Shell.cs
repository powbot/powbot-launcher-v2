using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace powbot_launcher_v2
{
    class Shell
    {

        public static Boolean Execute(string process, string dir, bool background, List<string> args) {
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(process);
                foreach (var arg in args)
                {
                    p.StartInfo.ArgumentList.Add(arg);
                }

                p.StartInfo.WorkingDirectory = dir;
                p.StartInfo.CreateNoWindow = background;
                p.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }
    }
}
