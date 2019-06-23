﻿using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Collections.Pooled.Benchmarks.PooledStack
{
    [Config(typeof(BenchmarkConfig))]
    public class Stack_Pop : StackBase
    {
        [Benchmark(Baseline = true)]
        public void StackPop()
        {
            if (Type == StackType.Int)
            {
                for (int i = 0; i < N; i++)
                {
                    _ = intStack.Pop();
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    _ = stringStack.Pop();
                }
            }
        }

        [Benchmark]
        public void PooledPop()
        {
            if (Type == StackType.Int)
            {
                for (int i = 0; i < N; i++)
                {
                    _ = intPooled.Pop();
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    _ = stringPooled.Pop();
                }
            }
        }

        [Params(10_000, 100_000)]
        public int N;

        [Params(StackType.Int, StackType.String)]
        public StackType Type;

        private Stack<int> intStack;
        private Stack<string> stringStack;
        private PooledStack<int> intPooled;
        private PooledStack<string> stringPooled;
        private int[] numbers;
        private string[] strings;

        [GlobalSetup]
        public void GlobalSetup()
        {
            numbers = CreateArray(N);

            if (Type == StackType.String)
            {
                strings = Array.ConvertAll(numbers, x => x.ToString());
            }
        }

        [IterationSetup]
        public void IterationSetup()
        {
            if (Type == StackType.Int)
            {
                intStack = new Stack<int>(numbers);
                intPooled = new PooledStack<int>(numbers);
            }
            else
            {
                stringStack = new Stack<string>(strings);
                stringPooled = new PooledStack<string>(strings);
            }
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            intPooled?.Dispose();
            stringPooled?.Dispose();
        }
    }
}
