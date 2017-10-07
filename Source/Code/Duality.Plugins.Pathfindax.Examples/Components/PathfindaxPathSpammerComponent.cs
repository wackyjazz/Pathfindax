﻿using System;
using System.Linq;
using Duality.Drawing;
using Duality.Editor;
using Duality.Plugins.Pathfindax.Extensions;
using Duality.Plugins.Pathfindax.PathfindEngine;
using Pathfindax.Grid;
using Pathfindax.Nodes;
using Pathfindax.PathfindEngine;
using Pathfindax.Primitives;
using Pathfindax.Utils;

namespace Duality.Plugins.Pathfindax.Examples.Components
{
    /// <summary>
    /// Spams path requests as a example
    /// Use the <see cref="TopLeftCorner"/> and <see cref="BottomRightCorner"/> properties to control where it will spam the path requests.
    /// </summary>
    [EditorHintCategory(PathfindaxStrings.PathfindaxTest)]
    public class PathfindaxPathSpammerComponent : Component, ICmpUpdatable, ICmpRenderer, ICmpInitializable
    {
        public byte AgentSize { get; set; }
        public PathfindaxCollisionCategory CollisionCategory { get; set; }
        public Point2 TopLeftCorner { get; set; }
        public Point2 BottomRightCorner { get; set; }
        public Vector2[] Path { get; private set; }

        [EditorHintRange(1, 1000)]
        public int FramesBetweenRequest { get; set; }

        [EditorHintFlags(MemberFlags.Invisible)]
        public float BoundRadius { get; }

        private GridPathfinderProxy _gridPathfinderProxy;

        private readonly Random _randomGenerator = new Random();
        private int _frameCounter;
        void ICmpUpdatable.OnUpdate()
        {
            if (_frameCounter >= FramesBetweenRequest)
            {
                var start = new PositionF(_randomGenerator.Next(TopLeftCorner.X, BottomRightCorner.X), _randomGenerator.Next(TopLeftCorner.Y, BottomRightCorner.Y));
                var end = new PositionF(_randomGenerator.Next(TopLeftCorner.X, BottomRightCorner.X), _randomGenerator.Next(TopLeftCorner.Y, BottomRightCorner.Y));
                var request = _gridPathfinderProxy.RequestPath(start, end, AgentSize, CollisionCategory);
                request.AddCallback(PathSolved);
                _frameCounter = 0;
            }
            _frameCounter++;
        }

        private void PathSolved(PathRequest pathRequest)
        {
            Path = pathRequest.CompletedPath?.Path.Select(p => p.ToVector2()).ToArray();
        }

        bool ICmpRenderer.IsVisible(IDrawDevice device)
        {
            return
    (device.VisibilityMask & VisibilityFlag.AllGroups) != VisibilityFlag.None &&
    (device.VisibilityMask & VisibilityFlag.ScreenOverlay) == VisibilityFlag.None;
        }

        void ICmpRenderer.Draw(IDrawDevice device)
        {
            if (Path != null)
            {
                var canvas = new Canvas(device);
                canvas.State.ZOffset = -10;

                for (var index = 0; index < Path.Length; index++)
                {
                    if (index == 0) canvas.State.ColorTint = ColorRgba.Green;
                    else if (index == Path.Length - 1) canvas.State.ColorTint = ColorRgba.Blue;
                    else canvas.State.ColorTint = ColorRgba.Red;
                    var position = Path[index];
                    canvas.FillCircle(position.X, position.Y, 10f);
                }

                canvas.State.ColorTint = ColorRgba.Red;
                for (var i = 1; i < Path.Length; i++)
                {
                    var from = Path[i - 1];
                    var to = Path[i];
                    canvas.DrawLine(from.X, from.Y, 0, to.X, to.Y, 0);
                }
            }
        }

        public void OnInit(InitContext context)
        {
            if (context == InitContext.Activate && DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
            {
                _gridPathfinderProxy = new GridPathfinderProxy();
            }
        }

        public void OnShutdown(ShutdownContext context) { }
    }
}
