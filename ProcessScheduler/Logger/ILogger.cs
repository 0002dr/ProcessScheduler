// Student Name
// Course
// Assignment Description
// Due Date

using System;

namespace LoggerLib
{
    /// <summary>
    /// An interface to define a logging facility
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Method to log some string data
        /// </summary>
        /// <param name="line">A string to put as a line of text in the log</param>
        /// <returns>True if line was logged properly, false otherwise</returns>
        bool Log(string line);

        /// <summary>
        /// Control method to turn on file-based logging
        /// </summary>
        void EnableFileLogging();

        /// <summary>
        /// Control method to turn off file-based logging
        /// </summary>
        void DisableFileLogging();

        /// <summary>
        /// Control method to turn on console-based logging
        /// </summary>
        void EnableConsoleLogging();

        /// <summary>
        /// Control method to turn off console-based logging
        /// </summary>
        void DisableConsoleLogging();

        /// <summary>
        /// String property to view details of the implementation
        /// </summary>
        string Info { get; }

        /// <summary>
        /// Boolean property which reflects if the log is currently logging to a file
        /// </summary>
        bool FileLogEnabled { get; }

        /// <summary>
        /// Boolean property which reflects if the log is currently logging to the console
        /// </summary>
        bool ConsoleLogEnabled { get; }
    }
}
