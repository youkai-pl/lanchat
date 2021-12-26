using System;

namespace Lanchat.Core.FileSystem
{
    /// <summary>
    ///     File system operations.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        ///     Get path for save received file.
        /// </summary>
        /// <param name="name">Base filename</param>
        string GetNewFilePath(string name);

        /// <summary>
        ///     Get file size.
        /// </summary>
        /// <param name="path">File path</param>
        long GetFileSize(string path);

        /// <summary>
        ///     Delete a file.
        /// </summary>
        /// <param name="path">File path</param>
        void DeleteFile(string path);

        /// <summary>
        ///     Catch file operations exceptions.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="errorHandler">Method called if exception was thrown</param>
        void CatchFileSystemExceptions(Exception e, Action errorHandler);
    }
}