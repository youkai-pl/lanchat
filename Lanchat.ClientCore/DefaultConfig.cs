using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Lanchat.Core;
using Lanchat.Core.Models;
using Syroot.Windows.IO;

namespace Lanchat.ClientCore
{
    public class DefaultConfig : IConfig
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Nickname { get; set; } = NicknamesGenerator.GimmeNickname();
        public int ServerPort { get; set; } = 3645;
        public int BroadcastPort { get; set; } = 3646;
        public List<IPAddress> BlockedAddresses { get; set; } = new();
        public Status Status { get; set; } = Status.Online;
        public bool UseIPv6 { get; set; } = false;
        public int MaxMessageLenght { get; set; } = 1500;
        public int MaxNicknameLenght { get; set; } = 20;
        public bool AutomaticConnecting { get; set; } = true;
        public string ReceivedFilesDirectory { get; set; } = GetDownloadsDirectory();

        public Config GetDefaultConfig()
        {
            var config = new Config();
            CopyPropertiesTo(this, config);
            config.Language = "default";
            config.Fresh = true;
            return config;
        }

        private static string GetDownloadsDirectory()
        {
            var path = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
            
            if (path != null)
            {
                return path;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = new KnownFolder(KnownFolderType.Downloads).Path;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = Environment.GetEnvironmentVariable("HOME") + "/Downloads";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                path = Environment.GetEnvironmentVariable("HOME") + "/Downloads";
            }

            return path;
        }
        
        private static void CopyPropertiesTo<T, TU>(T source, TU dest)
        {
            var sourceProps = typeof (T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.All(x => x.Name != sourceProp.Name)) continue;
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if(p.CanWrite) {
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }
            }
        }
    }
}