using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggerLib;
namespace SchedulingLib
{
    /// <summary>
    /// This class implements the Shorted Process Next scheduling algorithm. It executes the ready process
    /// with the shorted processing time first without preempting it.
    /// </summary>
    class SPN
    {
        ILogger logger = new Logger("SPN.csv");
        ProcessElement process = new ProcessElement();
        /// <summary>
        /// This method is an implementation of the shorted process next scheduling algorithm.
        /// It executes the ready process with the shorted processing time first without preempting it.
        /// </summary>
        /// <param name="processList">A list of processes to be executed</param>
        public void SPN_Schedule(List<ProcessElement> processList)
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
            int count = 0; // keeps track of how many processes have been executed
            int timer = 0; // keeps track of time
            int minExeTime = 0; // stores the shorted processing time
            ProcessElement current = new ProcessElement(); // process currently being executed
            // if there is no process, exit the method
            if (processList.Count == 0)
            {
                logger.Log("There are no processes to be executed.");
                return;
            }
            current = processList.First(); // otherwise, set the first process in the list to current 
            // if current process has not arrived yet, increment the timer until it arrives.
            while (current.ArriveTime > timer)
                timer++;
            // while there are still processes in the process list
            while (count < processList.Count-1)
            {
                // if current process is incomplete
                if (current.RemainTime != 0)
                {
                    count++; // increment the count value
                    timer += current.ExeTime; // update the timer
                    current.RemainTime -= current.ExeTime; // execute the program
                    current.TurnAroundTime = timer - current.ArriveTime; // update its turnaround time
                    processList.ElementAt(current.JobNumber - 1).TurnAroundTime = current.TurnAroundTime;
                    //copy the TAT info back into the processList
                }
                // query for a sublist that contains the ready processes that are to be executed
                List<ProcessElement> sublist = processList.Where(p => (p.RemainTime > 0 && p.ArriveTime <= timer)).ToList();
                // if sublist is not empty
                if (sublist.Count != 0)
                {
                    minExeTime = sublist.First().ExeTime; // set the minimum service time to the first element in the sublist
                    current = sublist.First(); // set current process to the first element in the sublist
                    // go through each process in the sublist
                    for (int i = 0; i < sublist.Count; i++)
                    {
                        // find the process with the shortest processing time and set that to the next process to be executed
                        if (minExeTime > sublist.ElementAt(i).ExeTime)
                        {
                            minExeTime = sublist.ElementAt(i).ExeTime;
                            current = sublist.ElementAt(i);
                        }
                    }
                }
                else // if no sublist is found, increment the timer until a new process arrives
                {
                    while (processList.ElementAt(count).ArriveTime > timer)
                        timer++;
                }
            }
            //logging data in file
            logger.Log("Shortest Process Next Algorithm\n");
            logger.DisableFileLogging();
            logger.Log("Data has been logged to file SPN.csv\n\n");
            logger.DisableConsoleLogging();
            logger.EnableFileLogging();
            logger.Log("Arrival Time,Wait Time,Turnaround Time,\n");
            try
            {
                foreach (ProcessElement p in processList)
                {
                    p.WaitTime = p.TurnAroundTime - p.ExeTime; // wait time is equal to the difference between the turnaround time and execution time
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
