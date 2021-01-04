using System;
using System.IO;

namespace powbot_launcher_v2
{
    class HomeFolder
    {

        public static string GetDirectory() {
            string homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(homeFolder, ".powbot");
        }
        public static void Create()
        {
            string powbotHomeFolder = GetDirectory();
            if (!Directory.Exists(powbotHomeFolder)) {
                Console.WriteLine($"Creating powbot home directory at {powbotHomeFolder}");
                Directory.CreateDirectory(powbotHomeFolder);
            }
        }
    }
}
