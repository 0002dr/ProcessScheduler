using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggerLib;
namespace SchedulingLib
{
    /// <summary>
    /// Implements the first come first served process scheduling algorithm. Process that arrives first
    /// will finish executing first. 
    /// </summary>
    public class FCFS
    {
        ILogger logger = new Logger("FCFS.csv");
        ProcessElement process = new ProcessElement();
        // a method that schedules processes based on the order of their entries. 
        // accept a process list
        // return nothing
        public void FCFS_Schedule(List<ProcessElement> processList)
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
            int count = 0; // index variable
            int timer = 0; // a variable to keep track of time
            if (processList.Count == 0)
            {
                logger.Log("There are no processes to be executed.");
                return;
            }
            // for each element in processList, calculate its data
            while (count < processList.Count)
            {
                process = processList.ElementAt(count); // each of the element in the list
                while (timer < process.ArriveTime) // if the process has not yet arrive, wait till it arrives
                    timer++;
                timer += process.ExeTime; // update the timer after the execution of the process
                process.TurnAroundTime = timer - process.ArriveTime; // each process's turnaround time equals exe time + wait time
                count++; // increment the index variable to get the next process 
            }
            // logging data to file 
            logger.Log("First Come First Served Algorithm\n");
            logger.DisableFileLogging();
            logger.Log("Data has been logged to file RoundRobin.csv\n\n");
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
            // catch any error caused by dividing 0
            catch (DivideByZeroException de)
            {
                logger.EnableConsoleLogging();
                logger.Log(de.Message);
            }
        }
    }
}
