using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
namespace WpfApp14 {
    [SimpleJob(RunStrategy.ColdStart, targetCount: 1)]
    public class Test {
        Window window;
        void DoEvents() {
            window.Dispatcher.Invoke(DispatcherPriority.Background,
                                          new Action(delegate { }));
        }
        [Benchmark]
        [STAThread]
        public void Resize() {
            window = new Window();
            window.Show();
            window.Width++;
            DoEvents();
            window.Width--;
            DoEvents();
            window.Close();
        }
    }
}
