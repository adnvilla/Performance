using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Collections.Specialized;

namespace ConfigurationManager
{
    [Config(typeof(Config))]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    public class ReadAppSettings
    {
        [Benchmark(Baseline = true)]
        public string AppSettingValue()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Setting"];
        }

        [Benchmark]
        public string HelperAppSettingVelue()
        {
            return AppSettingsHelper.AppSettings["Setting"];
        }
    }

    public static class AppSettingsHelper
    {
        public static NameValueCollection AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    lock (SyncRoot)
                    {
                        if (_appSettings == null)
                        {
                            _appSettings = System.Configuration.ConfigurationManager.AppSettings;
                        }
                    }
                }

                return _appSettings;
            }
        }

        private static readonly object SyncRoot = new object();
        private static NameValueCollection _appSettings;
    }
}