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

        public static int BroadcastPort
        {
            get => _BroadcastPort;
            set
            {
                _BroadcastPort = value;
                Save();
            }
        }

        public static int HostPort
        {
            get => _HostPort;
            set
            {
                _HostPort = value;
                Save();
            }
        }

        public static List<IPAddress> Muted { get; set; } = new List<IPAddress>();

        // Fields
        private static string _Nickname;

        private static int _BroadcastPort;
        private static int _HostPort;

        // Load config
        public static void Load()
        {
            try
            {
                // Load config to dynamic object
                dynamic json = JsonConvert.DeserializeObject(File.ReadAllText("config.json"));

                // Try use loaded config
                _Nickname = json.nickname;
                _BroadcastPort = json.broadcastport;
                _HostPort = json.hostport;

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
                BroadcastPort = 4001;
                HostPort = -1;
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
                broadcastport = BroadcastPort,
                hostport = HostPort,
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