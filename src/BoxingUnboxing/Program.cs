using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

namespace BoxingUnboxing
{
   public static class Program
   {
      public static void Main(string[] args)
      {

        // ClassTupleAsKey();
        // Reset();
        // // DURATION: 8,479 miliseconds


        // StructTupleAsKey();
        // Reset();
        // // DURATION: 8,880 miliseconds


        // MyStructAsKey();
        // Reset();
        // // DURATION: 2,896 miliseconds


        //MyStructAsKeyWithCompare();
        // // DURATION: 1,442 miliseconds

         var summary = BenchmarkRunner.Run<DictionaryAdd>();

      }

      private static void Reset()
      {
         GC.Collect();
         Thread.Sleep(500);
      }

      
   }

   public class Config : ManualConfig
   {
      public Config()
      {
         Add(Job.Dry.WithWarmupCount(1).WithLaunchCount(3).WithTargetCount(20));
      }
   }

}