using Lanchat.Cli.PromptLib;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace Lanchat.Cli.ConfigLib
{
    public static class Config
    {
        // Properties
        private static string _Nickname;
        private static int _Port;

        public static string Nickname
        {
            get => _Nickname;
            set
            {
                _Nickname = value;
                Save();
            }
        }
        public static int Port
        {
            get => _Port;
            set
            {
                _Port = value;
                Save();
            }
        }

        // Load config
        public static void Load()
        {
            try
            {
                // Load config to dynamic object
                dynamic json = JsonConvert.DeserializeObject(File.ReadAllText("config.json"));

                // Try use loaded config
                Nickname = json.nickname;
                Port = json.port;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.GetType());
                Trace.WriteLine(e.Message);

                Nickname = "";
                Port = 4001;
            }
        }

        // Save config
        private static void Save()
        {
            object json = new
            {
                nickname = Nickname,
                port = Port
            };
            try
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(json));
            }
            catch (Exception e)
            {
                Prompt.CrashScreen(e);
            }
        }
    }
}