using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Lanchat.Terminal
{
    public class Config
    {
        private static int _port = 3645;
        private static string _nickname = "user";

        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                Save();
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                Save();
            }
        }

        public static string Path { get; private set; }

        public static Config Load()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Path = Environment.GetEnvironmentVariable("HOME") + "/.Lancaht2/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Path = Environment.GetEnvironmentVariable("HOME") + "/Library/Preferences/.Lancaht2/";
                }

                return JsonSerializer.Deserialize<Config>(File.ReadAllText(Path + "config.json"));
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException) && !(e is DirectoryNotFoundException) && !(e is JsonException))
                {
                    throw;
                }

                Trace.WriteLine("[APP] Config load error");
                return new Config();
            }
        }

        private void Save()
        {
            try
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                File.WriteAllText(Path + "config.json", JsonSerializer.Serialize(this));
            }
            catch (Exception e)
            {
                if (!(e is DirectoryNotFoundException) && !(e is UnauthorizedAccessException))
                {
                    throw;
                }

                Trace.WriteLine(e.Message);
            }
        }
    }
}