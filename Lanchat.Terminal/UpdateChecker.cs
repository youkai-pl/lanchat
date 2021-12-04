using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public static class UpdateChecker
    {
        public static async Task CheckUpdatesAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "lanchat-terminal");

            try
            {
                var response = await client.GetAsync("https://api.github.com/repos/tofudd/lanchat/releases/latest");
                var result = await response.Content.ReadAsStringAsync();
                var lastGithubVersion = new Version(Regex.Match(result, "(?<=\"tag_name\":\")(.*)(?=\",\"target)").Value);

                if (Assembly.GetEntryAssembly().GetName().Version.CompareTo(lastGithubVersion) == -1)
                {
                    TabsManager.HomeView.AddText($"Update available: {lastGithubVersion}", ConsoleColor.Green);
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Checking for updates has failed.");
            }
        }
    }
}