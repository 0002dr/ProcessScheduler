using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingLib
{
    /// <summary>
    /// Properties of a process 
    /// </summary>
    public class ProcessElement
    {
        public int ArriveTime { get; set; } // stores the arrival time of each process
        public int ExeTime { get; set; } // stores the service time of each process
        public int WaitTime { get; set; } // stores the wait time of each process
        public int TurnAroundTime { get; set; } // stores the turnaround time of each process
        public int RemainTime { get; set; } // keeps track of the remain time of each process in the processor
        public int JobNumber { get; set; } // keeps track of the job number in the list
    }
}
