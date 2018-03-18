using BenchmarkDotNet.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BoxingUnboxing
{
   [Config(typeof(Config))]
   public class DictionaryAdd
   {
      /// <summary>
      /// Classes the tuple as key.
      ///
      ///           This is going to use the class implementation
      ///          of Tuple that was changed in .NET 4.0. It is
      ///          be inefficient for three reasons:
      ///          1) It is allocating a new class on the heap
      ///             for each and every key.
      ///          2) It is using the default object comparer
      ///             to check each item, causing inefficient
      ///             comparisons and...
      ///          3) Causing boxing of primitive values.
      ///
      /// </summary>
      [Benchmark]
      public static void ClassTupleAsKey()
      {
         var dictionary = new SortedDictionary<ClassTuple<int, string>, int>();

         for (var i = 0; i < 1000000; i++)
         {
            var key = new ClassTuple<int, string>(i % 1000, i.ToString());
            dictionary.Add(key, i);
         }
      }

      /// <summary>
      /// Structures the tuple as key.
      ///
      ///          This is going to use the struct implementation
      ///          of Tuple that was available from.NET 2.0
      ///          until 4.0. It is even less efficiente than the
      ///          class tuple for both reasons listed above, and
      ///          one new reason:
      ///          4) The StructTuple itself will be boxed when
      ///             being passed between comparers, causing
      ///             even more garbage collections to occur!
      ///
      /// </summary>
      [Benchmark]
      public static void StructTupleAsKey()
      {
         var dictionary = new SortedDictionary<StructTuple<int, string>, int>();

         for (var i = 0; i < 1000000; i++)
         {
            var key = new StructTuple<int, string>(i % 1000, i.ToString());
            dictionary.Add(key, i);
         }
      }

      /// <summary>
      /// Mies the structure as key.
      ///
      ///          Now we move to a custom struct implementation
      ///          for the dictionary key, but because we are using
      ///          a SortedDictionary we still need to implement
      ///          IComparable, which is not generic, and thus will
      ///          be slow for one reason:
      ///          1) The struct itself will be boxed when being
      ///             passed between comparers.
      ///
      /// </summary>
      [Benchmark]
      public static void MyStructAsKey()
      {
         var dictionary = new SortedDictionary<MyStructA, int>();

         for (var i = 0; i < 1000000; i++)
         {
            var key = new MyStructA(i % 1000, i.ToString());
            dictionary.Add(key, i);
         }
      }

      /// <summary>
      /// Mies the structure as key with compare.
      ///
      ///           This is pretty much as fast as we can get; no
      ///         heap allocations, no boxing, fast comparisons.
      ///
      /// </summary>
      [Benchmark]
      public static void MyStructAsKeyWithCompare()
      {
         var dictionary = new SortedDictionary<MyStructB, int>(MyStructBComparer.Instance);

         for (var i = 0; i < 1000000; i++)
         {
            var key = new MyStructB(i % 1000, i.ToString());
            dictionary.Add(key, i);
         }
      }

      public class ClassTuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable
      {
         private readonly T1 _item1;
         private readonly T2 _item2;

         public T1 Item1 => _item1;
         public T2 Item2 => _item2;

         public ClassTuple(T1 item1, T2 item2)
         {
            _item1 = item1;
            _item2 = item2;
         }

         public override bool Equals(object obj)
         {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
         }

         bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
         {
            var objTuple = other as ClassTuple<T1, T2>;
            if (objTuple == null) return false;
            return comparer.Equals(_item1, objTuple._item1) && comparer.Equals(_item2, objTuple._item2);
         }

         int IComparable.CompareTo(object obj)
         {
            return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
         }

         int IStructuralComparable.CompareTo(object other, IComparer comparer)
         {
            if (other == null) return 1;
            var objTuple = other as ClassTuple<T1, T2>;

            if (objTuple == null)
               throw new ArgumentException("ArgumentException_TupleIncorrectType", nameof(other));

            var c = comparer.Compare(_item1, objTuple._item1);
            return c != 0 ? c : comparer.Compare(_item2, objTuple._item2);
         }

         public override int GetHashCode()
         {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
         }

         int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
         {
            return CombineHashCodes(comparer.GetHashCode(_item1), comparer.GetHashCode(_item2));
         }

         private static int CombineHashCodes(int h1, int h2)
         {
            return ((h1 << 5) + h1) ^ h2;
         }
      }

      public struct StructTuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable
      {
         private readonly T1 _item1;
         private readonly T2 _item2;

         public T1 Item1 => _item1;
         public T2 Item2 => _item2;

         public StructTuple(T1 item1, T2 item2)
         {
            _item1 = item1;
            _item2 = item2;
         }

         public override bool Equals(object obj)
         {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
         }

         bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
         {
            if (!(other is StructTuple<T1, T2>)) return false;
            var objTuple = (StructTuple<T1, T2>)other;
            return comparer.Equals(_item1, objTuple._item1) && comparer.Equals(_item2, objTuple._item2);
         }

         int IComparable.CompareTo(object obj)
         {
            return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
         }

         int IStructuralComparable.CompareTo(object other, IComparer comparer)
         {
            var objTuple = (StructTuple<T1, T2>)other;
            var c = comparer.Compare(_item1, objTuple._item1);
            return c != 0 ? c : comparer.Compare(_item2, objTuple._item2);
         }

         public override int GetHashCode()
         {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
         }

         int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
         {
            return CombineHashCodes(comparer.GetHashCode(_item1), comparer.GetHashCode(_item2));
         }

         private static int CombineHashCodes(int h1, int h2)
         {
            return ((h1 << 5) + h1) ^ h2;
         }
      }

      private struct MyStructA : IComparable
      {
         public MyStructA(int i, string s)
         {
            I = i;
            S = s;
         }

         public readonly int I;
         public readonly string S;

         public int CompareTo(object obj)
         {
            var y = (MyStructA)obj;
            var c = I.CompareTo(y.I);
            return c != 0 ? c : StringComparer.Ordinal.Compare(S, y.S);
         }
      }

      private struct MyStructB
      {
         public MyStructB(int i, string s)
         {
            I = i;
            S = s;
         }

         public readonly int I;
         public readonly string S;
      }

      private class MyStructBComparer : IComparer<MyStructB>
      {
         public static readonly MyStructBComparer Instance = new MyStructBComparer();

         public int Compare(MyStructB x, MyStructB y)
         {
            var c = x.I.CompareTo(y.I);
            return c != 0 ? c : StringComparer.Ordinal.Compare(x.S, y.S);
         }
      }
   }
}