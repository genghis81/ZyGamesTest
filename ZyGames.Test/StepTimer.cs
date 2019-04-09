using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ZyGames.Test
{
    public class StepTimer
    {
        private Stopwatch _watch;
        private List<double> _watchLog;
        private StringBuilder _error;

        public StepTimer()
        {
            _watch = new Stopwatch();
            _error = new StringBuilder();
            _watchLog = new List<double>();
        }


        public int Runtimes { get; private set; }

        public double TotalTime { get; private set; }

        public double AveTime { get; private set; }

        public double MinTime { get; private set; }

        public double MaxTime { get; private set; }

        public int SuccessNum { get; private set; }

        public int FailNum { get; private set; }


        public void Reset()
        {
            Runtimes = 0;
            FailNum = 0;
            SuccessNum = 0;
            TotalTime = 0;
            AveTime = 0;
            MinTime = 0;
            MaxTime = 0;
            _error.Clear();
            _watchLog.Clear();
        }

        public string Error
        {
            get { return _error.ToString(); }
        }

        public void PushError(string error)
        {
            FailNum++;
            _error.AppendFormat("{0}>>{1}", DateTime.Now.ToString("HH:mm:ss"), error);
            _error.AppendLine();
        }

        public void Completed()
        {
            SuccessNum++;
        }

        public void StartTime()
        {
            Runtimes++;
            _watch.Restart();
        }

        public void StopTime()
        {
            _watch.Stop();
            _watchLog.Add(_watch.Elapsed.TotalMilliseconds);
        }

        public void DoResult()
        {
            var list = _watchLog.OrderBy(t => t).ToList();
            double total = 0;
            int count = list.Count;
            if (list.Count > 3)
            {
                count = count - 2;
                MinTime = list[0];
                MaxTime = list[list.Count - 1];
                for (int i = 1; i < list.Count - 1; i++)
                {
                    total += list[i];
                }
                TotalTime = total + MinTime + MaxTime;
            }
            else if (list.Count > 0)
            {
                MinTime = list[0];
                MaxTime = list[list.Count - 1];
                for (int i = 0; i < list.Count; i++)
                {
                    total += list[i];
                }
                TotalTime = total;
            }
            AveTime = total / (count > 0 ? count : 1);

        }
    }
}