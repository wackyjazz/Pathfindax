﻿using System.Collections;
using System.Collections.Generic;

namespace Pathfindax.Collections
{
	public class LookupArray : IEnumerable<int>
	{
		public int Capacity => _spots.Length;
		private byte _generation;
		private readonly byte[] _spots;
		public LookupArray(int size)
		{
			_spots = new byte[size];
			_generation = 1;
		}

		public void Occupy(int index)
		{
			_spots[index] = _generation;
		}

		public void Remove(int index)
		{
			_spots[index] = 0;
		}

		public void Clear()
		{
			if (_generation == byte.MaxValue)
			{
				_generation = 1;
				for (int i = 0; i < _spots.Length; i++)
				{
					Remove(i);
				}
			}
			else
			{
				_generation++;
			}			
		}

		public bool Contains(int index)
		{
			return _spots[index] == _generation;
		}

		public IEnumerator<int> GetEnumerator()
		{
			for (var i = 0; i < _spots.Length; i++)
			{
				if (_spots[i] == _generation) yield return i;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
