﻿using Pathfindax.Grid;
using Pathfindax.Nodes;
using Pathfindax.PathfindEngine;
using Pathfindax.Primitives;

namespace Duality.Plugins.Pathfindax.PathfindEngine
{
    public class NonGridPathfinderProxy : PathfinderProxy<ISourceNode, ISourceNodeNetwork<ISourceNode>>
    {
        /// <summary>
        /// Requests a new path
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="agentSize"></param>
        /// <param name="collisionLayer"></param>
        public PathRequest RequestPath(PositionF start, PositionF end, byte agentSize = 1, PathfindaxCollisionCategory collisionLayer = PathfindaxCollisionCategory.None)
        {
            var startNode = Pathfinder.SourceNodeNetwork.GetNode(start);
            var endNode = Pathfinder.SourceNodeNetwork.GetNode(end);
            return new PathRequest(Pathfinder, startNode, endNode, agentSize, collisionLayer);
        }

        /// <summary>
        /// Requests a new path
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="agentSize"></param>
        /// <param name="collisionLayer"></param>
        public PathRequest RequestPath(ISourceNode start, ISourceNode end, byte agentSize = 1, PathfindaxCollisionCategory collisionLayer = PathfindaxCollisionCategory.None)
        {
            return new PathRequest(Pathfinder, start, end, agentSize, collisionLayer);
        }
    }
}
