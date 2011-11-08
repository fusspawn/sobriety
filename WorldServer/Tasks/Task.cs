using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WorldServer.Tasks
{
    public class TaskItem
    {
        public readonly IEnumerator Task;
	    public TaskItem Next;
	    public Scheduler Scheduler;
	    public long Data;
 
	    public TaskItem (IEnumerator task, Scheduler scheduler)
	    {
		    this.Task = task;
		    this.Scheduler = scheduler;
	    }
    }
}
