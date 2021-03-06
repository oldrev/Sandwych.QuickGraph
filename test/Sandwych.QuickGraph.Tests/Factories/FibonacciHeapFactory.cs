// <copyright file="FibonacciHeapFactory.cs" company="MSIT">Copyright © MSIT 2007</copyright>

using System;
using QuickGraph.Collections;

namespace QuickGraph.Collections
{
    public static class FibonacciHeapFactory
    {
        public static FibonacciHeap<int, int> Create()
        {
            FibonacciHeap<int, int> fibonacciHeap
               = new FibonacciHeap<int, int>();
            return fibonacciHeap;
        }
    }
}
