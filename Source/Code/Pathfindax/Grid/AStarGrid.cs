﻿using System.Linq;
using Pathfindax.Collections;
using Pathfindax.Nodes;

namespace Pathfindax.Grid
{
	/// <summary>
	/// Contains data specific for the A* algorithm.
	/// Do not share this between threads.
	/// </summary>
	public class AStarGrid : NodeGridBase<IAStarGridNode>
	{
		public AStarGrid(INodeGrid<IGridNode> source)
		{
			Offset = source.Offset;
			NodeSize = source.NodeSize;
			NodeArray = new Array2D<IAStarGridNode>(source.NodeArray.Width, source.NodeArray.Height);
			WorldSize = source.WorldSize;
			for (int y = 0; y < source.NodeArray.Height; y++)
			{
				for (int x = 0; x < source.NodeArray.Width; x++)
				{
					var sourceNode = source.NodeArray[x, y];
					var aStarNode = new AstarGridNode(this, sourceNode);
					NodeArray[x, y] = aStarNode;
				}
			}

			for (int y = 0; y < source.NodeArray.Height; y++)
			{
				for (int x = 0; x < source.NodeArray.Width; x++)
				{
					var aStarNode = NodeArray[x, y];
					var sourceNode = source.NodeArray[x, y];
					foreach (var sourceNodeNeighbour in sourceNode.Connections.Where(n => n != null))
					{
						aStarNode.Connections.Add(new NodeConnection<IAStarGridNode>(aStarNode, NodeArray[sourceNodeNeighbour.To.GridX, sourceNodeNeighbour.To.GridY], sourceNodeNeighbour.CollisionCategory));
					}
				}
			}
		}
	}
}
