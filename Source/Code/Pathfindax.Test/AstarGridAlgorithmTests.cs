﻿using NUnit.Framework;
using Pathfindax.Algorithms;
using Pathfindax.Grid;
using Pathfindax.PathfindEngine;
using Pathfindax.Primitives;

namespace Pathfindax.Test
{
	[TestFixture]
	public class AstarGridAlgorithmTests
	{
		[Test, TestCaseSource(typeof(AstarGridAlgorithmCases), nameof(AstarGridAlgorithmCases.FindPathTestCases))]
		[MaxTime(2000)]
		public void FindPath_InitializedNodegrid_PathLengthIsNot0(AstarNodeGrid sourceNodeGrid, float x1, float y1, float x2, float y2)
		{
			var aStarAlgorithm = new AStarGridAlgorithm();
			var start = sourceNodeGrid.SourceSourceNodeGrid.GetNode(new PositionF(x1, y1));
			var end = sourceNodeGrid.SourceSourceNodeGrid.GetNode(new PositionF(x2, y2));
			var pathRequest = new PathRequest(start, end);
			var path = aStarAlgorithm.FindPath(sourceNodeGrid, pathRequest);
			Assert.AreEqual(path.Count > 0, true);
		}


	}
}
