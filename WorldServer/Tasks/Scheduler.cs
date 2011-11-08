using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WorldServer.Tasks
{
    public sealed class Scheduler
    {
        TaskList active, sleeping;

        public Scheduler()
        {
            active = new TaskList(this);
            sleeping = new TaskList(this);
        }

        public void AddTask(IEnumerable task) {
            active.Append(new TaskItem(task.GetEnumerator(), this));
        }

        public void AddTask(IEnumerator task)
        {
            active.Append(new TaskItem(task, this));
        }

        public void Run()
        {
            //cache this, it's expensive to access DateTime.Now
            long nowTicks = DateTime.Now.Ticks;

            //move woken tasks back into the active list
            var en = sleeping.GetEnumerator();
            while (en.MoveNext())
                if (en.Current.Data < nowTicks)
                    en.MoveCurrentToList(active);

            //run all the active tasks
            en = active.GetEnumerator();
            while (en.MoveNext())
            {
                //run each task's enumerator for one yield iteration
                IEnumerator t = en.Current.Task;
                if (!t.MoveNext())
                {
                    //it finished, so remove it
                    en.RemoveCurrent();
                    continue;
                }

                //check the current state
                object state = t.Current;
                if (state == null)
                {
                    //it's just cooperatively yielding, state unchanged 
                    continue;
                }
                else if (state is TimeSpan)
                {
                    //it wants to sleep, move to the sleeping list. we use the Data property for the wakeup time 
                    en.Current.Data = nowTicks + ((TimeSpan)state).Ticks;
                    en.MoveCurrentToList(sleeping);
                } else if (state is Signal) {
	                TaskItem task = en.RemoveCurrent ();
	                task.Data = 0;
	                ((Signal)state).Add (task);
                } else if (state is ICollection<Signal>) {
	                TaskItem task = en.RemoveCurrent ();
	                task.Data = 0;
	                foreach (Signal s in ((ICollection<Signal>)state))
		                s.Add (task);
                }
                else if (state is bool) {
                    if ((bool)state == false)
                    {
                        en.RemoveCurrent();
                        continue;
                    }
                }
                else if (state is IEnumerable)
                {
                    throw new NotImplementedException("Nested tasks are not supported yet");
                }
                else
                {
                    throw new InvalidOperationException("Unknown task state returned:" + state.GetType().FullName);
                }
            }
        }

        internal void AddToActive(TaskItem task)
        {
            active.Append(task);
        }
    }
}
