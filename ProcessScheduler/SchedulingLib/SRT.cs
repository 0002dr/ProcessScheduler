using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggerLib;
namespace SchedulingLib
{
    /// <summary>
    /// This class is an implementation of the Shorted Remain Time scheduling technique. It is a preempting 
    /// version of SPN. 
    /// </summary>
    class SRT
    {
        ILogger logger = new Logger("SRT.csv");
        ProcessElement process = new ProcessElement();
        /// <summary>
        /// The method implements the shorted remain time scheduling algorithm. A process is preempted when
        /// the newly arrived process has shorter remaining service time. When a process is finished, the 
        /// processor then chooses the next process with the shortest remaining time
        /// </summary>
        /// <param name="processList">a list of processes to be executed</param>
        public void SRT_Schedule(List<ProcessElement> processList)
        {
            int totalWaitTime = 0; // keeps track of the total wait time of all jobs
            int totalTurnAroundTime = 0; // keeps track of he total turnaround time of all jobs
            double avgWaitTime = 0; // average wait time of all jobs
            double avgTurnAroundTime = 0; // average turnaround time of all jobs
            double wTDeviation = 0; // wait time standard deviation
            double taTDeviation = 0; // turnaround time standard deviation
            double sumOfWTSquares = 0; // temp values to use in calculating
            double sumOfTATSquares = 0; // the standard deviations
            int numOfJobs = processList.Count; // stores the number of job in the list
            int count = 0; // keeps track of how many processes have been completed
            int timer = 0; // keeps track of time
            ProcessElement current = new ProcessElement(); // process currently being 
            ProcessElement next = new ProcessElement(); // the next process in the ready queue
            List<ProcessElement> waitlist = new List<ProcessElement> { };
            // if there is no process, exit the method
            if (processList.Count == 0)
            {
                logger.Log("There are no processes to be executed.");
                return;
            }
            // while there are incomplete processes
            while (count < processList.Count-1)
            {
                // get a sublist of processes that have arrived and uncompleted
                List<ProcessElement> sublist = processList.Where(p => (p.RemainTime > 0 && p.ArriveTime <= timer)).ToList();
                // if sublist is not empty
                if (sublist.Count != 0)
                {
                    current = sublist.First(); // set the first element to be the current process
                    // go through each process in the sublist
                    foreach (ProcessElement p in sublist)
                    {
                        // and find the process with the shorted remaining time
                        if (p.RemainTime < current.RemainTime)
                        {
                            current = p; // set that to be the next process executed
                            if (current.RemainTime == current.ExeTime)
                                current.WaitTime = timer - current.ArriveTime;
                        }
                    }
                    current.RemainTime--; // execute the process
                    timer++; // update the timer
                    current.TurnAroundTime = timer - current.ArriveTime; // update the turnaround time
                    if (current.RemainTime == 0) // if the process is finished
                    {
                        count++; // increment the count value
                        processList.ElementAt(current.JobNumber - 1).TurnAroundTime = current.TurnAroundTime;
                        // copy the turnaround time to the process list
                    }
                }
                else
                {
                    while (processList.ElementAt(count).ArriveTime > timer && count!=processList.Count-1)
                        timer++; // if no sublist is generated, increment the timer while waiting for a new process to arrive
                }
            }
            //logging data in file
            logger.Log("Shortest Remaining Time Algorithm\n");
            logger.DisableFileLogging();
            logger.Log("Data has been logged to file SRT.csv\n\n");
            logger.DisableConsoleLogging();
            logger.EnableFileLogging();
            logger.Log("Arrival Time,Wait Time,Turnaround Time,\n");
            try
            {
                foreach (ProcessElement p in processList)
                {
                    //p.WaitTime = p.TurnAroundTime - p.ExeTime; // wait time is equal to the difference between the turnaround time and execution time
                    logger.Log(p.ArriveTime.ToString() + "," + p.WaitTime.ToString() + "," + p.TurnAroundTime + ",\n");
                    totalWaitTime += p.WaitTime;
                    totalTurnAroundTime += p.TurnAroundTime;
                }
                // standard deviation = the square root of (the (the sum of the square of the difference between each item and the mean)/number of elements)
                avgWaitTime = (double)totalWaitTime / (double)numOfJobs; // calculate average wait time
                avgTurnAroundTime = (double)totalTurnAroundTime / (double)numOfJobs; // calculate average turnaround time
                foreach (ProcessElement p in processList)
                {
                    sumOfWTSquares += Math.Pow(p.WaitTime - avgWaitTime, 2);
                    sumOfTATSquares += Math.Pow(p.TurnAroundTime - avgTurnAroundTime, 2);
                }
                wTDeviation = Math.Sqrt(sumOfWTSquares / count);
                taTDeviation = Math.Sqrt(sumOfTATSquares / count);
                logger.Log("\n\nAvg WaitTime,Avg TurnaroundTime,Std Dev of WaitTime, Std Dev of TurnaroundTime,\n");
                logger.Log(avgWaitTime.ToString("f2") + "," + avgTurnAroundTime.ToString("f2") + "," + wTDeviation.ToString("f2") + "," + taTDeviation.ToString("f2") + ",\n");
            }
            catch (DivideByZeroException de)
            {
                logger.EnableConsoleLogging();
                logger.Log(de.Message);
            }
        }
    }
}
