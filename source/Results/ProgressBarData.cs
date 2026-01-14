using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    internal class ProgressBarData
    {
        private Label _progressLabel;

        internal int FormValue = 0;
        internal int CurrentValue = 0;
        internal int Total = 0;

        internal ProgressBarData(Label label)
        {
            _progressLabel = label;
        }

        internal void Increment()
        {
            Interlocked.Increment(ref CurrentValue);
        }

        internal void Print()
        {
            int currentVolatileValue = Volatile.Read(ref CurrentValue);
            if (currentVolatileValue > FormValue)
            {
                FormValue = currentVolatileValue;
                _progressLabel.Text = $"{currentVolatileValue} / {Total}";
            }
        }
    }
}
