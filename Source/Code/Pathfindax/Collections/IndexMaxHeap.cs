﻿using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Pathfindax.Collections
{
	public interface IIndexHeapItem<T>
	{
		bool HasHigherPriority(in T other);
	}

	/// <summary>
	/// A fast maxheap that is used as a priority queue for pathfinding.
	/// Does not store the items itself but keeps references to them with array indexes.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class IndexMaxHeap<T>
		where T : IIndexHeapItem<T>
	{
		/// <summary>
		/// The current amount of items in the heap.
		/// </summary>
		public int Count { get; private set; }
		public int Capacity => _indexes.Length;
		public ReadOnlyCollection<int> Indexes => new ReadOnlyCollection<int>(_indexes);
		private int[] _indexes;
		private int[] _heapIndexes;
		private T[] _array;

		public IndexMaxHeap(T[] array)
		{
			Initialize(array, array.Length);
		}

		public IndexMaxHeap(int size)
		{
			Initialize(null, size);
		}

		private void Initialize(T[] array, int size)
		{
			_array = array;
			_indexes = new int[size];
			_heapIndexes = new int[size];
			for (int i = 0; i < _indexes.Length; i++)
			{
				_indexes[i] = -1;
			}

			for (int i = 0; i < _heapIndexes.Length; i++)
			{
				_heapIndexes[i] = -1;
			}
		}

		public void AssignArray(T[] array)
		{
			_array = array;
			Count = 0;
		}

		/// <summary>
		/// Adds the item that is at the specified <paramref name="index"/> to the heap
		/// </summary>
		/// <exception cref="IndexOutOfRangeException">If the interal array is full</exception>
		/// <param name="index"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(int index)
		{
			_heapIndexes[index] = Count;
			_indexes[Count] = index;
			SortUp(index);
			Count++;
		}

		/// <summary>
		/// Removes the first item from the heap. 
		/// Since this is a maxheap it will have the highest value which is determined by the implementation of the <see cref="IComparable{T}"/> interface.
		/// </summary>
		/// <returns></returns>
		public int RemoveFirst()
		{
			var firstItem = _indexes[0];
			Count--;
			var lastIndex = _indexes[Count];
			_indexes[0] = lastIndex;
			_heapIndexes[lastIndex] = 0;
			SortDown(lastIndex);
			return firstItem;
		}

		/// <summary>
		/// Returns the index of the first item from the heap but does not remove it.
		/// </summary>
		/// <returns></returns>
		public int Peek()
		{
			return _indexes[0];
		}

		/// <summary>
		/// Returns true if this heap contains the specified index
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(int index)
		{
			var heapIndex = _heapIndexes[index];
			if (heapIndex < 0 || heapIndex >= Count) return false;
			return _indexes[heapIndex] == index;
		}

		private void SortDown(int itemIndex)
		{
			ref var item = ref _array[itemIndex];
			var itemHeapIndex = _heapIndexes[itemIndex];
			while (true)
			{
				var childIndexLeft = itemHeapIndex * 2 + 1;
				var childIndexRight = childIndexLeft + 1;

				if (childIndexLeft < Count)
				{
					var swapHeapIndex = childIndexLeft;
					if (childIndexRight < Count && _array[_indexes[childIndexLeft]].HasHigherPriority(_array[_indexes[childIndexRight]]) == false)
					{
						swapHeapIndex = childIndexRight;
					}

					var swapItemIndex = _indexes[swapHeapIndex];
					ref var swapItem = ref _array[_indexes[swapHeapIndex]];
					if (swapItem.HasHigherPriority(item))
					{
						_indexes[itemHeapIndex] = swapItemIndex;
						_heapIndexes[itemIndex] = swapHeapIndex;

						_heapIndexes[swapItemIndex] = itemHeapIndex;

						itemHeapIndex = swapHeapIndex;
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
			_indexes[itemHeapIndex] = itemIndex;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SortUp(int itemIndex)
		{	
			ref var item = ref _array[itemIndex];
			var parentItemHeapIndex = _heapIndexes[itemIndex];
			do
			{
				parentItemHeapIndex = (parentItemHeapIndex - 1) / 2;
				var parentItemIndex = _indexes[parentItemHeapIndex];

				ref var parentItem = ref _array[parentItemIndex];
				if (parentItem.HasHigherPriority(item))
					break;

				var itemHeapIndex = _heapIndexes[itemIndex];
				_indexes[itemHeapIndex] = parentItemIndex;
				_heapIndexes[parentItemIndex] = itemHeapIndex;

				_heapIndexes[itemIndex] = parentItemHeapIndex;

			} while (parentItemHeapIndex >= 1);
			_indexes[_heapIndexes[itemIndex]] = itemIndex;			
		}

		public void Update(int index)
		{
			SortUp(index);
		}
	}

	public struct HeapStruct : IIndexHeapItem<HeapStruct>
	{
		public int Value { get; }

		public HeapStruct(int value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public bool HasHigherPriority(in HeapStruct other)
		{
			return Value > other.Value;
		}
	}
}
