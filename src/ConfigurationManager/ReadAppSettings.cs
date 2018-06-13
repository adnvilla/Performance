using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

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
        public string HelperAppSettingValue()
        {
            return AppSettingsHelper.AppSettings["Setting"];
        }

        [Benchmark]
        public string AppSettingsMyHashTableValue()
        {
            return AppSettingsMyHashTableHelper.AppSettings["Setting"];
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

    public static class AppSettingsMyHashTableHelper
    {
        public static MyHashTable AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    lock (SyncRoot)
                    {
                        if (_appSettings == null)
                        {
                            var ap = System.Configuration.ConfigurationManager.AppSettings;

                            _appSettings = new MyHashTable();

                            for (int i = 0; i < ap.AllKeys.Length; i++)
                            {
                                _appSettings[ap.Keys.Get(i)] = ap.AllKeys[i];
                            }
                        }
                    }
                }

                return _appSettings;
            }
        }

        private static readonly object SyncRoot = new object();
        private static MyHashTable _appSettings;
    }


    public class MyHashTable
    {
        private Hashtable hashtable;

        public MyHashTable()
        {
            hashtable = new Hashtable();
        }

        public string this[string key]
        {
            get { return (string) hashtable[key]; }
            set { hashtable[key] = value; }
        }
    }
}