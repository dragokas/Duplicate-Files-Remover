using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DuplicatesFinder
{
    internal class ElapsedTimeWatcher
    {
        private Stopwatch _elapsedWatcher = new();
        private Label _searchTimeLabel;
        internal ElapsedTimeWatcher(Label searchTimeLabel)
        {
            _searchTimeLabel = searchTimeLabel;
        }
        internal void Start() => _elapsedWatcher.Start();
        internal void Stop() => _elapsedWatcher.Stop();
        internal void Reset() => _elapsedWatcher.Reset();
        internal void Print()
        {
            _searchTimeLabel.Invoke(new Action(() =>
            {
                _searchTimeLabel.Text = ((int)_elapsedWatcher.Elapsed.TotalSeconds).ToString() + " s.";
            }));
        }
    }
}
