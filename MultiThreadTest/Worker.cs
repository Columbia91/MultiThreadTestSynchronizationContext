using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTest
{
    public class Worker
    {
        private bool _cancelled = false;

        public void Cancel()
        {
            _cancelled = true;
        }

        public void Work(object param)
        {
            SynchronizationContext context = (SynchronizationContext)param;

            for (int i = 0; i <= 99; i++)
            {
                if (_cancelled)
                    break;

                Thread.Sleep(50);

                context.Send(OnProcessChanged, i);
            }

            context.Send(OnworkCompleted, _cancelled);
        }

        private void OnProcessChanged(object i)
        {
            if (ProcessChanged != null)
                ProcessChanged((int)i);
        }

        private void OnworkCompleted(object cancelled)
        {
            if (WorkCompleted != null)
                WorkCompleted((bool)cancelled);
        }

        public event Action<int> ProcessChanged;
        public event Action<bool> WorkCompleted;
    }
}
