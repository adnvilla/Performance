using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ConfigurationManager
{
    class Program
    {
        static void Main(string[] args)
        {

            var summary = BenchmarkRunner.Run<ReadAppSettings>();
        }
    }

    public class Config : ManualConfig
    {
        public Config()
        {
            Add(Job.Dry.WithWarmupCount(1).WithLaunchCount(3).WithTargetCount(30));
        }
    }
}
