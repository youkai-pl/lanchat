using Lanchat.Cli.Ui;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Lanchat.Cli.ProgramLib
{
    public static class Config
    {
        // Properties
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

        public static List<IPAddress> Muted { get; set; } = new List<IPAddress>();

        // Fields
        private static string _Nickname;
        private static int _Port;

        // Load config
        public static void Load()
        {
            try
            {
                // Load config to dynamic object
                dynamic json = JsonConvert.DeserializeObject(File.ReadAllText("config.json"));

                // Try use loaded config
                _Nickname = json.nickname;
                _Port = json.port;

                // Convert strings to ip addresses
                List<string> MutedStrings = json.muted.ToObject<List<string>>();
                for (int i = 0; i < MutedStrings.Count; i++)
                {
                    Muted.Add(IPAddress.Parse(MutedStrings[i]));
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.GetType());
                Trace.WriteLine(e.Message);

                Nickname = "";
                Port = 4001;
                Muted = new List<IPAddress>();
            }
        }

        // Save config
        private static void Save()
        {
            // Convert ip addresses to strings
            var MutedStrings = new List<string>();
            for (int i = 0; i < Muted.Count; i++)
            {
                MutedStrings.Add(Muted[i].ToString());
            }

            object json = new
            {
                nickname = Nickname,
                port = Port,
                muted = MutedStrings
            };
            try
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(json, Formatting.Indented));
            }
            catch (Exception e)
            {
                Prompt.CrashScreen(e);
            }
        }

        // Add muted user
        public static void AddMute(IPAddress ip)
        {
            Muted.Add(ip);
            Save();
        }

        // Remove muted user
        public static void RemoveMute(IPAddress ip)
        {
            Muted.Remove(ip);
            Save();
        }
    }
}