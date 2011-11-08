using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldServer.Tasks
{
    public class Signal
    {
        static int nextId = int.MinValue;

        int id = nextId++;
        List<TaskItem> tasks = new List<TaskItem>();
        bool isSet = true;

        public void Set()
        {
            if (isSet)
                return;
            isSet = true;
            //decrement the wait count of all tasks waiting for thsi signal
            foreach (TaskItem task in tasks)
                if (--task.Data == 0)
                    //if the wait count is zero, the task isn't waiting for any more signals, so re-schedule it
                    task.Scheduler.AddToActive(task);
            tasks.Clear();
        }

        internal void Add(TaskItem task)
        {
            //signal only becomes unset when it has tasks
            if (isSet)
                isSet = false;
            //the signal keeps a list of tasks that are waiting for it
            tasks.Add(task);
            //use the task's data for tracking the number of signals it's still waiting for
            task.Data++;
        }
    }
}
