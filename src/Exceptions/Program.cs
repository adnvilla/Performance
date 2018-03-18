using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    [Config(typeof(Config))]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    public class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Program>();
        }

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(RPlotExporter.Default);
            }
        }

        public struct ResultAndErrorCode<T>
        {
            public T Result;
            public int ErrorCode;
        }

        [Benchmark(Baseline = true)]
        public ResultAndErrorCode<string> ErrorCodeWithReturnValue()
        {
            var result = new ResultAndErrorCode<string>();
            result.Result = null;
            result.ErrorCode = 5;
            return result;
        }

        [Benchmark]
        public Exception ExceptionTryCatch()
        {
            try
            {
                Level20(); // start *all* the way down the stack
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                return ioex;
            }
        }

        [Benchmark]
        public Exception ExceptionMediumTryCatch()
        {
            try
            {
                Level10(); // start 1/2 way down
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Force collection of a full StackTrace
                return ioex;
            }
        }

        [Benchmark]
        public Exception ExceptionDeepTryCatch()
        {
            try
            {
                Level1();
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Force collection of a full StackTrace
                return ioex;
            }
        }

        [Benchmark]
        public string ExceptionMessage()
        {
            try
            {
                Level20(); // start *all* the way down the stack
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Only get the simple message from the Exception (don't trigger a StackTrace collection)
                return ioex.Message;
            }
        }

        [Benchmark]
        public string ExceptionMediumMessage()
        {
            try
            {
                Level10(); // start 1/2 way down
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Only get the simple message from the Exception (don't trigger a StackTrace collection)
                return ioex.Message;
            }
        }

        [Benchmark]
        public string ExceptionDeepMessage()
        {
            try
            {
                Level1();
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Only get the simple message from the Exception (don't trigger a StackTrace collection)
                return ioex.Message;
            }
        }

        [Benchmark]
        public string ExceptionStackTrace()
        {
            try
            {
                Level20(); // start *all* the way down the stack
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Force collection of a full StackTrace
                return ioex.StackTrace;
            }
        }

        private static readonly InvalidOperationException cachedEx = new InvalidOperationException("Benchmark");

        [Benchmark]
        public string CachedExceptionStackTrace()
        {
            try
            {
                throw cachedEx;
            }
            catch (InvalidOperationException ioEx)
            {
                return ioEx.StackTrace;
            }
        }

        [Benchmark]
        public string RareExceptionStackTrace()
        {
            try
            {
                RareLevel20(); // start all the way down
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Force collection of a full StackTrace
                return ioex.StackTrace;
            }
        }

        [Benchmark]
        public string ExceptionMediumStackTrace()
        {
            try
            {
                Level10(); // start 1/2 way down
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Force collection of a full StackTrace
                return ioex.StackTrace;
            }
        }

        [Benchmark]
        public string RareExceptionMediumStackTrace()
        {
            try
            {
                RareLevel10(); // start 1/2 way down
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Force collection of a full StackTrace
                return ioex.StackTrace;
            }
        }

        [Benchmark]
        public string ExceptionDeepStackTrace()
        {
            try
            {
                Level1();
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Force collection of a full StackTrace
                return ioex.StackTrace;
            }
        }

        [Benchmark]
        public string RareExceptionDeepStackTrace()
        {
            try
            {
                RareLevel1();
                return null; //Prevent Error CS0161: not all code paths return a value
            }
            catch (InvalidOperationException ioex)
            {
                // Force collection of a full StackTrace
                return ioex.StackTrace;
            }
        }

        // Impact Probability (cumulative) 3.7e-04 = 0.00037 = 0.037000000% chance of Earth impact
        // or 1 in 2,700 chance or 99.96300000% chance the asteroid will miss the Earth
        // See http://www.universetoday.com/69640/researchers-say-asteroid-has-1-in-1000-chance-of-hitting-earth-in-2182/
        // and http://neo.jpl.nasa.gov/risk/a101955.html#legend and http://neo.jpl.nasa.gov/cgi-bin/ip?3.7e-04
        /// <summary> 1 in 2,700 chance - Impact Probability (cumulative) = 0.037000000% (99.96300000% chance the asteroid will miss the Earth)</summary>
        private static long chanceOfAsteroidHit = 2700;

        private static long counter;

        [Setup]
        public void Setup()
        {
            counter = 0;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level1()
        {
            Level2();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level2()
        {
            Level3();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level3()
        {
            Level4();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level4()
        {
            Level5();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level5()
        {
            Level6();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level6()
        {
            Level7();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level7()
        {
            Level8();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level8()
        {
            Level9();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level9()
        {
            Level10();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level10()
        {
            Level11();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level11()
        {
            Level12();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level12()
        {
            Level13();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level13()
        {
            Level14();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level14()
        {
            Level15();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level15()
        {
            Level16();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level16()
        {
            Level17();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level17()
        {
            Level18();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level18()
        {
            Level19();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level19()
        {
            Level20();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Level20()
        {
            counter++;
            // will *always* happen, but makes it a fair comparision
            // so both benchmarks pay the cost 'counter++' and 'counter % chanceOfAsteroidHit'
            if (counter % chanceOfAsteroidHit >= 0)
                throw new InvalidOperationException("Deep Stack Trace");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel1()
        {
            RareLevel2();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel2()
        {
            RareLevel3();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel3()
        {
            RareLevel4();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel4()
        {
            RareLevel5();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel5()
        {
            RareLevel6();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel6()
        {
            RareLevel7();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel7()
        {
            RareLevel8();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel8()
        {
            RareLevel9();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel9()
        {
            RareLevel10();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel10()
        {
            RareLevel11();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel11()
        {
            RareLevel12();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel12()
        {
            RareLevel13();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel13()
        {
            RareLevel14();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel14()
        {
            RareLevel15();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel15()
        {
            RareLevel16();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel16()
        {
            RareLevel17();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel17()
        {
            RareLevel18();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel18()
        {
            RareLevel19();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel19()
        {
            RareLevel20();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RareLevel20()
        {
            counter++;
            // will *rarely* happen (1 in 2700)
            if (counter % chanceOfAsteroidHit == 1)
                throw new InvalidOperationException("Deep Stack Trace - Rarely triggered");
        }
    }
}