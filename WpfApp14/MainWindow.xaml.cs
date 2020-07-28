using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using NUnit.Framework;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dispatcher dispatcher;

        public MainWindow()
        {
            InitializeComponent();
            dispatcher = Dispatcher;
            Loaded += MainWindow_Loaded;
        }

        public static Summary StartTest()
        {
            ManualConfig config = new ManualConfig()
      .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                .AddValidator(JitOptimizationsValidator.DontFailOnError)
                .AddDiagnoser(new PmcDiagnoser())
                .AddHardwareCounters(HardwareCounter.BranchInstructions)
                .AddLogger(ConsoleLogger.Default)
                .AddColumnProvider(DefaultColumnProviders.Instance);
            return BenchmarkRunner.Run<Test>(config);
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            StartTest();
        }
    }

    [TestFixture]
    public class TestAdapter {
        [Test]
        public void DoTest() {
            var res = MainWindow.StartTest();
            Assert.Less(int.Parse( res.Table.Columns[44].Content[0].Replace(",", "")), 115212800);
        }
    }
}
