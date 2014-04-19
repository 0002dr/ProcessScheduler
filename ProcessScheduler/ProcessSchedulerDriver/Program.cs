// Dana Zhang
// Operating Systems 
// 2011 Fall
// Project 2 - Process Scheduler
// This program simulates the allocation of jobs to a single processor using four different algorithms:
// FCFS, Round-Robin, SPN, and SRT. Corresponding data is calculated and output to four different files.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchedulingLib;
namespace ProcessSchedulerDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            Scheduler scheduler = new Scheduler(); // instantiate a Scheduler to schedule all processes
            Pause();
        }
        public static void Pause()
        {
            Console.WriteLine("Press [ENTER] to continue..");
            Console.ReadLine();
        }
    }
}
