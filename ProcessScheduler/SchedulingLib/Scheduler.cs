using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
namespace SchedulingLib
{
    /// <summary>
    /// The base class scheduler. It implements the interface and its default constructor calls different
    /// scheduling methods in different classes.
    /// </summary>
    public class Scheduler: IScheduler
    {
        int num = 0; // variable used to assign a job number to each process
        const string FILENAME = "process.txt"; // file to be read
        List<ProcessElement> processList = new List<ProcessElement> { };
        /// <summary>
        /// public default constructor that invokes the method GetData
        /// </summary>
        public Scheduler()
        {
            processList = GetData(); // call GetData() and store its returned value in processList
            // schedule the processes using FCFS algorithm
            FCFS fcfs = new FCFS(); 
            fcfs.FCFS_Schedule(processList);
            // schedule the processes using Round-Robin algorithm
            foreach (ProcessElement p in processList)
            {
                p.TurnAroundTime = 0; // TAT and WT of each process is initialized to 0
                p.WaitTime = 0; 
                p.RemainTime = p.ExeTime; // remain time is initially execution time of each process
                num++;
                p.JobNumber += num; // assign a job number to each process
            }
            RoundRobin rr = new RoundRobin();
            rr.RoundRobin_Schedule(processList);
            // schedule the processes using Shortest Process Next technique
            foreach (ProcessElement p in processList)
            {
                p.RemainTime = p.ExeTime;
            }
            SPN spn = new SPN();
            spn.SPN_Schedule(processList);
            // schedule the processes using Shorted Remaining Time technique
            foreach (ProcessElement p in processList)
            {
                p.RemainTime = p.ExeTime;
            }
            SRT srt = new SRT();
            srt.SRT_Schedule(processList);
        }

        #region IScheduler Members
        /// <summary>
        /// Reads a file containing processes to be executed and converts it to a list of processes
        /// </summary>
        /// <returns>a list of processes with two properties given in the file</returns>
        public List<ProcessElement> GetData()
        {
            string[] lines = File.ReadAllLines(FILENAME);
            string[] fields;
            string input;
            List<ProcessElement> processList = new List<ProcessElement> { };
            foreach (string line in lines) // parse each line in file
            {
                try
                {
                    if (line != "" && !line.StartsWith("#")) // empty lines and comments are ignored
                    {
                        ProcessElement process = new ProcessElement();
                        input = line.Trim();
                        input = Regex.Replace(input, @"\s+", " ");
                        fields = input.Split(' ');
                        process.ArriveTime = Convert.ToInt32(fields[0]); // the first number is the arrival time
                        process.ExeTime = Convert.ToInt32(fields[1]); // the second number is the service time
                        processList.Add(process);
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Format error: " + e.Message);
                }
            }
            return processList;
        }

        #endregion
    }
}
