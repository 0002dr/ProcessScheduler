using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace LoggerLib
{
    public class Logger: ILogger
    {
        // private data members to store logging status
        private bool consoleLogEnabled = true;
        private bool fileLogEnabled = true;
        private bool loggingStatus = true;
        // private string to store logger info
        private string info;
        // private fields used to write to output file.
        private string filename;
        private const string FILENAME = "LogFile.txt";
        StreamWriter sw;
        // properties
        public string Info
        {
            get 
            { 
                return info;
            }
            set 
            {
                info = value; 
            }
        }
        public bool FileLogEnabled
        {
            get
            {
                return fileLogEnabled;
            }
            set { fileLogEnabled = value; }
        }
        public bool ConsoleLogEnabled
        {
            get 
            { 
                return consoleLogEnabled;
            }
            set
            {
                consoleLogEnabled = value;
            }
        }
        // public constructor that accepts the name of a file and creates the file
        public Logger(string inputFilename)
        {
            try
            {
                if (inputFilename != FILENAME)
                    filename = inputFilename;
                else
                    filename = FILENAME;
                sw = File.CreateText(filename);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SystemException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // default constructor that supplies a name for the output file and creates it 
        public Logger()
        {
            try
            {
                filename = FILENAME;
                 sw = File.CreateText(filename);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SystemException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Log method that accepts a message and returns a boolean value
        /// </summary>
        /// <param name="line">accepts a string of content to log in to file</param>
        /// <returns>returns a boolean value to indicate whether or not anything has been written into the file</returns>
        public bool Log(string line)
        {
            try
            {
                loggingStatus = true; // default status is true
                info = "Last implemented time: " + File.GetLastWriteTime(filename).ToString(); // file info
                // if consoleLog is enabled, then write line to console and set status to true
                if (ConsoleLogEnabled) 
                {
                    Console.Write(line);
                    loggingStatus = true;
                }
                // if fileLog is enabled, then write line to file and set status to true
                if (FileLogEnabled)
                {
                    sw.Write(line);
                    sw.Flush();
                    loggingStatus = true;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SystemException e)
            {
                Console.WriteLine(e.Message);
            }
            return loggingStatus;
        }
        // methods below enable and/or disable file and/or console logging
        public void EnableFileLogging()
        {
            fileLogEnabled = true;
        }
        public void DisableFileLogging()
        {
            FileLogEnabled = false;
        }
        public void EnableConsoleLogging()
        {
            ConsoleLogEnabled = true;
        }
        public void DisableConsoleLogging()
        {
            ConsoleLogEnabled = false;
        }
    }
}
