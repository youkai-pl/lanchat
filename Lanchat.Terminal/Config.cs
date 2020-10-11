using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Lanchat.Terminal
{
    public class Config
    {
        private static int _port;
        private static string _nickname;

        [JsonConstructor]
        public Config(string nickname, int port)
        {
            Nickname = nickname;
            Port = port;
        }
        
        [DefaultValue("3645")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                Save();
            }
        }

        [DefaultValue("user")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
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

                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path + "config.json"));
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException) && !(e is DirectoryNotFoundException) &&
                    !(e is JsonSerializationException) && !(e is JsonReaderException))
                {
                    throw;
                }

                Trace.WriteLine("[APP] Config load error");
                return JsonConvert.DeserializeObject<Config>("{}");
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

                File.WriteAllText(Path + "config.json", JsonConvert.SerializeObject(this, Formatting.Indented));
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