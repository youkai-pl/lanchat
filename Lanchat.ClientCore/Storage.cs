using System;
using System.IO;
using Lanchat.Core.Config;
using Lanchat.Core.FileSystem;

namespace Lanchat.ClientCore
{
    /// <inheritdoc/>
    public class Storage : IStorage
    {
        private readonly IConfig config;

        public Storage(IConfig config)
        {
            this.config = config;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void DeleteIncompleteFile(string path)
        {
            File.Delete(path);
        }

        /// <inheritdoc/>
        public string GetFilePath(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);
            var newPath = Path.Combine(config.ReceivedFilesDirectory, $"{fileName}{fileExt}");

            for (var i = 1;; ++i)
            {
                if (!File.Exists(newPath))
                {
                    return newPath;
                }

                newPath = Path.Combine(config.ReceivedFilesDirectory, $"{fileName}({i}){fileExt}");
            }
        }

        /// <inheritdoc/>
        public long GetFileSize(string path)
        {
            return new FileInfo(path).Length;
        }
    }
}