﻿using System.Collections;
using NUnit.Framework;
using Pathfindax.Collections;

namespace Pathfindax.Test.Tests.Collections
{
	[TestFixture]
	class MaxHeapTests
	{
		[Test, TestCaseSource(typeof(MaxHeapTests), nameof(HeapTestCases))]
		public void MaxHeap_RemoveFirst(ValueHeapItem<int>[] items)
		{
			//Create the heap and add the items.
			var heap = new MaxHeap<ValueHeapItem<int>>(items.Length);
			for (var i = 0; i < items.Length; i++)
			{
				heap.Add(items[i]);
			}

			//Check if the items are in the correct order.
			var previousValue = int.MaxValue;
			for (var i = 0; i < items.Length; i++)
			{
				var value = heap.RemoveFirst().Value;
				if (value <= previousValue) previousValue = value;
				else Assert.Fail($"Value {value} is bigger than previous value {previousValue}");
			}
		}

		[Test, TestCaseSource(typeof(MaxHeapTests), nameof(HeapTestCases))]
		public void MaxHeap_Contains_True(ValueHeapItem<int>[] items)
		{
			//Create the heap and add the items.
			var heap = new MaxHeap<ValueHeapItem<int>>(items.Length);
			for (var i = 0; i < items.Length; i++)
			{
				heap.Add(items[i]);
			}

			//Check if all added items are contained in the heap.
			for (var i = 0; i < items.Length; i++)
			{
				Assert.IsTrue(heap.Contains(items[i]), $"Contains returned false for item {items[i]}");
			}
		}

		[Test, TestCaseSource(typeof(MaxHeapTests), nameof(HeapTestCases))]
		public void MaxHeap_Contains_False(ValueHeapItem<int>[] items)
		{
			//Create the heap and add the items.
			var heap = new MaxHeap<ValueHeapItem<int>>(items.Length);
			for (var i = 0; i < items.Length; i++)
			{
				heap.Add(items[i]);
			}

			//Create new items and check if contains returns false
			for (var i = 0; i < items.Length; i++)
			{
				var item = new ValueHeapItem<int>(items[i].Value);
				Assert.IsFalse(heap.Contains(item), $"Contains returned true for item {item}");
			}
		}

		public static IEnumerable HeapTestCases
		{
			get
			{
				yield return GenerateHeapTestCase(3, 5, 1);
				yield return GenerateHeapTestCase(0, 1, 2);
				yield return GenerateHeapTestCase(10, 9, 8);
				yield return GenerateHeapTestCase(100, 10, 1);
				yield return GenerateHeapTestCase(58, 72, 1, 0, 5342, 5932, 9999);
			}
		}

		private static TestCaseData GenerateHeapTestCase(params int[] values)
		{
			var testCaseData = new ValueHeapItem<int>[values.Length];
			for (var i = 0; i < values.Length; i++)
			{
				testCaseData[i] = new ValueHeapItem<int>(values[i]);
			}
			return new TestCaseData(new[] { testCaseData }).SetName($"Values: {string.Join(", ", values)}");
		}
	}
}
