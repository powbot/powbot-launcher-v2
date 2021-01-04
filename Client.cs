using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace PowBotLauncher
{
    class Client
    {
        public static string GetDirectory()
        {
            return Path.Combine(HomeFolder.GetDirectory(), "client");
        }

        private static string GetRemoteHash()
        {
            using (var client = new WebClient())
            {
                return client.DownloadString("https://powbot.org/game/current_client").Replace("\n", "");
            }
        }

        private static void ObtainClient(string hash, string outputFile, Action<string> onStatusChange)
        {
            onStatusChange($"Downloading latest client: {hash}");
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += (sender, args) =>
                    onStatusChange($"Downloading latest client: {hash} ({args.ProgressPercentage}% completed)");
                client.DownloadFile($"https://powbot.org/game/{hash}.jar", outputFile);
            }

            if (!File.Exists(outputFile))
            {
                onStatusChange("Failed to download client");
                Environment.Exit(255);
            }
        }

        private static string ComputeSha1Hash(string file)
        {
            using var fs = File.OpenRead(file);
            var sha = new SHA1Managed();
            return BitConverter.ToString(sha.ComputeHash(fs)).Replace("-", "").ToLower();
        }

        public static string EnsureLatestClient(Action<string> onStatusChange)
        {
            if (!Directory.Exists(GetDirectory()))
            {
                Directory.CreateDirectory(GetDirectory());
            }

            var expectedHash = GetRemoteHash();
            var clientFile = Path.Combine(GetDirectory(), "PowBot.jar");
            if (!File.Exists(clientFile))
            {
                onStatusChange("Local client not found - downloading...");
                ObtainClient(expectedHash, clientFile, onStatusChange);
            }

            var actualHash = ComputeSha1Hash(clientFile);
            if (actualHash != expectedHash)
            {
                onStatusChange("Local client out of date - downloading...");
                ObtainClient(expectedHash, clientFile, onStatusChange);
            }

            onStatusChange($"Using client {ComputeSha1Hash(clientFile)}");
            return clientFile;
        }
    }
}