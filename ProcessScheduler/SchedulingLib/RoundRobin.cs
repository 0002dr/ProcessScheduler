using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggerLib;
namespace SchedulingLib
{
    /// <summary>
    /// This class implements the round-robin scheduling algorithm. Each process is given a certain quantum
    /// to execute. If the quantum expires and the process is uncompleted, it is preempted into a ready queue.
    /// If a new process arrives, it is put in the ready queue. Then the processor selects the next process
    /// in the ready queue following the FCFS pricipal.
    /// process in the ready queue
    /// </summary>
    class RoundRobin
    {
        ILogger logger = new Logger("RoundRobin.csv"); // instantiate a logger and specifiy destination
        ProcessElement process = new ProcessElement(); // instantiate a ProcessElement

        /// <summary>
        /// This method implements the round robin scheduling technique. Each process is given a quantum
        /// to execute and is put into a ready stack when a new process arrives
        /// </summary>
        /// <param name="processList">List of processes to be executed</param>
        public void RoundRobin_Schedule(List<ProcessElement> processList)
        {
            Queue<ProcessElement> readyQueue = new Queue<ProcessElement> { };// a list of processes preempted
            ProcessElement current = new ProcessElement(); // process currently being executed
            ProcessElement next = new ProcessElement(); // the next process in the ready queue
            const int QUANTUM = 4; // time slice given each process to execute being preempted
            int numOfJobs = processList.Count; // number of jobs in the ready queue
            int count = 0; // an index to refer to the job in the list 
            int timer = 0; // keeps track of time
            int timeAllocated = 0; // time allocated for each process
            int totalWaitTime = 0; // keeps track of the total wait time of all jobs
            int totalTurnAroundTime = 0; // keeps track of he total turnaround time of all jobs
            double avgWaitTime = 0; // average wait time of all jobs
            double avgTurnAroundTime = 0; // average turnaround time of all jobs
            double wTDeviation = 0; // wait time standard deviation
            double taTDeviation = 0; // turnaround time standard deviation
            double sumOfWTSquares = 0; // temp values to use in calculating
            double sumOfTATSquares = 0; // the standard deviations
            // initially, each process's remaining time is its service time

            foreach (ProcessElement p in processList)
                p.RemainTime = p.ExeTime;
            current = processList.ElementAt(count); // set the first job to be the current executing job
            // if there are jobs in the processList
            if (processList.Count != 0) // put the first job in the readyQueue
                readyQueue.Enqueue(processList.First());
            else // otherwise program terminates
            {
                logger.Log("There are no processes to be executed.");
                return;
            }
            // when there are still processes in the ready queue
            while (current.ArriveTime > timer) // if the first process has not arrived, increment the timer
                timer++;    // until it arrives
            while (readyQueue.Count != 0)
            {
                // if the process has not arrived yet, wait until it arrives
                current = readyQueue.Dequeue(); // select the first process in the queue to execute
                if (current.ExeTime == current.RemainTime)
                    processList.ElementAt(current.JobNumber - 1).WaitTime = timer - current.ArriveTime;
                // when a process arrives and it has not finished executing 
                if (current.RemainTime > 0 && current.RemainTime <= QUANTUM)
                {
                    timeAllocated = current.RemainTime; // time allocated is its remain time
                    current.RemainTime -= timeAllocated; // execute the job
                }
                else
                {
                    timeAllocated = QUANTUM; // otherwise, time allocated is the quantum specified
                    current.RemainTime -= timeAllocated; // execute the job
                }
                timer += timeAllocated; // update the timer
                timeAllocated = 0; //reset the time that's been allocated
                current.TurnAroundTime = timer - current.ArriveTime; // update current process's turnaround time
                // if there are other incomplete processes in the ready queue, update their TAT
                if (readyQueue.Count != 0)
                {
                    foreach (ProcessElement p in readyQueue)
                    {
                        if (p.RemainTime != 0)
                            p.TurnAroundTime = timer - p.ArriveTime;
                    }
                }
                // if the current process is completed, copy its TAT to the process list
                if (current.RemainTime == 0)
                {
                    processList.ElementAt(current.JobNumber - 1).TurnAroundTime = current.TurnAroundTime;
                }
                // when there are still processes expected, go through each expected process
                // and find the processes that have arrived
                for (int i = count; i < numOfJobs; ++i)
                {
                    if (count < numOfJobs - 1)
                    {
                        count++; // increment the count value
                        next = processList.ElementAt(count); // set next to the next process in the list
                        // if the job arrives and its remain time is not 0
                        if (timer >= next.ArriveTime && next.RemainTime != 0)
                        {
                            readyQueue.Enqueue(next); // add it to the ready queue

                        }
                        else
                            count--; // otherwise, restore the count value
                        if (next.ExeTime == 0) // if the process does not take any time to execute, move on
                            count++;
                    }
                }
                // if current process is not finished, add it back to the ready queue
                if (current.RemainTime != 0)
                    readyQueue.Enqueue(current);
            }
            //logging data in file

            logger.Log("Round-Robin Algorithm\n");
            logger.DisableFileLogging();
            logger.Log("Data has been logged to file RoundRobin.csv\n\n");
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
