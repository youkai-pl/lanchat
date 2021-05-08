using System;
using System.IO;
using Lanchat.Core.Config;

namespace Lanchat.Core.FileSystem
{
    internal class Storage : IStorage
    {
        private readonly IConfig config;

        internal Storage(IConfig config)
        {
            this.config = config;
        }
        
        public string GetFilePath(string file)
        {
            var path = Path.Combine(config.ReceivedFilesDirectory, file);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);

            for (var i = 1;; ++i)
            {
                if (!File.Exists(path))
                {
                    return path;
                }

                path = Path.Combine(config.ReceivedFilesDirectory, $"{fileName}({i}){fileExt}");
            }
        }
        
        public long GetFileSize(string path)
        {
            return new FileInfo(path).Length;
        }

        public void DeleteIncompleteFile(string path)
        {
            File.Delete(path);
        }
        
        public void CatchFileSystemException(Exception e, Action errorHandler)
        {
            if (e is not (
                DirectoryNotFoundException or
                FileNotFoundException or
                IOException or
                UnauthorizedAccessException))
            {
                throw e;
            }
            
            errorHandler();
        }
    }
}