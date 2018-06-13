# Performance
Performance related things



# Boxing - Unboxing Bench

``` ini

BenchmarkDotNet=v0.10.1, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i7-5500U CPU 2.40GHz, ProcessorCount=4
Frequency=2338337 Hz, Resolution=427.6544 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.7.2633.0
  Job-QTBGET : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.7.2633.0

LaunchCount=3  RunStrategy=ColdStart  TargetCount=20  
UnrollFactor=1  WarmupCount=1  

```
Method | Mean | StdErr | StdDev | Median | Gen 0 | Gen 1 | Gen 2 | Allocated
------------------------- | -------------- | ------------ | -------------- |-------------- | ------------ | ------------ |---------- | ----------
ClassTupleAsKey | 7,001.1404 ms | 138.3584 ms | 1,071.7192 ms | 6,607.0421 ms | 101050.0000 |  37550.0000 | 4050.0000 | 600.71 MB |
StructTupleAsKey | 7,046.6457 ms |  28.2645 ms |   218.9356 ms | 6,958.6388 ms | 638950.0000 | 135050.0000 | 6000.0000 |   1.65 GB |
MyStructAsKey | 2,399.8246 ms |  12.4823 ms |    96.6873 ms | 2,388.9916 ms | 127100.0000 |  49550.0000 | 4050.0000 |    765 MB | 
MyStructAsKeyWithCompare |   955.2248 ms |   5.2553 ms |    40.7071 ms |   938.4278 ms |  11950.0000 |   8900.0000 | 2950.0000 |   59.6 MB |







# Exceptions Bench

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i7-5500U CPU 2.40GHz, ProcessorCount=4
Frequency=2338337 Hz, Resolution=427.6544 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.7.2633.0
  DefaultJob : Clr 4.0.30319.42000, 32bit LegacyJIT-v4.7.2633.0


```
 |                        Method |            Mean |        StdDev |    Scaled | Scaled-StdDev |
 |------------------------------ |---------------- |-------------- |---------- |-------------- |
 |      ErrorCodeWithReturnValue |      12.2150 ns |     0.1463 ns |      1.00 |          0.00 |
 |       RareExceptionStackTrace |      26.4149 ns |     0.2138 ns |      2.16 |          0.03 |
 | RareExceptionMediumStackTrace |      70.1164 ns |     0.4487 ns |      5.74 |          0.07 |
 |   RareExceptionDeepStackTrace |     108.7963 ns |     0.9961 ns |      8.91 |          0.13 |
 |             ExceptionTryCatch |  13,891.9944 ns |   149.2097 ns |  1,137.44 |         17.51 |
 |              ExceptionMessage |  13,949.2975 ns |    70.2262 ns |  1,142.13 |         14.14 |
 |        ExceptionMediumMessage |  20,045.6745 ns |   155.5210 ns |  1,641.28 |         22.37 |
 |       ExceptionMediumTryCatch |  20,385.6284 ns |   186.9361 ns |  1,669.12 |         24.09 |
 |          ExceptionDeepMessage |  25,456.5397 ns |   231.1298 ns |  2,084.31 |         29.94 |
 |         ExceptionDeepTryCatch |  25,535.8895 ns |   127.3833 ns |  2,090.80 |         25.86 |
 |     CachedExceptionStackTrace |  36,777.4288 ns |   187.0627 ns |  3,011.23 |         37.36 |
 |           ExceptionStackTrace |  45,445.3053 ns |   315.2136 ns |  3,720.93 |         49.18 |
 |     ExceptionMediumStackTrace | 116,205.4792 ns |   639.9017 ns |  9,514.56 |        119.63 |
 |       ExceptionDeepStackTrace | 175,454.3172 ns | 1,559.0901 ns | 14,365.68 |        204.93 |





# ConfigurationManager


    BenchmarkDotNet=v0.9.7.0
    OS=Microsoft Windows NT 6.1.7601 Service Pack 1
    Processor=Intel(R) Core(TM) i7-4770 CPU 3.40GHz, ProcessorCount=8
    Frequency=3312861 ticks, Resolution=301.8539 ns, Timer=TSC
    HostCLR=MS.NET 4.0.30319.42000, Arch=32-bit RELEASE
    JitModules=clrjit-v4.7.2558.0

    Type=ReadAppSettings  Mode=SingleRun  LaunchCount=3  
    WarmupCount=1  TargetCount=30  

                          Method |        Median |        StdDev | Scaled |
    ---------------------------- |-------------- |-------------- |------- |
     AppSettingsMyHashTableValue |   301.8500 ns |   170.5169 ns |   0.09 |
           HelperAppSettingValue | 1,509.2700 ns | 1,201.2947 ns |   0.45 |
                 AppSettingValue | 3,320.3900 ns |   965.1868 ns |   1.00 |
