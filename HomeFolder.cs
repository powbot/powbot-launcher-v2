using System;
using System.IO;

namespace PowBotLauncher
{
    class HomeFolder
    {
        public static string GetDirectory()
        {
            string homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(homeFolder, ".powbot");
        }

        public static void Create(Action<string> onStatusChange)
        {
            var powbotHomeFolder = GetDirectory();
            if (!Directory.Exists(powbotHomeFolder))
            {
                onStatusChange("Checking/creating PowBot directory...");
                Directory.CreateDirectory(powbotHomeFolder);
            }
        }
    }
}
