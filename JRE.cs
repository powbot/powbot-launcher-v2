using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace powbot_launcher_v2
{
    class JRE
    {

        public static string GetDirectory() {
            string homeFolder = HomeFolder.GetDirectory();
            return Path.Combine(homeFolder, "jre");
        }

        public static string GetOrObtainJREBinary() {
            string dir = GetDirectory();
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            string binary = FindBinary(dir);
            if (binary != null) {
                Console.WriteLine($"Java binary found at {binary}");
            } else {
                Console.WriteLine("Java binary not found, downloading...");
                binary = ObtainJRE();
            }

            return binary;
        }

        public static string ObtainJRE() {
            string downloadUrl = GetJREDownloadURL();
            string outputFile = Path.Combine(GetDirectory(), Path.GetFileName(downloadUrl));

            using (var client = new WebClient())
            {
                client.DownloadFile(downloadUrl, outputFile);
            }

            if (outputFile.EndsWith(".zip")) 
            {
                if (!Unzip(outputFile))
                {
                    throw new Exception($"{outputFile} - Failed to decompress file");
                }
            } else if (outputFile.EndsWith(".gz"))
            {
                if (!Untar(outputFile))
                {
                    throw new Exception($"{outputFile} - Failed to decompress file");
                }
            } else {
                throw new Exception($"{outputFile} - File type not supported");
            }

            return FindBinary(GetDirectory());
        }

        private static Boolean Unzip(string zipFile) {
            return Shell.Execute("unzip", GetDirectory(), false, new List<string> {Path.GetFileName(zipFile)});
        }
        private static Boolean Untar(string gzFile) {
            return Shell.Execute("tar", GetDirectory(), false, new List<string> {"-xvf", Path.GetFileName(gzFile)});
        }

        private static string GetBinaryName() {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            switch (pid) 
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return "javaw.exe";
                default:
                    return "java";
            }
        }

        private static string GetJREDownloadURL() {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            switch (pid) 
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
		            return "https://github.com/AdoptOpenJDK/openjdk11-binaries/releases/download/jdk-11.0.9.1%2B1/OpenJDK11U-jre_x86-32_windows_hotspot_11.0.9.1_1.zip";
                case PlatformID.MacOSX:
                case PlatformID.Unix:
		            return "https://github.com/AdoptOpenJDK/openjdk11-binaries/releases/download/jdk-11.0.9.1%2B1/OpenJDK11U-jre_x64_mac_hotspot_11.0.9.1_1.tar.gz";
                default:
		            return "https://github.com/AdoptOpenJDK/openjdk11-binaries/releases/download/jdk-11.0.9.1%2B1/OpenJDK11U-jre_x64_linux_hotspot_11.0.9.1_1.tar.gz";
            }
        }

        private static string FindBinary(string sDir) 
        {
            try 
            {

                foreach (string d in Directory.GetDirectories(sDir)) 
                {
                    string binary = FindBinary(d);
                    if (binary != null) {
                        return binary;
                    }
                }
                foreach (string f in Directory.GetFiles(sDir)) 
                {
                    if (f.EndsWith($"{GetBinaryName()}")) {
                        return f;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            return null;
        }
    }
}
