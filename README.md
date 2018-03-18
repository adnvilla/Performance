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
                   Method |          Mean |      StdErr |        StdDev |        Median |       Gen 0 |       Gen 1 |     Gen 2 | Allocated |
------------------------- |-------------- |------------ |-------------- |-------------- |------------ |------------ |---------- |---------- |
          ClassTupleAsKey | 7,001.1404 ms | 138.3584 ms | 1,071.7192 ms | 6,607.0421 ms | 101050.0000 |  37550.0000 | 4050.0000 | 600.71 MB |
         StructTupleAsKey | 7,046.6457 ms |  28.2645 ms |   218.9356 ms | 6,958.6388 ms | 638950.0000 | 135050.0000 | 6000.0000 |   1.65 GB |
            MyStructAsKey | 2,399.8246 ms |  12.4823 ms |    96.6873 ms | 2,388.9916 ms | 127100.0000 |  49550.0000 | 4050.0000 |    765 MB |
 MyStructAsKeyWithCompare |   955.2248 ms |   5.2553 ms |    40.7071 ms |   938.4278 ms |  11950.0000 |   8900.0000 | 2950.0000 |   59.6 MB |
